using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveState : StateTemplate<AIControl>
{
    /// <summary>
    /// 移动状态
    ///     1.是否移动到最大距离
    ///     2.是否移动结束
    /// </summary>

    public AIMoveState(int id, AIControl owner) : base(id, owner){}
    public override void OnStart()
    {
        base.OnStart();
        //制作路径栈
        Stack<Vector3> path = new Stack<Vector3>();
        //获取路径
        BoardManager.instance.AIAStar(
            owner._chessManager._chessMove.startPos,
            owner.enemyManager._chessMove.startPos,
            ref path,
            100);
        //打不到就发呆
        if (path.Count >= 0)
        {
            //推出起点
            path.Pop();
        }
        //制作队列
        Queue<Vector3> newPath = new Queue<Vector3>();
        //制作可移动的路径长度
        while(path.Count>0)
        {
            //出栈 然后 入队
            newPath.Enqueue(path.Pop());
        }
        Debug.Log("新路径长度" + newPath.Count);
        int lenth = 0;
        //判断是否到达可攻击距离
        if (newPath.Count > (owner._chessManager._chessState.moveMaxDictance + owner._chessManager._chessState.attackMaxDictance))
        {
            //攻击不到
            lenth = owner._chessManager._chessState.moveMaxDictance;
        }
        else
        {
            //正好攻击到
            //去除攻击距离
            lenth = newPath.Count - owner._chessManager._chessState.attackMaxDictance;
        }
        
        lenth = lenth > 0 ? lenth : 0;
        if (lenth == 0)
        {
            owner.WaitTime(0.5f);
        }
        else
        {
            //开启移动协成
            owner._chessManager._chessMove.AIMove(lenth, newPath);
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
