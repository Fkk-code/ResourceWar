using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilty;
using UIFrame;
using UnityEngine.EventSystems;

public class Btn01ByPool : ObjectByPool
{
    private UIWidgetsBase _uiWidget;

    private void Awake()
    {
        _uiWidget = gameObject.AddComponent<UIWidgetsBase>();
    }

    private void Start()
    {
        OnClick();
    }

    /// <summary>
    /// 点击按钮
    /// </summary>
    private void OnClick()
    {
        gameObject.GetComponent<UIWidgetsBase>().Button.onClick.AddListener(() =>
        {
            Facace.instance.LoadScene("MainScene");
        });
    }
    
    public override void ObjectInit(object initParameter)
    {
        object[] parameter = initParameter as object[];

        Transform parent = parameter[0] as Transform;

        _uiWidget._currentModule = parameter[1] as UIModuleBase;

        //设置父对象
        transform.SetParent(parent);
        //设置锚点计算后的本地坐标
        _uiWidget.RectTransform.anchoredPosition = Vector2.zero;
        //设置本地缩放
        transform.localScale = Vector3.one;
    }

    public override void ObjectDispose(object disposeParameter)
    {
        throw new System.NotImplementedException();
    }
}
