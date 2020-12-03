using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateEnum
{
    Idle,//思考
    Attack,//攻击
    Move,//移动
    Flee//逃跑
}
public class AIControl : MonoBehaviour
{
    //状态机
    private StateMachine sm;
    //设置状态
    public AIStateEnum AIState = AIStateEnum.Idle;
    //棋子组件
    public ChessManager _chessManager;
    //目标敌人组件
    public ChessManager enemyManager;
    void OnEnable()
    {
        //设置状态机
        sm = new StateMachine();
        //添加状态
        sm.RecordState(new AIIdleState(0, this));
        sm.RecordState(new AIMoveState(1, this));
        sm.RecordState(new AIAttackState(2, this));
    }
    /// <summary>
    /// AI开启
    /// </summary>
    public void StartAI()
    {
        Debug.Log("AIControl");
        //初始化
        _chessManager = GetComponent<ChessManager>();
        enemyManager = null;
        float min = 100000;
        //遍历敌人列表
        for (int i = 0; i < GameConst.GetInstance().PlayerchessPerfab.Count; i++)
        {
            if (GameConst.GetInstance().PlayerchessPerfab[i].GetComponent<ChessManager>()._chessState.Hp > 0)
            {
                if (Vector2Int.Distance(
                    GameConst.GetInstance().PlayerchessPerfab[i].GetComponent<ChessManager>()._chessMove.startPos,
                    _chessManager._chessMove.startPos) < min)
                {
                    min = Vector2Int.Distance(
                    GameConst.GetInstance().PlayerchessPerfab[i].GetComponent<ChessManager>()._chessMove.startPos,
                    _chessManager._chessMove.startPos);
                    enemyManager = GameConst.GetInstance().PlayerchessPerfab[i].GetComponent<ChessManager>();
                }
            }
        }
        //判断是否拿到攻击目标
        if (enemyManager == null)
        {
            Debug.Log("没有敌人了");
            //战斗结束
            BattleManager.instance.BattleEnd(100);
        }
        else
        {
            //AI开始
            StartCoroutine(wait(1f));
        }
    }
    public void WaitTime(float ttt)
    {
        StartCoroutine(wait(ttt));
    }
    IEnumerator wait(float ttt)
    {
        yield return new WaitForSeconds(ttt);
        sm.ReplaceState(0);
    }
}
