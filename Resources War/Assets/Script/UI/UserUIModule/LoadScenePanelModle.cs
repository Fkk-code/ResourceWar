using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
public class LoadScenePanelModle : UIModuleBase
{
    //主界面控制组件
    LoadScenePanelController loadScenePanelController;
    protected void Start()
    {
        //绑定控制器
        if (loadScenePanelController == null)
        {
            loadScenePanelController = new LoadScenePanelController();
            BindController(loadScenePanelController);
        }
    }
}
