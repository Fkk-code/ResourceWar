using System.Collections.Generic;
using Utilty;

/// <summary>
/// 技能编号枚举
/// </summary>
public enum SkillNumber
{
    治疗术,
    凝神,
    燃烧,
    火球术
}

/// <summary>
/// 技能效果枚举
/// </summary>
public enum SkillEffect
{
    Increase,//增加
    Reduce,  //减少
    Special  //特殊
}

/// <summary>
/// 技能对象枚举
/// </summary>
public enum SkillTarget
{
    Self,   //自身
    Enemy,  //敌人
    Create  //创造
}

/// <summary>
/// 技能类型枚举
/// </summary>
public enum SkillType
{
    LifeValue,      //生命值
    MagicalValue,  //魔法值
    PhysicaLAttack, //物理攻击
    PhysicalDefense,//物理防御
    MagicAttack,    //魔攻
    MagicDefense,   //魔防
    DodgeRate,      //闪避率
    CritRate        //暴击率
}

/// <summary>
/// 技能基类
/// </summary>
public class SkillBase
{
    //技能基类效果
    public SkillEffect skillEffect;
    //技能基类对象
    public SkillTarget skillTarget;
    //技能基类类型
    public SkillType skillType;
    //技能基类值
    public int skillValue;
    //技能粒子特效
    public string PSName;
    //构造方法
    public SkillBase(SkillEffect skillEffect, SkillTarget skillTarget, SkillType skillType, int skillValue)
    {
        this.skillEffect = skillEffect;
        this.skillTarget = skillTarget;
        this.skillType = skillType;
        this.skillValue = skillValue;
    }
    public SkillBase(SkillEffect skillEffect, SkillTarget skillTarget, SkillType skillType, int skillValue, string PSName)
    {
        this.skillEffect = skillEffect;
        this.skillTarget = skillTarget;
        this.skillType = skillType;
        this.skillValue = skillValue;
        this.PSName = PSName;
    }
}

/// <summary>
/// 技能类
/// </summary>
public class Skill
{
    //技能编号
    public SkillNumber number;
    //技能名称
    public string name;
    //技能消耗
    public int needMp;
    //技能回合数
    public int Round;
    //技能对象
    public SkillTarget target;
    //技能范围
    public int range;
    //技能效果范围
    public int effectrange;
    //技能基类
    public SkillBase[] skillBases;
    /// <summary>
    /// 构造技能
    /// </summary>
    /// <param name="number">技能编号</param>
    /// <param name="needMp">需要的蓝量</param>
    /// <param name="target">目标类型</param>
    /// <param name="range">技能范围</param>
    /// <param name="effectrange">技能效果范围</param>
    /// <param name="skillBases">技能效果基类</param>
    public Skill(SkillNumber number, int needMp, SkillTarget target, int range, int effectrange, SkillBase[] skillBases)
    {
        this.number = number;
        name = number.ToString();
        this.needMp = needMp;
        Round = 3;
        this.target = target;
        this.range = range;
        this.effectrange = effectrange;
        this.skillBases = skillBases;
    }
}

/// <summary>
/// 技能类 类信息
/// </summary>
public class SkillClass : Singleton<SkillClass>
{
    //技能信息字典
    public Dictionary<SkillNumber, Skill> skillInfo;
    //构造
    private SkillClass()
    {
        //实例化字典
        skillInfo = new Dictionary<SkillNumber, Skill>();
        #region 治疗术
        SkillBase[] skillBases = new SkillBase[2]
        {
            new SkillBase(SkillEffect.Reduce,SkillTarget.Self,SkillType.MagicalValue,10),
            new SkillBase(SkillEffect.Increase,SkillTarget.Enemy,SkillType.LifeValue,20,"治疗术")
        };
        Skill newSkill = new Skill(SkillNumber.治疗术 , 10, SkillTarget.Enemy, 5, 1, skillBases);
        skillInfo.Add(SkillNumber.治疗术, newSkill);
        #endregion
        #region 凝神
        skillBases = new SkillBase[1]
        {
            new SkillBase(SkillEffect.Increase,SkillTarget.Self,SkillType.MagicalValue,10,"凝神")
        };
        newSkill = new Skill(SkillNumber.凝神, 0, SkillTarget.Self, 0, 0, skillBases);
        skillInfo.Add(SkillNumber.凝神, newSkill);
        #endregion
        #region 燃烧
        skillBases = new SkillBase[2]
        {
            new SkillBase(SkillEffect.Reduce,SkillTarget.Self,SkillType.MagicalValue,10),
            new SkillBase(SkillEffect.Reduce,SkillTarget.Enemy,SkillType.LifeValue,20,"燃烧")
        };
        newSkill = new Skill(SkillNumber.燃烧, 10, SkillTarget.Enemy, 5, 1, skillBases);
        skillInfo.Add(SkillNumber.燃烧, newSkill);
        #endregion
        #region 火球术
        skillBases = new SkillBase[2]
        {
            new SkillBase(SkillEffect.Reduce,SkillTarget.Self,SkillType.MagicalValue,5),
            new SkillBase(SkillEffect.Reduce,SkillTarget.Create,SkillType.LifeValue,10,"火球术")
        };
        newSkill = new Skill(SkillNumber.火球术, 5, SkillTarget.Enemy, 7, 0, skillBases);
        skillInfo.Add(SkillNumber.火球术, newSkill);
        #endregion
    }
}
