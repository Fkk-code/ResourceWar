using System.Collections;

using UIFrame;


public class KnapsackPanelController : UIControllerBase
{
    /// <summary>
    /// 启动控制器
    /// </summary>
    /// <param name="module"></param>
    public override void ControllerStart(UIModuleBase module)
    {
        base.ControllerStart(module);
        //关闭背包页面
        CloseKnapsackPanel();
    }


    /// <summary>
    /// 关闭背包页面
    /// </summary>
    private void CloseKnapsackPanel()
    {
        //找到关闭按钮
        UIWidgetsBase sortBtn = _module.FindCurrentModuleWidget("Sort_Btn#");
        //给按钮绑定事件
        sortBtn.Button.onClick.AddListener(() =>
        {
            UIManager.GetInstance().CloseModule("KnapsackPanel");
        });
    }
}
