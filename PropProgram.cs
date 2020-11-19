using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEditor;
using UnityEngine;
class PropProgram 
{
    private static PropProgram pp;
    public static PropProgram GetPropProgram()
    {
        if (pp == null)
            pp = new PropProgram();
        return pp;
    }
    private PropProgram()
    {
        D_Prop.Add(PropEnum.小剑, new Prop(PropEnum.小剑, EquipTypeEnum.武器, 5, 1, "简简单单一把剑"));
        D_Prop.Add(PropEnum.铁剑, new Prop(PropEnum.铁剑, EquipTypeEnum.武器, 10, 2, "用铁打造的一把剑"));

        D_Prop.Add(PropEnum.包子头, new Prop(PropEnum.包子头, EquipTypeEnum.头部, 5, 1, "把包子顶在脑子上"));
        D_Prop.Add(PropEnum.士兵护头, new Prop(PropEnum.士兵护头, EquipTypeEnum.头部, 10, 2, "小兵标配的头盔"));

        D_Prop.Add(PropEnum.初行装, new Prop(PropEnum.初行装, EquipTypeEnum.套装, 5, 1, "新手装没其他衣服就先穿上吧"));
        D_Prop.Add(PropEnum.武士服, new Prop(PropEnum.武士服, EquipTypeEnum.套装, 10, 2, "穿上就说明你是一位武士了"));

        D_Prop.Add(PropEnum.馒头, new Prop(PropEnum.馒头, PropTypeEnum.消耗品, 5, 5, 1, "恢复50生命值"));
        D_Prop.Add(PropEnum.豆奶, new Prop(PropEnum.豆奶, PropTypeEnum.消耗品, 5, 10, 2, "提高10点攻击力"));
    }
    public static Dictionary<PropEnum, Prop> D_Prop = new Dictionary<PropEnum, Prop>();
}
class Prop
{
    public PropEnum Pe;//道具编号
    public PropTypeEnum Type;//道具种类
    public EquipTypeEnum Equiptype;//装备类型
    private int maxNum;//最大叠加数
    private int buyMoney;//买入价格
    private int sellMoney;//卖出价格
    private string introduce;//道具介绍

    //装备
    public Prop(PropEnum pe, EquipTypeEnum equiptype,
        int buyMoney,int sellMoney,string introduce)
    {
        Pe = pe;
        Type = PropTypeEnum.装备;
        Equiptype = equiptype;
        BuyMoney = buyMoney;
        SellMoney = sellMoney;
        Introduce = introduce;
    }
    //道具
    public Prop(PropEnum pe, PropTypeEnum type,int maxnum,
        int buyMoney, int sellMoney, string introduce)
    {
        Pe = pe;
        Type = type;
        BuyMoney = buyMoney;
        MaxNum = maxnum;
        SellMoney = sellMoney;
        Introduce = introduce;
    }

    public int MaxNum { get => maxNum; set => maxNum = value; }
    public int BuyMoney { get => buyMoney; set => buyMoney = value; }
    public int SellMoney { get => sellMoney; set => sellMoney = value; }
    public string Introduce { get => introduce; set => introduce = value; }
}
