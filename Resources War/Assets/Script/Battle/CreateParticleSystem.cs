using UnityEngine;

public class CreateParticleSystem : MonoBehaviour
{
    //目标组件
    private ChessManager target;
    //启动指令
    private bool StartFly = false;
    private HitTagetDelegate htd;
    private SkillType skillType;
    private int result;
    void Update()
    {
        //开关
        if (!StartFly)
            return;
        //距离够近
        if (Vector3.Distance(transform.position, target.transform.position) < 1f)
        {
            //停止飞行
            StartFly = false;
            //摧毁自己
            Destroy(gameObject);
            //击中效果
            ShotTarget();
        }
        //向目标飞行
        transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.05f);
    }
    /// <summary>
    /// 启动
    /// </summary>
    /// <param name="cm"></param>
    public void Setting(ChessManager cm, HitTagetDelegate htd, SkillType skillType, int result)
    {
        this.htd = htd;
        this.skillType = skillType;
        this.result = result;
        target = cm;
        StartFly = true;
    }
    /// <summary>
    /// 击中效果
    /// </summary>
    public void ShotTarget()
    {
        htd(skillType, target, result);
    }
}
