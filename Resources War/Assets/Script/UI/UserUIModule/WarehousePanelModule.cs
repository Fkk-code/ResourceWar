using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;

public class WarehousePanelModule : UIModuleBase
{
    WarehousePanelController warehousePanelController;

    void Start()
    {
        if (warehousePanelController == null)
        {
            warehousePanelController = new WarehousePanelController();
            //绑定控制器
            BindController(warehousePanelController);
        }
    }

}
