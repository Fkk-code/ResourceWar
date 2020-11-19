using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 资源加载模块
/// 1 异步加载
/// 2 委托 lambda表达式
/// 3 携程
/// 4 泛型
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    //同步加载资源
  public T Load<T>(string name)where T:Object
    {
        T res = Resources.Load<T>(name);
        //如果对象是一个gameobject的类型 实例化之后 再返回出去 外部可以直接使用
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else//TextAsset AudioClip
            return res;
    }

    public void LoadAsync<T>(string name,UnityAction<T> callBack)where T : Object
    {
        //开启加载异步加载的携程
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsyns(name,callBack));
        
    }

    private  IEnumerator ReallyLoadAsyns<T>(string name,UnityAction<T> callback)where T:Object
    {
       ResourceRequest r= Resources.LoadAsync<T>(name);
        yield return r;

        if (r.asset is GameObject)
            callback(GameObject.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
        
    }
}
