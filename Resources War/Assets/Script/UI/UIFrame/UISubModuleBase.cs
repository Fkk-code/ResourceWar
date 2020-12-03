using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilty;

namespace UIFrame
{
    public class UISubModuleBase : MonoBehaviour
    {
        public Dictionary<string, UIWidgetsBase> _uiWidgets;
        [Header("当前模块重要元件所添加的脚本名称")]
        //当前元件脚本
        public string currentModuleWidgetScript = "UIFrame.UIWidgetsBase";
        protected virtual void Awake()
        {
            _uiWidgets = new Dictionary<string, UIWidgetsBase>();
            LoadImportentWidgets();

        }
        private void LoadImportentWidgets()
        {
            //当前widget是否继承基类或者就是基类本身
            if (!ReflectionManager.GetInstance().TypeIsExtentOrEquel
                (currentModuleWidgetScript, "UIWidgetsBase"))
            {
                //命名不存在，依旧使用基类组件
                currentModuleWidgetScript = "UIFrame.UIWidgetsBase";
            }
            //获取当前对象的所有子对象
            Transform[] allChild = transform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < allChild.Length; i++)
            {
                for (int j = 0; j < SystemDefine.IMPROTANT_WIDGET_TOKEN.Length; j++)
                {//判断子对象是否为该符号结尾
                    if (allChild[i].name.EndsWith(SystemDefine.IMPROTANT_WIDGET_TOKEN[j]))
                    {
                        //获取脚本组件
                        UIWidgetsBase uiWidget = allChild[i].gameObject.AddComponent(
                            Type.GetType(currentModuleWidgetScript)) as UIWidgetsBase;
                        //设置Widgets归属为当前模块
                        uiWidget._currentModule = null;
                        //添加到字典
                        _uiWidgets.Add(allChild[i].name, uiWidget);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 通过元件名称查找当前模块的元件
        /// </summary>
        /// <param name="widgetName"></param>
        public UIWidgetsBase FindCurrentModuleWidget(string widgetName)
        {
            if (!_uiWidgets.ContainsKey(widgetName))
            {
                Debug.LogError($"查找的{widgetName}不存在");
                return null;
            }
            return _uiWidgets[widgetName];
        }
    }
}
