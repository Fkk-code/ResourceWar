using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
using Utilty;

public class TrophyByPool : ObjectByPool
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

        string spriteName = parameter[1] as string;

        _uiWidget._currentModule = parameter[1] as UIModuleBase;

        //设置父对象
        transform.SetParent(parent);
        //设置锚点计算后的本地坐标
        _uiWidget.RectTransform.anchoredPosition = Vector2.zero;
        //设置本地缩放
        transform.localScale = Vector3.one;

        //根据对应编号的名字获取元件路径
        string equipPath = UIConfigurationManager.GetInstance().GetWidgetAssetPathByName(spriteName);
        //加载精灵图片
        Sprite equipSprite = AssetsManager.GetInstance().GetAssets<Sprite>(equipPath);
        //设置图片
        _uiWidget.Image.sprite = equipSprite;
    }

    public override void ObjectDispose(object disposeParameter)
    {
        throw new System.NotImplementedException();
    }
}
