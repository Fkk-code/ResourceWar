using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    //状态字典
    public Dictionary<int, StateBase> StateCache;
    //上一个状态
    public StateBase PreviousState;
    //当前状态
    public StateBase CurrentState;
    //构造函数
    public StateMachine()
    {
        //初始化状态字典
        StateCache = new Dictionary<int, StateBase>();
        //没有上个状态
        PreviousState = null;
        //设置为当前状态
        CurrentState = null;
    }
    //状态持续
    public void FSMUpdate()
    {
        if (CurrentState == null)
            return;
        CurrentState.OnUpdate();
    }
    //存储状态
    public void RecordState(StateBase stateBase)
    {
        //获取编号
        int id = stateBase.id;
        //判断状态是否存在
        if (StateCache.ContainsKey(id))
            return;
        //存储
        StateCache.Add(id, stateBase);
        //设置状态机
        stateBase.Machine = this;
    }
    //状态切换
    public void ReplaceState(int stateid)
    {
        //判断状态是否存在
        if (!StateCache.ContainsKey(stateid))
            return;
        //设置上个状态
        PreviousState = CurrentState;
        //设置当前状态
        CurrentState = StateCache[stateid];
        //状态开始
        CurrentState.OnStart();
    }
}
