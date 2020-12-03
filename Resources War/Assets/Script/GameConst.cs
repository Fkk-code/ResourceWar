using System.Collections.Generic;
using UnityEngine;
using Utilty;
/// <summary>
/// 英雄预制体枚举
/// </summary>
public enum HeroEnum
{
    WizardBlue,
    WizardPurple,
    WizardRed
}
/// <summary>
/// 怪物预制体枚举
/// </summary>
public enum EnemyEnum
{
    //蝙蝠
    BatGreen,
    BatPink,
    BatRed,
    //仙人掌
    CactusMagenta,
    CactusOrange,
    CactusPurple
}
/// <summary>
/// 收集品预制体枚举
/// </summary>
public enum CollectionEnum
{
    i一叠钞票,
    i寒冰珠,
    i火焰珠,
    i银,
    i锂矿,
    i锂矿石,
    i黄金,
    i朱雀矿,
    i青铜,
    i绿水灵,
    i蓝色果实,
    i刺客之石,
    i猎人之石,
    i骑士之石,
    i魔法之石
}
public  class GameConst: Singleton<GameConst>
{
    public static int OPEN_ANI_PARAMETER;
    private GameConst()
    {
        OPEN_ANI_PARAMETER = Animator.StringToHash("Open");
        PlayerchessPerfab = new List<GameObject>();
        EnemyPrefab = new List<GameObject>();
    }
    //Lua路径
    public string path = Application.dataPath + "\\Script/Hero";
    //方格预制体
    public GameObject PrefabCube;
    //敌人预制体
    public List<GameObject> EnemyPrefab;
    //玩家预制体
    public List<GameObject> PlayerchessPerfab;
    //战斗地图信息
    public InteractiveProp activeInfo;
    //账号编号
    public int Id;
    //账号金币
    public int Money = 0;
    //账号钻石
    public int Jewel = 0;
}
