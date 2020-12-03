using System.Collections.Generic;
using UnityEngine;
using System;
using Utilty;
using UnityEngine.UI;

namespace UIFrame
{
    [RequireComponent(typeof(CanvasGroup))]
    /// <summary>
    /// UI模块基类，需挂载在模块的根对象上
    /// </summary>
    public abstract class UIModuleBase : MonoBehaviour
    {
        public UIModuleType uIModuleType = UIModuleType.SingleControl;
        public Dictionary<string, UIWidgetsBase> _uiWidgets;
        public Dictionary<string, UIWidgetsBase> _uiSecondWidgets;
        UIWidgetsBase[] secondWidgets;

        [Header("当前模块重要元件所添加的脚本名称")]
        //当前元件脚本
        public string currentModuleWidgetScript = "UIFrame.UIWidgetsBase";
        protected CanvasGroup _canvasGroup;

        protected virtual void Awake()
        {
            _uiWidgets = new Dictionary<string, UIWidgetsBase>();
            _uiSecondWidgets = new Dictionary<string, UIWidgetsBase>();
            _canvasGroup = GetComponent<CanvasGroup>();
            LoadImportentWidgetsAndAddLocalization();
            LoadSecondImportentWidgets();
            if (SystemDefine.UseLocalization)
            {
                InitModuleLanguage();
            }
            
        }
      
        private void InitModuleLanguage()
        {
            int id = PlayerPrefs.GetInt(SystemDefine.PLAYERPREFS_LANGUAGEID);
            UILocalizationTextManager.GetInstance().LoacalizationTextChange(id);
        }

        #region Module CallBacks

        /// <summary>
        /// 打开当前模块
        /// </summary>
        public virtual void OnOpen()
        {
            gameObject.SetActive(true);
            //当前模块可以和用户交互
            _canvasGroup.blocksRaycasts = true;
            //将当前对象设置为最后一个字对象,使其显示在所有窗口最上面
            transform.SetAsLastSibling();

        }

        /// <summary>
        /// 当前模块挂起
        /// </summary>
        public virtual void OnPause()

        {
            //if (uIModuleType == UIModuleType.SingleControl)
           // {
                _canvasGroup.blocksRaycasts = false;
           // }

        }

        /// <summary>
        /// 当前模块恢复
        /// </summary>
        public virtual void OnResume()
        {
            //if (uIModuleType == UIModuleType.SingleControl)
            //{
                _canvasGroup.blocksRaycasts = true;
            //}
        }

        /// <summary>
        /// 关闭当前模块
        /// </summary>
        public virtual void OnClose()
        {
            _canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }
        #endregion

        protected void BindController(UIControllerBase controllerBase)
        {
            controllerBase.ControllerStart(this);
        }
 
        private void LoadImportentWidgetsAndAddLocalization()
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
                        uiWidget._currentModule = this;
                       //添加到字典
                        _uiWidgets.Add(allChild[i].name,uiWidget);
                        break;
                    }
                }
                if (SystemDefine.UseLocalization)
                {
                    AddLocalizationTextComponent(allChild[i]);
                }
                
            }
        }
        private void LoadSecondImportentWidgets()
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
                //判断子对象是否为该符号结尾
                if (allChild[i].name.EndsWith(SystemDefine.SECONDWIDGETS_TOKEN))
                {
                    //获取脚本组件
                    UIWidgetsBase uiWidget = allChild[i].gameObject.AddComponent(
                        Type.GetType(currentModuleWidgetScript)) as UIWidgetsBase;
                    //设置Widgets归属为当前模块
                    uiWidget._currentModule = this;
                    //添加到字典
                    _uiSecondWidgets.Add(allChild[i].name, uiWidget);
                   // break;
                }
            }
        }
        public UIWidgetsBase[] GetSecondWidgets()
        {

            if (secondWidgets == null)
            {
                secondWidgets = new UIWidgetsBase[_uiSecondWidgets.Values.Count];
                _uiSecondWidgets.Values.CopyTo(secondWidgets, 0);
            }
            return  secondWidgets;
        }
       private void AddLocalizationTextComponent(Transform child)
        {
            //该组件没有Text 返回
            if (child.GetComponent<Text>() == null)
            {
                return;
            }//以$开头  返回
            if (child.name.StartsWith(SystemDefine.NotInvolvedInLocalization))
            {
                return;
            }
            //添加组件
            UILocalText localText = child.gameObject.AddComponent<UILocalText>();
            localText.textString = child.GetComponent<Text>().text;
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
        public UIWidgetsBase FindCurrentModuleSecondWidget(string widgetName)
        {
            if (!_uiSecondWidgets.ContainsKey(widgetName))
            {
                Debug.LogError($"查找的{widgetName}不存在");
                return null;
            }
            return _uiSecondWidgets[widgetName];
        }
        public void UnRegisterWidget(string widgetName)
        {
            if (!_uiWidgets.ContainsKey(widgetName))
            {
                Debug.LogError("当前模块不存在");
                return;
            }
            _uiWidgets.Remove(widgetName);
            Debug.Log($"{name}模块中的{widgetName}已经取消注册");
        }
    }
}