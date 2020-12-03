using System.Collections.Generic;
using UnityEngine;
using Utilty;
/// <summary>
/// 英雄预制体枚举
/// </summary>
public enum HeroEnum
{
    //金刚狼
    BarbarianBlue,
    BarbarianBrown,
    BarbarianPurple,
    //射手
    ArcherBlue,
    ArcherGreen,
    ArcherRed,
    //骑士
    KnightBlue,
    KnightBrown,
    KnightPurple,
    //巫师
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
/// <summary>
/// 技能编号枚举
/// </summary>
public enum SkillNumber
{
    //法师
    治疗术,
    凝神,
    燃烧,
    火球术,
    冥想,
    //骑士
    战斗怒吼,//提升攻击力
    骑士祝福,//提升最大生命值
    防御姿态,//提升防御力
    冲锋姿态,//提升移动速度
    //猎人
    鹰眼术,//提升射程
    风暴,//群体魔法
    弱点击破,//降低防御
    禁足,//降低速度
    //金刚狼
    狂暴//提升攻击

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
