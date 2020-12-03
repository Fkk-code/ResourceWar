using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : StateTemplate<AIControl>
{
    /// <summary>
    /// 攻击状态
    ///     1.使用攻击还是技能
    ///     2.攻击是否结束
    /// </summary>

    public AIAttackState(int id, AIControl owner) : base(id, owner) { }
    public override void OnStart()
    {
        base.OnStart();
        //路径数组
        Stack<Vector3> path = new Stack<Vector3>();
        //获取路径
        BoardManager.instance.AIAStar(
            owner._chessManager._chessMove.startPos,
            owner.enemyManager._chessMove.startPos,
            ref path,
            100);
        Debug.Log(path.Count);
        //判断是否可以攻击
        if (path.Count > 0)
            path.Pop();
        if(path.Count <= owner._chessManager._chessState.attackMaxDictance)
        {
            //设置目标
            owner._chessManager._chessAttack.ChessAttackTaget(owner.enemyManager);
            //棋子转向
            owner._chessManager._chessMove.PlayerRoll(owner.enemyManager._chessMove.startPos);
            //启动动画
            owner._chessManager._chessAttack.PlayAnimation(owner._chessManager._chessAttack.NORMALATK);
        }
        //延迟然后切换状态
        owner.WaitTime(1f);
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
