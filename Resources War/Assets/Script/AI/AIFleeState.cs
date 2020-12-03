using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFleeState : StateTemplate<AIControl>
{
    /// <summary>
    /// 逃跑状态
    ///     1.吃药
    ///     2.离开被攻击范围
    /// </summary>

    public AIFleeState(int id, AIControl owner) : base(id, owner)
    {
    }

    public override void OnStart()
    {
        base.OnStart();
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
