using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrame;

public class StartPanelModule : UIModuleBase
{
    StartPanelController startPanelController;

    void Start()
    {
        if (startPanelController == null)
        {
            startPanelController = new StartPanelController();
            //绑定控制器
            BindController(startPanelController);
        }
        StartCoroutine(ShowTextAndButton());
    }

    IEnumerator ShowTextAndButton()
    {
        for (int i = 0; i < 14; i++)
        {
            yield return new WaitForSeconds(0.2f);
            startPanelController.ShowTitle(i);
        }
    }
}
