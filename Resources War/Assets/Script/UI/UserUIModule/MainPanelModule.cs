using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;

public class MainPanelModule : UIModuleBase
{
    //主界面控制组件
    MainPanelController mainPanelController;
    protected void Start()
    {
        //绑定控制器
        if (mainPanelController == null)
        {
            mainPanelController = new MainPanelController();
            BindController(mainPanelController);
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnResume()
    {
        base.OnResume();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
