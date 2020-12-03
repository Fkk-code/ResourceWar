using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;

public class EndBattlePanelModule : UIModuleBase
{
    EndBattlePanelController endBattlePanelController;

    void Start()
    {
        if (endBattlePanelController == null)
        {
            endBattlePanelController = new EndBattlePanelController();
            //绑定控制器
            BindController(endBattlePanelController);
        }
    }

    void Update()
    {
        
    }
}
