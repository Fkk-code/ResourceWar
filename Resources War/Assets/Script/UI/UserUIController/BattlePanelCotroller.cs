using UnityEngine;
using UIFrame;
using Utilty;

public class BattlePanelCotroller : UIControllerBase
{
    /// <summary>
    /// 启动控制器
    /// </summary>
    /// <param name="module"></param>
    public override void ControllerStart(UIModuleBase module)
    {
        base.ControllerStart(module);
        //初始化界面
        Normal();
        Action();
    }
    public void ChargeView(int i)
    {
        UIWidgetsBase EnemyRound = _module.FindCurrentModuleWidget("EnemyRound#");
        UIWidgetsBase ActionlButtonManager = _module.FindCurrentModuleWidget("ActionlButtonManager#");
        UIWidgetsBase NormalButtonManager = _module.FindCurrentModuleWidget("NormalButtonManager#");
        switch (i)
        {
            case 1:
                //敌人
                EnemyRound.gameObject.SetActive(true);
                ActionlButtonManager.gameObject.SetActive(false);
                NormalButtonManager.gameObject.SetActive(false);
                break;
            case 2:
                //行动
                EnemyRound.gameObject.SetActive(false);
                ActionlButtonManager.gameObject.SetActive(true);
                NormalButtonManager.gameObject.SetActive(false);
                break;
            case 3:
                //普通
                EnemyRound.gameObject.SetActive(false);
                ActionlButtonManager.gameObject.SetActive(false);
                NormalButtonManager.gameObject.SetActive(true);
                break;
        }
    }
    /// <summary>
    /// 普通状态
    /// </summary>
    /// <param name="module"></param>
    public void Normal()
    {
        //添加按钮事件
        _module.FindCurrentModuleWidget("MoveBtn#").Button.onClick.AddListener(() =>
        {
            Debug.Log("IsMove" + BattleManager.instance.IsMove);
            Debug.Log("IsAction" + BattleManager.instance.IsAction);
            if (BattleManager.instance.IsMove && !BattleManager.instance.IsAction)
            {
                BattleManager.instance.ReturnChessMove();
            }
            else if(!BattleManager.instance.IsMove)
            {
                BattleManager.instance.ShowMoveArea();
            }
        });

        _module.FindCurrentModuleWidget("ActionBtn#").Button.onClick.AddListener(() =>
        {
            //进入行动
            if (!BattleManager.instance.IsAction)
                BattleManager.instance.showChessState = BattleManager.ShowChessState.选择行动;
        });
        _module.FindCurrentModuleWidget("StopBtn#").Button.onClick.AddListener(() =>
        {
            BattleManager.instance.NextRound();
            _module.FindCurrentModuleWidget("MoveTxt#").Text.text = "移动";
        });
        _module.FindCurrentModuleWidget("StateBtn#").Button.onClick.AddListener(() =>
        {
            //空
        });
    }
    public void Action()
    {
        //添加按钮事件
        _module.FindCurrentModuleWidget("AttackBtn#").Button.onClick.AddListener(() =>
        {
            BattleManager.instance.ShowAttackArea();
        });
        _module.FindCurrentModuleWidget("SkillBtn#").Button.onClick.AddListener(() =>
        {
            //进入行动
            BattleManager.instance.showChessState = BattleManager.ShowChessState.选择技能;
        });
        _module.FindCurrentModuleWidget("BagBtn#").Button.onClick.AddListener(() =>
        {

        });
        _module.FindCurrentModuleWidget("ReturnBtn#").Button.onClick.AddListener(() =>
        {
            //进入行动
            BattleManager.instance.showChessState = BattleManager.ShowChessState.普通;
        });
    }
}