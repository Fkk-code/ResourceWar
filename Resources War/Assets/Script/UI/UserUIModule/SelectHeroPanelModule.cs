using UIFrame;
using UnityEngine;

public class SelectHeroPanelModule : UIModuleBase
{
    SelectHeroPanelController selectHeroPanelController;
    bool isOpen = false;
    void Update()
    { 
        if(isOpen && GameConst.GetInstance().PlayerchessPerfab.Count>0)
        {
            //更新参加人数
            selectHeroPanelController.UpdatePeople();
        }
    }
    public override void OnOpen()
    {
        base.OnOpen();
        isOpen = true;
        if (selectHeroPanelController == null)
        {
            selectHeroPanelController = new SelectHeroPanelController();
            //绑定控制器
            BindController(selectHeroPanelController);
        }
        selectHeroPanelController.RefreshInfo();
    }
    public override void OnClose()
    {
        base.OnClose();
        isOpen = false;
    }
}
