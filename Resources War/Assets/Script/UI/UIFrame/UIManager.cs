using System.Collections.Generic;
using UnityEngine;
using Utilty;
namespace UIFrame
{
    public class UIManager : Singleton<UIManager>
    {
        /// <summary>
        /// 所管理的所有UI模块
        /// </summary>
        private Dictionary<string, UIModuleBase> _uiModules;

        private Stack<UIModuleBase> _uiModuleStack;
        private Transform _canvas;
        private UIManager()
        {
            _uiModuleStack = new Stack<UIModuleBase>();
            _uiModules = new Dictionary<string, UIModuleBase>();
            _canvas = GameObject.FindWithTag("Canvas").transform;
        }

        #region Load & UnLoad

        
        private bool LoadModule(string moduleName,Vector2? anchoredPosition)
        {
            //TEST防止重复加载
            if (_uiModules.ContainsKey(moduleName))
            {
                _uiModules[moduleName].OnOpen();
                return true;
            }
            //字典中根本没有该模块 或 字典里该模块值为空
            if (!_uiModules.ContainsKey(moduleName) ||
               _uiModules[moduleName] == null)
            {
                string panelPath = UIConfigurationManager.GetInstance().GetPanelAssetPathByName(moduleName);
                GameObject module;
                if (anchoredPosition == null)
                {
                    module = PrefabManager.GetInstance().CreateGameObjectByPrefab
                   (panelPath, _canvas);
                }
                else
                {
                    module = PrefabManager.GetInstance().CreateGameObjectByPrefab
                   (panelPath, _canvas, (Vector2)anchoredPosition);
                }
               
               //去掉（Clone）
                module.name = moduleName;
                UIModuleBase moduleBase = module.GetComponent<UIModuleBase>();
                
                //如果为空相当于添加并赋值
                _uiModules[moduleName] = moduleBase;
            }
            return false;
        }
        /// <summary>
        /// 加载模块
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>该模块是否已经加载过</returns>
        private bool LoadModule(string moduleName)
        {
            //TEST防止重复加载
            if (_uiModules.ContainsKey(moduleName))
            {
                _uiModules[moduleName].OnOpen();
                return true;
            }
            //字典中根本没有该模块 或 字典里该模块值为空
            if (!_uiModules.ContainsKey(moduleName) ||
               _uiModules[moduleName] == null)
            {
                string panelPath = UIConfigurationManager.GetInstance().GetPanelAssetPathByName(moduleName);
                GameObject module = PrefabManager.GetInstance().
                    CreateGameObjectByPrefab(panelPath, _canvas);
                //去掉（Clone）
                module.name = moduleName;
                UIModuleBase moduleBase = module.GetComponent<UIModuleBase>();

                //如果为空相当于添加并赋值
                _uiModules[moduleName] = moduleBase;
                
            }
            return false;
        }
        private void UnLoadModule(string moduleName)
        {
            if (!_uiModules.ContainsKey(moduleName))
            {
                Debug.LogError("当前模块不存在，无法卸载");
                return;
            }
            //获取该模块
            UIModuleBase uIModuleBase = _uiModules[moduleName];
            //从字典里面移除掉
            _uiModules.Remove(moduleName);
            Object.Destroy(uIModuleBase.gameObject);
        }
        #endregion

        #region Find Module Widget

        /// <summary>
        /// 跨模块获取某个原件[确保要找的模块已经被加载]
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="widgetName"></param>
        /// <returns></returns>
        public UIWidgetsBase FindWidget(string moduleName,string widgetName)
        {
            if (!_uiModules.ContainsKey(moduleName)
                 || _uiModules[moduleName] == null)
            {
                Debug.Log("当前模块不存在");
                return null;
            }
            return _uiModules[moduleName].FindCurrentModuleWidget(widgetName);
            
        }

        #endregion
      

        #region 模糊管理Test1.0

        /// <summary>
        /// 打开模块（忽略模块性质）
        /// </summary>
        /// <param name="moduleName">模块名字</param>
        /// <param name="localPos">初始化模块时，模块生成坐标</param>
        public void VagueOpenModule(string moduleName,Vector2? localPos)
        {
            
            if (localPos == null)
            {
                LoadModule(moduleName);
            }
            else
            {
                LoadModule(moduleName, localPos);
               
            }
            switch (_uiModules[moduleName].uIModuleType)
            {
                case UIModuleType.SingleControl:
                    PushModule(moduleName);
                    break;
                case UIModuleType.MultipleControl:
                    OpenModule(moduleName);
                    break;
                default:
                    break;
            }
        }
     
        #endregion

        #region 单窗口管理
        public void PushModule(string moduleName)
        {
            LoadModule(moduleName, Vector2.zero);
            if (_uiModules[moduleName].uIModuleType == UIModuleType.MultipleControl)
            {
                Debug.LogError("请选多窗口模式");
                UnLoadModule(moduleName);
                return;
            }

            //如果栈不为空
            if (_uiModuleStack.Count != 0)
            {
                //TEST
                //栈顶元素暂停
                //for (int i = 0; i < _uiModuleStack.Count; i++)
                //{
                //    _uiModuleStack.ToArray()[i].OnPause();
                //}
                _uiModuleStack.Peek().OnPause();
            }
            //新元素压栈
            _uiModuleStack.Push(_uiModules[moduleName]);
            //显示新元素
            _uiModuleStack.Peek().OnOpen();

        }
        public void PopModule()
        {
            if (_uiModuleStack.Count == 0)
            {
                return;
            }
            _uiModuleStack.Pop().OnClose();
            if (_uiModuleStack.Count == 0)
            {
                return;
            }
            //TEST
            //for (int i = 0; i < _uiModuleStack.Count; i++)
            //{
            //    _uiModuleStack.ToArray()[i].OnResume();
            //}
            //新栈顶模块执行恢复方法
            _uiModuleStack.Peek().OnResume();
          
        }
        /// <summary>
        /// 弹出模块 【主模块保留】
        /// </summary>
        /// <param name="mainStay">是否保留主模块</param>
        public void PopModule(bool mainStay)
        {
            if (mainStay && _uiModuleStack.Count == 1)
            {
                return;
            }
            PopModule();
        }
        #endregion


        #region 多窗口

        /// <summary>
        /// 打开多窗口模式的模块
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="anchoredPosition"></param>
        public void OpenModule(string moduleName)
        {
             LoadModule(moduleName);
            if (_uiModules[moduleName].uIModuleType == UIModuleType.SingleControl)
            {
                Debug.Log("请选单窗口模式");
                UnLoadModule(moduleName);
                return;
            }
            _uiModules[moduleName].OnOpen();
        }

        /// <summary>
        /// 关闭多窗口模式模块
        /// </summary>
        /// <param name="moduleName"></param>
        public void CloseModule(string moduleName)
        {
            if (!_uiModules.ContainsKey(moduleName))
            {
                Debug.Log("当前模块不存在，无法关闭");
                return;
            }
            _uiModules[moduleName].OnClose();
        }
        #endregion
    }
}