using UIFrame;
using UnityEngine;

public class SelectHeroPanelModule : UIModuleBase
{
    SelectHeroPanelController selectHeroPanelController;
    bool isOpen = false;
    void Update()
    { 
        if(isOpen && GameConst.GetInstance().PlayerchessPerfab.Count>=0)
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
    public override void OnResume()
    {
        base.OnResume();
        selectHeroPanelController.RefreshInfo();
    }
    public void Destoryhero()
    {
        Transform tff = UIManager.GetInstance().FindWidget("SelectHeroPanel", "HeroList#").transform;
        for (int i = 0; i < tff.childCount; i++)
        {
            Destroy(tff.GetChild(i));
        }
    }
    public override void OnClose()
    {
        base.OnClose();
        Debug.Log("clossss");
        isOpen = false;
    }
}
