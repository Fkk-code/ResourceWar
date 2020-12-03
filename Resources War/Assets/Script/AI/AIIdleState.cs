using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : StateTemplate<AIControl>
{
    /// <summary>
    /// 思考状态
    ///     1.血量是否合适？吃血药回血（只能吃3次）
    ///     2.是否可以攻击到目标？寻找防御最低的单位攻击
    ///     3.没有可攻击的目标向距离最近的敌方单位移动
    /// </summary>

    public AIIdleState(int id, AIControl owner) : base(id, owner){}

    public override void OnStart()
    {
        base.OnStart();
        if (!BattleManager.instance.IsMove)
        {
            Machine.ReplaceState(1);
            BattleManager.instance.IsMove = true;
        }
        else if (!BattleManager.instance.IsAction)
        {
            Machine.ReplaceState(2);
            BattleManager.instance.IsAction = true;
        }
        else
        {
            //结束回合
            BattleManager.instance.NextRound();
        }
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}