using System;
using System.Collections.Generic;
using UnityEngine;

public class ChessState : MonoBehaviour
{
    //棋子管理器
    private ChessManager _chessManager;
    //棋子名称
    EnemyEnum enemyEnum = EnemyEnum.BatGreen;
    //棋子等级
    public int Level = 1;
    //最大移动距离
    [Range(3, 7)]
    public int moveMaxDictance =3;
    //最大攻击距离
    [Range(1,5)]
    public int attackMaxDictance=1;
    //移动方式 走路 闪现
    //基础数值
    [NonSerialized]
    public int Hp;
    [NonSerialized]
    public int Mp;
    [Range(1, 100)]
    public int Hp_Max = 20;
    [Range(1, 100)]
    public int Mp_Max = 10;
    [Range(1, 100)]
    public int Atk = 5;
    [Range(1, 100)]
    public int Def = 3;
    [Range(1, 100)]
    public int MAtk = 8;
    [Range(1, 100)]
    public int MDef = 3;
    [Range(1, 100)]
    public int Speed = 1;
    //技能数组
    public List<SkillNumber> skills;
    //buff数组
    public Dictionary<SkillNumber, int> buffs;
    //队伍编号
    [NonSerialized]
    public int team;
    //①生命值②最大生命值③魔法值④最大魔法值⑤攻击力⑥防御力⑦魔法攻击力⑧魔法防御力⑨速度 
    [NonSerialized]
    public int[] baseState = new int[9];
    void Awake()
    {
        //获取组件
        _chessManager = GetComponent<ChessManager>();
        //初始化
        buffs = new Dictionary<SkillNumber, int>();
    }
    /// <summary>
    /// 战前设置
    /// </summary>
    /// <param name="t">队伍编号</param>
    public void InitBeforeBattle(int t)
    {
        //查询数据
        //设置属性
        team = t;
        buffs = new Dictionary<SkillNumber, int>();
        Hp = Hp_Max;
        Mp = Mp_Max;
    }

}
