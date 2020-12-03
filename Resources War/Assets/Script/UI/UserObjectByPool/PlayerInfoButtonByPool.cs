using UnityEngine;
using Utilty;
using UIFrame;

public class PlayerInfoButtonByPool : ObjectByPool
{
    private UIWidgetsBase _uiWidget;

    private void Awake()
    {
        //添加元件
        _uiWidget = gameObject.AddComponent<UIWidgetsBase>();
    }
    /// <summary>
    /// 物体生成
    /// </summary>
    /// <param name="initParamater"></param>
    public override void ObjectInit(object initParamater)
    {
        //获取玩家列表数组
        object[] parameter = initParamater as object[];
        //获取父物体坐标
        Transform parent = parameter[0] as Transform;
        _uiWidget._currentModule = parameter[1] as UIModuleBase;
        //设置父对象
        transform.SetParent(parent);
    }
    /// <summary>
    /// 物体摧毁
    /// </summary>
    /// <param name="initParamater"></param>
    public override void ObjectDispose(object initParamater)
    {
        throw new System.NotImplementedException();
    }
}
