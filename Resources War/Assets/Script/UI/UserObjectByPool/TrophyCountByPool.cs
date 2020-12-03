using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrame;
using Utilty;

public class TrophyCountByPool : ObjectByPool
{
    private UIWidgetsBase _uiWidget;

    private void Awake()
    {
        _uiWidget = gameObject.AddComponent<UIWidgetsBase>();
    }


    public override void ObjectInit(object initParameter)
    {
        object[] parameter = initParameter as object[];

        Transform parent = parameter[0] as Transform;

        _uiWidget._currentModule = parameter[1] as UIModuleBase;

        //设置父对象
        transform.SetParent(parent);
        //设置锚点计算后的本地坐标
        _uiWidget.RectTransform.anchoredPosition = new Vector2(37,-37);
        //设置本地缩放
        transform.localScale = Vector3.one;

        int randomIndex = Random.Range(0, 5);

        //设置文本显示内容
        gameObject.GetComponent<Text>().text = randomIndex.ToString();
    }

    public override void ObjectDispose(object disposeParameter)
    {
        throw new System.NotImplementedException();
    }
}
