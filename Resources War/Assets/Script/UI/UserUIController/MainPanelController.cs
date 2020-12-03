using UIFrame;
using Utilty;

public class MainPanelController : UIControllerBase
{
    //页面是否打开
    bool playerInfoIsOn = false;

    /// <summary>
    /// 控制器启动
    /// </summary>
    /// <param name="module"></param>
    public override void ControllerStart(UIModuleBase module)
    {
        base.ControllerStart(module);
        OpenPlayerInfoPanel();
    }
    /// <summary>
    /// 打开玩家信息界面
    /// </summary>
    private void OpenPlayerInfoPanel()
    {
        //找到“玩家信息”按钮
        UIWidgetsBase playerInfoBtn = _module.FindCurrentModuleWidget("PlayerInfo_Btn#");
        //绑定事件
        playerInfoBtn.Button.onClick.AddListener(() =>
        {
            if (playerInfoIsOn == false)
            {
                //打开玩家信息页面
                UIManager.GetInstance().OpenModule("PlayerInfoPanel");
            }
            else
            {
                //关闭玩家信息页面
                UIManager.GetInstance().CloseModule("PlayerInfoPanel");
            }
            playerInfoIsOn = !playerInfoIsOn;
        });
        //找到“仓库信息”按钮
        UIWidgetsBase warehousePanel_Btn = _module.FindCurrentModuleWidget("WarehousePanel_Btn#");
        //绑定事件
        warehousePanel_Btn.Button.onClick.AddListener(() =>
        {
            if (playerInfoIsOn == false)
            {
                //打开玩家信息页面
                UIManager.GetInstance().OpenModule("WarehousePanel");
            }
            else
            {
                //关闭玩家信息页面
                UIManager.GetInstance().CloseModule("WarehousePanel");
            }
            playerInfoIsOn = !playerInfoIsOn;
        });
    }
}
