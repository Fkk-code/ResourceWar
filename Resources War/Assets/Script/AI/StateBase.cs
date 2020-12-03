using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase : MonoBehaviour
{
    //状态编号
    public int id;
    //当前状态的状态机
    public StateMachine Machine;
    //构造函数
    public StateBase(int id)
    {
        this.id = id;
    }
    /// <summary>
    /// 状态开始
    /// </summary>
    public virtual void OnStart(){}
    /// <summary>
    /// 状态中
    /// </summary>
    public virtual void OnUpdate(){}
    /// <summary>
    /// 状态退出
    /// </summary>
    public virtual void OnExit(){}


}
