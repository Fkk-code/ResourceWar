using System.Collections;
using System.Collections.Generic;
using UIFrame;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    //棋子摄像机
    private MainCamearManager mainCamearManager;
    //棋子数组
    public List<ChessManager> chessManagers;
    //当前操作棋子下标
    public int currentChess;
    //当前释放的技能
    public SkillNumber currentSkillNumber;
    //提示灯的预制体
    public GameObject logLightPrefabs;
    //状态枚举
    public enum ShowChessState { 普通, 移动结束, 选择行动, 选择技能, 敌方回合 }
    //当前战斗状态
    public ShowChessState showChessState = ShowChessState.普通;
    //当前棋子状态
    public bool IsMove = false;
    public bool IsAction = false;
    void Awake()
    {
        instance = this;
        //初始化
        chessManagers = new List<ChessManager>();
        //获取摄像机
        mainCamearManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamearManager>();
    }
    void OnEnable()
    {
        //获取棋子
        GameObject[] go = GameObject.FindGameObjectsWithTag("Chess");
        //获取灯
        logLightPrefabs = Resources.Load<GameObject>("LogLight");
        //清空
        chessManagers.Clear();
        //获取组件
        for (int i = 0; i < go.Length; i++)
            chessManagers.Add(go[i].GetComponent<ChessManager>());
        //创建UI
        Facace.instance.LoadBattleScene();
        //测试
        currentChess = -1;
        //开始战斗
        NextRound();
    }
    void Update()
    {
        if (currentChess < 0)
            return;
        //移动
        if (Input.GetMouseButtonDown(0) && showChessState == ShowChessState.普通)
            ChessMoveToPoint();
        //攻击
        if (Input.GetMouseButtonDown(0) && showChessState == ShowChessState.选择行动)
            SetChessAttackObject();
        //技能选择目标
        if (Input.GetMouseButtonDown(0) && showChessState == ShowChessState.选择技能)
            SetChessSkillObject();
    }
    void OnGUI()
    {
        if (currentChess < 0)
            return;
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        GUILayout.Label("当前棋子编号：" + currentChess, style);
        GUILayout.Label("生命值:" + chessManagers[currentChess]._chessState.Hp + " / " + chessManagers[currentChess]._chessState.Hp_Max, style);
        GUILayout.Label("魔法值:" + chessManagers[currentChess]._chessState.Mp, style);
        GUILayout.Label("攻击力:" + chessManagers[currentChess]._chessState.Atk, style);
        GUILayout.Label("防御力:" + chessManagers[currentChess]._chessState.Def, style);
        GUILayout.Label("魔法攻击力:" + chessManagers[currentChess]._chessState.MAtk, style);
        GUILayout.Label("魔法防御力:" + chessManagers[currentChess]._chessState.MDef, style);
        GUILayout.Label("速度:" + chessManagers[currentChess]._chessState.Speed, style);
        if (chessManagers[currentChess]._chessState.buffs.Keys.Count > 0)
        {
            string s = "";
            foreach (SkillNumber skill in chessManagers[currentChess]._chessState.buffs.Keys)
            {
                s += "|"+skill.ToString();
            }
            GUILayout.Label("状态:" + s, style);
        }


        switch (showChessState)
        {
            case ShowChessState.普通:
                break;
            case ShowChessState.选择行动:
                break;
            case ShowChessState.选择技能:
                SelectSkill();
                break;
        }
    }
    #region 移动回溯
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector2Int startPoint;
    #endregion
    /// <summary>
    /// 挑选技能状态
    /// </summary>
    private void SelectSkill()
    {
        GUIStyle style = new GUIStyle();
        style.name = "button";
        style.fontSize = 50;
        for (int i = 0; i < chessManagers[currentChess]._chessState.skills.Count; i++)
        {
            //制作按钮
            if (GUILayout.Button(chessManagers[currentChess]._chessState.skills[i].ToString(), style))
            {
                //测试
                Debug.Log("释放技能 " + chessManagers[currentChess]._chessState.skills[i].ToString());
                //更新当前释放技能
                currentSkillNumber = chessManagers[currentChess]._chessState.skills[i];
                //判断技能类型
                switch (SkillClass.GetInstance().skillInfo[chessManagers[currentChess]._chessState.skills[i]].target)
                {
                    case SkillTarget.Self:
                        //直接释放
                        ReleaserSkill();
                        break;
                    case SkillTarget.Enemy:
                        //显示技能范围
                        ShowSkillArea(SkillClass.GetInstance().skillInfo[chessManagers[currentChess]._chessState.skills[i]].range);
                        break;
                }
            }
        }
    }
    /// <summary>
    /// 获取棋子攻击目标
    /// </summary>
    private void SetChessAttackObject()
    {
        //去除点灯光
        RemoveLight();
        //选择一个提示灯进行攻击
        GameObject go = MouseRayObject();
        //检测鼠标在什么物体上
        if (go != null && go.CompareTag("LogLight"))
        {
            if (BoardManager.instance.GetChessByPoint((int)go.transform.position.x, (int)go.transform.position.z) != null)
            {
                //攻击成功
                IsAction = true;
                UIManager.GetInstance().FindWidget("BattlePanel#", "MoveTxt#").Text.text = "移动";
                //设置目标
                chessManagers[currentChess]._chessAttack.ChessAttackTaget(BoardManager.instance.GetChessByPoint((int)go.transform.position.x, (int)go.transform.position.z));
                //棋子转向
                chessManagers[currentChess]._chessMove.PlayerRoll(go.transform.position);
                //启动动画
                chessManagers[currentChess]._chessAttack.PlayAnimation(chessManagers[currentChess]._chessAttack.NORMALATK);
            }
            //改变状态
            showChessState = ShowChessState.普通;
        }
    }
    /// <summary>
    /// 获取棋子技能目标
    /// </summary>
    private void SetChessSkillObject()
    {
        //去除点灯光
        RemoveLight();
        //选择一个提示灯进行攻击
        GameObject go = MouseRayObject();
        //检测鼠标在什么物体上
        if (go != null && go.CompareTag("LogLight"))
        {
            //判断蓝量是否充足
            if (chessManagers[currentChess]._chessState.Mp < SkillClass.GetInstance().skillInfo[currentSkillNumber].needMp)
                return;
            //初始化
            Queue<Vector2Int> path = new Queue<Vector2Int>();
            List<ChessManager> skillTarget = new List<ChessManager>();
            //查找技能范围
            int range = SkillClass.GetInstance().skillInfo[currentSkillNumber].effectrange;
            //查看作用敌人列表
            BoardManager.instance.BFS_Skill(
                new Vector2Int((int)go.transform.position.x, (int)go.transform.position.z)//攻击的坐标点
                , range//技能作用范围
                , path//返回的作用点
                );
            Debug.Log("技能效果大小" + range);
            Debug.Log("路径大小" + path.Count);
            while (path.Count > 0)
            {
                Vector2Int point = path.Dequeue();
                if (BoardManager.instance.GetChessByPoint(point.x, point.y) != null
                    && BoardManager.instance.GetChessByPoint(point.x, point.y)._chessState.Hp > 0)
                    skillTarget.Add(BoardManager.instance.GetChessByPoint(point.x, point.y));
            }
            Debug.Log("敌人数量" + skillTarget.Count);
            //没有目标则返回
            if (skillTarget.Count == 0)
                return;
            //攻击成功
            IsAction = true;
            //转向
            chessManagers[currentChess]._chessMove.PlayerRoll(go.transform.position);
            //设置目标
            SkillReleaser.GetInstance().SetEnemyListAndSelf(
                currentSkillNumber,//技能编号
                chessManagers[currentChess], //施法者
                skillTarget//作用者
                );
            //启动动画
            chessManagers[currentChess]._chessAttack.PlayAnimation(chessManagers[currentChess]._chessAttack.SKILLRELEASE);
            //改变状态
            showChessState = ShowChessState.普通;
        }
    }
    /// <summary>
    /// 对自己释放技能
    /// </summary>
    private void ReleaserSkill()
    {
        //判断蓝量是否充足
        if (chessManagers[currentChess]._chessState.Mp < SkillClass.GetInstance().skillInfo[currentSkillNumber].needMp)
            return;
        //初始化
        Queue<Vector2Int> path = new Queue<Vector2Int>();
        List<ChessManager> skillTarget = new List<ChessManager>();
        //查找技能范围
        int range = SkillClass.GetInstance().skillInfo[currentSkillNumber].effectrange;
        //查看作用敌人列表
        BoardManager.instance.BFS_Skill(
            chessManagers[currentChess]._chessMove.startPos//攻击的坐标点
            , range//技能作用范围
            , path//返回的作用点
            );
        //查看作用列表
        while (path.Count > 0)
        {
            Vector2Int point = path.Dequeue();
            if (BoardManager.instance.GetChessByPoint(point.x, point.y) != null
                && BoardManager.instance.GetChessByPoint(point.x, point.y)._chessState.Hp > 0)
                skillTarget.Add(BoardManager.instance.GetChessByPoint(point.x, point.y));
        }
        //攻击成功
        IsAction = true;
        UIManager.GetInstance().FindWidget("BattlePanel#", "MoveTxt#").Text.text = "移动";
        //设置目标
        SkillReleaser.GetInstance().SetEnemyListAndSelf(
            currentSkillNumber,//技能编号
            chessManagers[currentChess], //施法者
            skillTarget//作用者
            );
        //启动动画
        chessManagers[currentChess]._chessAttack.PlayAnimation(chessManagers[currentChess]._chessAttack.SKILLRELEASE);
        //改变状态
        showChessState = ShowChessState.普通;
    }
    /// <summary>
    /// 棋子移动
    /// </summary>
    private void ChessMoveToPoint()
    {
        //去除点灯光
        RemoveLight();
        //选择一个提示灯进行移动
        GameObject go = MouseRayObject();
        //检测鼠标在什么物体上
        if (go != null && go.CompareTag("LogLight"))
        {
            //记录起始位置
            lastPosition = chessManagers[currentChess].transform.position;
            lastRotation = chessManagers[currentChess].transform.rotation;
            startPoint = chessManagers[currentChess]._chessMove.startPos;
            //移动
            chessManagers[currentChess]._chessMove.MoveToArrived(new Vector2Int((int)go.transform.position.x, (int)go.transform.position.z));
            //移动成功
            IsMove = true;
            //更改UI名称
            if (!IsAction)
                UIManager.GetInstance().FindWidget("BattlePanel#", "MoveTxt#").Text.text = "返回";
        }
    }
    /// <summary>
    /// 棋子移动回溯
    /// </summary>
    public void ReturnChessMove()
    {
        //回溯棋子坐标
        chessManagers[currentChess].transform.position = lastPosition;
        //回溯方向
        chessManagers[currentChess].transform.rotation = lastRotation;
        //设置棋盘字典
        BoardManager.instance.UpdateBoardDic(chessManagers[currentChess], chessManagers[currentChess]._chessMove.startPos, startPoint);
        //回溯坐标
        chessManagers[currentChess]._chessMove.startPos = startPoint;
        //移动回溯成功
        IsMove = false;
        UIManager.GetInstance().FindWidget("BattlePanel#", "MoveTxt#").Text.text = "移动";
    }
    /// <summary>
    /// 显示移动范围
    /// </summary>
    public void ShowMoveArea()
    {
        //获取可移动范围
        int[,] moveArea = BoardManager.instance.BFS_Move(
            chessManagers[currentChess]._chessMove.startPos,//起点
            chessManagers[currentChess]._chessState.moveMaxDictance,//最大移动距离
            chessManagers[currentChess]._chessState.team//玩家队伍
            );
        //防止重复生成
        if (transform.childCount > 0)
            return;
        //有棋子的地方不能走
        for (int i = 0; i < chessManagers.Count; i++)
            moveArea[chessManagers[i]._chessMove.startPos.x, chessManagers[i]._chessMove.startPos.y] = 0;
        //刷提示灯
        for (int i = 0; i < moveArea.GetLength(0); i++)
        {
            for (int j = 0; j < moveArea.GetLength(1); j++)
            {
                if (moveArea[i, j] == 1)
                {
                    GameObject go = Instantiate(logLightPrefabs, transform);
                    go.transform.position = new Vector3(i, 1.26f, j);
                    go.GetComponent<SimpleLight>().ChangeState("移动");
                }
            }
        }
    }
    /// <summary>
    /// 显示攻击范围
    /// </summary>
    public void ShowAttackArea()
    {
        //不在重复显示攻击范围
        if (transform.childCount > 0 && showChessState == ShowChessState.选择行动)
            return;
        RemoveLight();
        //获取可攻击范围
        int[,] moveArea = BoardManager.instance.BFS_Attack(
            chessManagers[currentChess]._chessMove.startPos,//棋子起点
            chessManagers[currentChess]._chessState.attackMaxDictance//最大攻击范围
            );
        //从技能状态中切换回来
        if (showChessState == ShowChessState.选择技能)
            showChessState = ShowChessState.选择行动;
        //刷提示灯
        for (int i = 0; i < moveArea.GetLength(0); i++)
        {
            for (int j = 0; j < moveArea.GetLength(1); j++)
            {
                if (moveArea[i, j] == 1)
                {
                    GameObject go = Instantiate(logLightPrefabs, transform);
                    go.transform.position = new Vector3(i, 1.26f, j);
                    go.GetComponent<SimpleLight>().ChangeState("攻击");
                }
            }
        }
    }
    /// <summary>
    /// 显示技能范围
    /// </summary>
    private void ShowSkillArea(int distance)
    {
        if (transform.childCount > 0 && showChessState == ShowChessState.选择技能)
            return;
        RemoveLight();
        //获取可攻击范围
        int[,] moveArea = BoardManager.instance.BFS_Attack(
            chessManagers[currentChess]._chessMove.startPos,//棋子起点
           distance//技能释放范围
            );

        //刷提示灯
        for (int i = 0; i < moveArea.GetLength(0); i++)
        {
            for (int j = 0; j < moveArea.GetLength(1); j++)
            {
                if (moveArea[i, j] == 1)
                {
                    GameObject go = Instantiate(logLightPrefabs, transform);
                    go.transform.position = new Vector3(i, 1.26f, j);
                    go.GetComponent<SimpleLight>().ChangeState("技能");
                }
            }
        }
    }
    /// <summary>
    /// 移除提示灯
    /// </summary>
    private void RemoveLight()
    {
        //去除点灯光
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 鼠标射线检测
    /// </summary>
    /// <returns></returns>
    private GameObject MouseRayObject()
    {
        //射线返回值
        RaycastHit raycastHit;
        //屏幕射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            return raycastHit.collider.gameObject;
        }
        return null;
    }
    /// <summary>
    /// 切换棋子
    /// </summary>
    public void NextRound()
    {
        //切换棋子
        currentChess = (currentChess + 1) % chessManagers.Count;
        //按速度排序
        if (currentChess == 0)
            chessManagers.Sort();
        //棋子死亡直接跳过
        while (chessManagers[currentChess]._chessState.Hp <= 0)
        {
            currentChess = (currentChess + 1) % chessManagers.Count;
        }
        if (chessManagers[currentChess]._chessState.team == 1)
        {
            //判断敌人是否死亡
            if (EnemyIsDie())
            {
                BattleEnd(1);
            }
            showChessState = ShowChessState.普通;
        }
        else
        {
            showChessState = ShowChessState.敌方回合;
            //启动AI
            chessManagers[currentChess].GetComponent<AIControl>().StartAI();
        }
        //摄像头切换目标
        mainCamearManager.Target = chessManagers[currentChess].transform;
        //初始化
        IsMove = false;
        IsAction = false;
    }
    /// <summary>
    /// 判断敌人是否全部死亡
    /// </summary>
    /// <returns></returns>
    private bool EnemyIsDie()
    {
        for (int i = 0; i < GameConst.GetInstance().EnemyPrefab.Count; i++)
        {
            if (GameConst.GetInstance().EnemyPrefab[i].GetComponent<ChessState>().Hp > 0)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 战斗结束
    /// </summary>
    public void BattleEnd(int team)
    {
        StartCoroutine(WaitTime(team));
    }
    /// <summary>
    /// 延迟弹出奖励提示窗
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    IEnumerator WaitTime(int team)
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.GetInstance().OpenModule("EndBattlePanel");
        UIManager.GetInstance().FindWidget("EndBattlePanel", "Title_Result#")
            .Text.text = team == 100 ? "Default" : "Victory";
        if (team != 100)
        {
            //增加金币
            GameConst.GetInstance().Money += 100 * GameConst.GetInstance().EnemyPrefab.Count;
            //
            Debug.Log(GameConst.GetInstance().Money);
        }
    }
}
