using System.Collections.Generic;
using UnityEngine;
using Utilty;
//定义委托，括号里参数个数与需要委托执行的函数一致
public delegate void HitTagetDelegate(SkillType skillType, ChessManager cm, int result);
public class SkillReleaser : Singleton<SkillReleaser>
{
    //单例
    private SkillReleaser() { }
    //技能编号
    private SkillNumber skillNumber;
    //释放者
    private ChessManager selfcm;
    //受作用者
    private List<ChessManager> enemycms;
    //获取技能目标
    public void SetEnemyListAndSelf(SkillNumber skillNumber, ChessManager selfcm, List<ChessManager> enemycm)
    {
        //记录技能编号
        this.skillNumber = skillNumber;
        //记录释放者
        this.selfcm = selfcm;
        //设置目标列表
        enemycms = enemycm;
    }
    /// <summary>
    /// 获取技能目标(自身)
    /// </summary>
    /// <param name="skillNumber"></param>
    /// <param name="selfcm"></param>
    public void SetEnemyListAndSelf(SkillNumber skillNumber, ChessManager selfcm)
    {
        this.skillNumber = skillNumber;
        this.selfcm = selfcm;
    }
    /// <summary>
    /// 技能效果
    /// </summary>
    public void UseSkillBase()
    {
        //获取技能信息
        Skill skill = SkillClass.GetInstance().skillInfo[skillNumber];
        //遍历技能基类
        for (int i = 0; i < skill.skillBases.Length; i++)
        {
            //获取技能效果值
            int result = skill.skillBases[i].skillValue;
            //增加Or降低
            result = skill.skillBases[i].skillEffect == SkillEffect.Increase ? result : -result;
            //对自身
            switch (skill.skillBases[i].skillTarget)
            {
                case SkillTarget.Self:
                    //对自身
                    //判断是否产生粒子特效
                    if (skill.skillBases[i].PSName != null)
                        CreatePaticleSystem(selfcm, skill.skillBases[i].PSName);
                    //作用
                    BaseSkillType(skill.skillBases[i].skillType, selfcm, result);
                    break;
                case SkillTarget.Enemy:
                    //对敌人
                    for (int j = 0; j < enemycms.Count; j++)
                    {
                        //是否产生粒子特效
                        if (skill.skillBases[i].PSName != null)
                            CreatePaticleSystem(enemycms[j], skill.skillBases[i].PSName);
                        //计算魔法的伤害值
                        if (result < 0)
                        {
                            //魔法伤害可以被魔法防御抵抗
                            result = (selfcm._chessState.MAtk + result) - enemycms[j]._chessState.MDef;
                            result = result > 0 ? 0 : result;
                        }
                        //作用
                        BaseSkillType(skill.skillBases[i].skillType, enemycms[j], result);
                    }
                    break;
                case SkillTarget.Create:
                    //弹道技能
                    for (int j = 0; j < enemycms.Count; j++)
                    {
                        //制作委托
                        HitTagetDelegate hitTagetDelegate = new HitTagetDelegate(BaseSkillType);
                        //计算魔法的伤害值
                        if (result < 0)
                        {
                            //魔法伤害可以被魔法防御抵抗
                            result = (selfcm._chessState.MAtk + result) - enemycms[j]._chessState.MDef;
                            result = result > 0 ? 0 : result;
                        }
                        //创造子弹
                        CreateBullet(selfcm, skill.skillBases[i].PSName, enemycms[j],hitTagetDelegate, skill.skillBases[i].skillType,result);
                    }
                    break;
            }
        }
    }
    /// <summary>
    /// 处理效果
    /// </summary>
    /// <param name="skillType"></param>
    /// <param name="cm"></param>
    /// <param name="result"></param>
    private void BaseSkillType(SkillType skillType, ChessManager cm, int result)
    {
        //文本
        string res = result > 0 ? "+" + result : result.ToString();
        //颜色
        Color color = Color.red;
        //处理值
        switch (skillType)
        {
            case SkillType.LifeValue:
                color = Color.red;
                if (result < 0)
                {
                    cm._chessState.Hp += result;
                    cm._chessAttack.IsDie(cm);
                }
                else if(result > 0)
                {
                    result = cm._chessState.Hp + result > cm._chessState.Hp_Max ?
                        cm._chessState.Hp_Max - cm._chessState.Hp :
                        result;
                    cm._chessState.Hp += result;
                }
                break;
            case SkillType.MagicalValue:
                cm._chessState.Mp += result;
                color = Color.blue;
                break;
            case SkillType.PhysicaLAttack:
                cm._chessState.Atk += result;
                break;
            case SkillType.PhysicalDefense:
                cm._chessState.Def += result;
                break;
            case SkillType.MagicAttack:
                cm._chessState.MAtk += result;
                break;
            case SkillType.MagicDefense:
                cm._chessState.MDef += result;
                break;
            case SkillType.DodgeRate:
                break;
            case SkillType.CritRate:
                break;
        }
        //显示GUI
        cm._chessAttack.DamageGUI(cm.transform, res, color);
    }
    /// <summary>
    /// 产生粒子特效
    /// </summary>
    /// <param name="cm"></param>
    /// <param name="PSName"></param>
    private void CreatePaticleSystem(ChessManager cm,string PSName)
    {
        //加载资源
        GameObject go = Resources.Load<GameObject>("ParticleSystem/" + PSName);
        //生产粒子
        cm._chessAttack.CreateParticleSystem(go);
    }
    private void CreateBullet(ChessManager cm,string PSName,ChessManager target, HitTagetDelegate htd,SkillType skillType,int result)
    {
        //加载资源
        GameObject go = Resources.Load<GameObject>("ParticleSystem/" + PSName);
        //生产粒子
        cm._chessAttack.CreateBullet(go,target,htd, skillType, result);
    }
}
