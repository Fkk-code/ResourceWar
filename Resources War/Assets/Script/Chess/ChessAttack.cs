using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAttack : MonoBehaviour
{
    //棋子管理器
    private ChessManager _chessManager;
    [Header("走路")]
    public string WALK = "Walk";
    [Header("跳跃")]
    public string JUMP = "Jump";
    [Header("普通攻击")]
    public string NORMALATK = "Left Claw Attack";
    [Header("暴击")]
    public string POWERATK = "Jump Attack";
    [Header("技能释放")]
    public string SKILLRELEASE = "Cast Spell 01";
    [Header("受伤")]
    public string TAKEDAMAGE = "Take Damage";
    [Header("死亡")]
    public string DIE = "Die";
    void Awake()
    {
        _chessManager = GetComponent<ChessManager>();
    }
    void Start()
    {
        if (!_chessManager)
            _chessManager = GetComponent<ChessManager>();
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="s"></param>
    public void PlayAnimation(string s)
    {
        _chessManager._animator.SetTrigger(s);
    }
    public void PlayAnimation(string s, bool bvalue)
    {
        _chessManager._animator.SetBool(s, bvalue);
    }
    public void PlayAnimation(string s, float fvalue)
    {
        _chessManager._animator.SetFloat(s, fvalue);
    }
    /// <summary>
    /// 生成粒子特效
    /// </summary>
    /// <param name="go"></param>
    public void CreateParticleSystem(GameObject go)
    {
        //实例化
        GameObject PS = Instantiate(go, transform);
    }
    /// <summary>
    /// 生成子弹
    /// </summary>
    /// <param name="go"></param>
    /// <param name="target"></param>
    public void CreateBullet(GameObject go,ChessManager target,HitTagetDelegate htd, SkillType skillType, int result)
    {
        //实例化
        GameObject bbb = Instantiate(go, transform.position+go.transform.position, Quaternion.identity);
        //添加组件
        CreateParticleSystem cps = bbb.GetComponent<CreateParticleSystem>()?? bbb.AddComponent<CreateParticleSystem>();
        //组件赋值
        cps.Setting(target, htd, skillType, result);
    }
    /// <summary>
    /// 攻击目标列表
    /// </summary>
    private List<ChessManager> AttackTaget;
    /// <summary>
    /// 设置攻击的目标
    /// </summary>
    /// <param name="cm"></param>
    public void ChessAttackTaget(List<ChessManager> cm)
    {
        //清空列表
        AttackTaget = new List<ChessManager>();
        //获取列表棋子
        AttackTaget = cm;
    }
    /// <summary>
    /// 单个目标（普通攻击）
    /// </summary>
    /// <param name="cm"></param>
    public void ChessAttackTaget(ChessManager cm)
    {
        //清空列表
        AttackTaget = new List<ChessManager>();
        //获取列表棋子
        AttackTaget.Add(cm);
    }
    /// <summary>
    /// 普通攻击(关键帧)
    /// </summary>
    public void TagetIsHurt()
    {
        for (int i = 0; i < AttackTaget.Count; i++)
        {
            //结果
            int result = _chessManager._chessState.Atk - AttackTaget[i]._chessState.Def;
            //UI显示结果
            string res = "-" + result.ToString();
            //显示UI
            DamageGUI(AttackTaget[i].transform, res, Color.red);
            //扣除血量
            AttackTaget[i]._chessState.Hp -= result;
            IsDie(AttackTaget[i]);
        }
    }
    public void IsDie(ChessManager cm)
    {
        //是否死亡
        if (cm._chessState.Hp <= 0)
        {
            //死亡动画
            cm._chessAttack.PlayAnimation(_chessManager._chessAttack.DIE);
        }
        else
        {
            //受伤动画
            cm._chessAttack.PlayAnimation(_chessManager._chessAttack.TAKEDAMAGE);
        }
    }
    /// <summary>
    /// 受伤UI显示
    /// </summary>
    /// <param name="taget"></param>
    /// <param name="result"></param>
    /// <param name="color"></param>
    public void DamageGUI(Transform taget,string result,Color color)
    {
        //实例
        GameObject go = Instantiate(Resources.Load<GameObject>("DamageGUI"),taget);
        //设置
        go.GetComponent<DamageGUI>().Value = result;
        go.GetComponent<DamageGUI>()._color = color;
    }
    /// <summary>
    /// 技能效果(关键帧)
    /// </summary>
    public void SkillEffect()
    {
        SkillReleaser.GetInstance().UseSkillBase();
    }
}
