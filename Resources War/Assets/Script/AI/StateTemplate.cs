using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTemplate<T> : StateBase
{
    //状态拥有者
    public T owner;
    //构造函数
    public StateTemplate(int id,T owner) : base(id)
    {
        //获得类型
        this.owner = owner;
    }
}
