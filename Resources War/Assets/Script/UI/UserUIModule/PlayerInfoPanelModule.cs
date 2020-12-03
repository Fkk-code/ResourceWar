using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;

public class PlayerInfoPanelModule : UIModuleBase
{
    PlayerInfoPanelController playerInfoPanelController;

    void Start()
    {
        if (playerInfoPanelController == null)
        {
            playerInfoPanelController = new PlayerInfoPanelController();
            //绑定控制器
            BindController(playerInfoPanelController);
        }
    }

    void Update()
    {
        //关闭玩家信息页面
        //playerInfoPanelController.ClosePlayerPanel();
    }
}
