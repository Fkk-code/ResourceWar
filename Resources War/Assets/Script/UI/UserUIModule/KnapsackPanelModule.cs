using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;

public class KnapsackPanelModule : UIModuleBase
{
    KnapsackPanelController knapsackPanelController;

    void Start()
    {
        if (knapsackPanelController == null)
        {
            knapsackPanelController = new KnapsackPanelController();
            //绑定控制器
            BindController(knapsackPanelController);
        }
    }

}
