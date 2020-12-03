
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //单例
    public static BoardManager instance;
    //棋盘父类
    private GameObject BoardParent;
    //棋盘字典
    private Dictionary<Vector2Int, BoardTerrinItem> boardDic;
    //摄像机
    private Camera _camera;
    //战斗管理器
    public BattleManager _battleManager;
    #region 回调
    void Awake()
    {
        //单例
        instance = this;
        //创建棋盘父物体
        BoardParent = GameObject.FindGameObjectWithTag("Board") ?? new GameObject("BoardParent");
        //设置父物体标签
        BoardParent.tag = "Board";
        //初始化棋盘字典
        boardDic = new Dictionary<Vector2Int, BoardTerrinItem>();
    }
    void Start()
    {
        if (BoardParent.transform.childCount == 0)
        {
            //创建棋盘
            NewCreateBoardTerrain();
            //创建敌方棋子
            CreateEnemyChessItem();
            //创建我方棋子
            CreatePlayerChessItem();
            //添加战斗脚本
            _battleManager = GetComponent<BattleManager>() ?? gameObject.AddComponent<BattleManager>();
        }
        else
        {
            //创建棋盘字典
            CreateBoardDic();
        }
    }
    #endregion
    /// <summary>
    /// 创建棋盘地形
    /// </summary>
    private void CreateBoardTerrain()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                GameObject go = Instantiate(GameConst.GetInstance().PrefabCube, BoardParent.transform);
                go.AddComponent<MeshCollider>();
                go.transform.localPosition = new Vector3(i, 0, j);
                go.tag = "BoardItem";

                //获取坐标
                Vector2Int v2 = new Vector2Int(Mathf.RoundToInt(go.transform.position.x),
                    Mathf.RoundToInt(go.transform.position.z));
                //判断字典内是否存在
                if (!boardDic.ContainsKey(v2) || go.transform.position.y > boardDic[v2].vector3.y)
                {
                    //放入数组
                    boardDic[v2] = new BoardTerrinItem(v2, go.transform.position);
                }
            }
        }
    }
    /// <summary>
    /// 创建棋盘地形（新）
    /// </summary>
    private void NewCreateBoardTerrain()
    {
        GameObject go = Instantiate(GameConst.GetInstance().PrefabCube, BoardParent.transform);
        go.AddComponent<MeshCollider>();
        go.transform.localPosition = new Vector3(10, 0, 10);
        go.transform.localScale = new Vector3(30, 1, 30);
        go.tag = "BoardItem";
        for (int i = 0; i <20; i++)
        {
            for (int j = 0; j <20; j++)
            {
                //获取坐标
                Vector2Int v2 = new Vector2Int(i,j);
                //放入数组
                boardDic[v2] = new BoardTerrinItem(v2, new Vector3(i, 0, j));
            }
        }
    }
    /// <summary>
    /// 创建敌人棋子
    /// </summary>
    private void CreateEnemyChessItem()
    {
        int[] startX = new int[10] { 9, 11, 7, 13, 5, 15 ,3,17,1,19};
        for (int i = 0; i < GameConst.GetInstance().EnemyPrefab.Count; i++)
        {
            //创建
            GameConst.GetInstance().EnemyPrefab[i] = Instantiate(GameConst.GetInstance().EnemyPrefab[i]);
            //随机坐标
            Vector2Int v2 = new Vector2Int(startX[i], UnityEngine.Random.Range(15, 20));
            //设置坐标
            GameConst.GetInstance().EnemyPrefab[i].transform.position = new Vector3(v2.x, 1, v2.y);
            //设置方向
            GameConst.GetInstance().EnemyPrefab[i].transform.eulerAngles = Vector3.up * 180;
            //特殊设置
            ChessState cs = GameConst.GetInstance().EnemyPrefab[i].GetComponent<ChessState>() ?? GameConst.GetInstance().EnemyPrefab[i].AddComponent<ChessState>();
            //敌方棋子
            cs.InitBeforeBattle(100);
            AIControl AIC = GameConst.GetInstance().EnemyPrefab[i].GetComponent<AIControl>() ?? GameConst.GetInstance().EnemyPrefab[i].AddComponent<AIControl>();
            //修改字典信息
            UpdateBoardDic(GameConst.GetInstance().EnemyPrefab[i].GetComponent<ChessManager>()??GameConst.GetInstance().EnemyPrefab[i].AddComponent<ChessManager>(), v2);
        }
    }
    /// <summary>
    /// 创建友方棋子
    /// </summary>
    private void CreatePlayerChessItem()
    {
        int[] startX = new int[10] { 9, 11, 7, 13, 5, 15, 3, 17, 1, 19 };
        for (int i = 0; i < GameConst.GetInstance().PlayerchessPerfab.Count; i++)
        {
            //创建
            GameConst.GetInstance().PlayerchessPerfab[i] = Instantiate(GameConst.GetInstance().PlayerchessPerfab[i]);
            //随机坐标
            Vector2Int v2 = new Vector2Int(startX[i], UnityEngine.Random.Range(0, 5));
            //设置坐标
            GameConst.GetInstance().PlayerchessPerfab[i].transform.position = new Vector3(v2.x, 1, v2.y);
            //特殊设置
            ChessState cs = GameConst.GetInstance().PlayerchessPerfab[i].GetComponent<ChessState>() ?? GameConst.GetInstance().PlayerchessPerfab[i].AddComponent<ChessState>();
            cs.InitBeforeBattle(1);
            //修改字典信息
            UpdateBoardDic(GameConst.GetInstance().PlayerchessPerfab[i].GetComponent<ChessManager>()?? GameConst.GetInstance().PlayerchessPerfab[i].AddComponent<ChessManager>(), v2);
        }
    }
    /// <summary>
    /// 棋子移动后 修改字典信息
    /// </summary>
    public void UpdateBoardDic(ChessManager cm, Vector2Int startV2, Vector2Int endV2)
    {
        //格子没有棋子
        boardDic[startV2].currentChessManaager = null;
        //格子为空地
        boardDic[startV2].BoardType = 0;
        //新格子信息
        UpdateBoardDic(cm, endV2);
    }
    /// <summary>
    /// 棋子移动后 修改字典信息
    /// </summary>
    public void UpdateBoardDic(ChessManager cm, Vector2Int startV2)
    {
        //新格子上有棋子
        boardDic[startV2].currentChessManaager = cm;
        //格子不为空地
        boardDic[startV2].BoardType = cm._chessState.team;
    }
    /// <summary>
    /// 创建棋盘字典
    /// </summary>
    private void CreateBoardDic()
    {
        //获取Transform组件
        Transform _transform = BoardParent.transform;
        //遍历所有格子
        for (int i = 0; i < _transform.childCount; i++)
        {
            //排除障碍物
            if (!_transform.GetChild(i).CompareTag("BoardItem"))
                continue;
            //获取坐标
            Vector2Int v2 = new Vector2Int((int)_transform.GetChild(i).transform.position.x,
                (int)_transform.GetChild(i).transform.position.z);
            //判断字典内是否存在
            if (!boardDic.ContainsKey(v2) || _transform.GetChild(i).position.y > boardDic[v2].vector3.y)
            {
                //放入数组
                boardDic[v2] = new BoardTerrinItem(v2, _transform.GetChild(i).transform.position);
            }
        }
    }
    public ChessManager GetChessByPoint(int x, int y)
    {
        if (boardDic[new Vector2Int(x, y)].BoardType != 0)
            return boardDic[new Vector2Int(x, y)].currentChessManaager;
        return null;
    }
    #region 搜索设置
    //搜索方向（方格8方向）
    private int[] dirX = { 1, 0, -1, 0, 1, -1, 1, -1 };
    private int[] dirY = { 0, 1, 0, -1, 1, -1, -1, 1 };
    //搜寻数组
    Dictionary<Vector2Int, AStarBase> aStarBase;
    #endregion
    #region BFS搜索
    /// <summary>
    /// 广度优先算法
    /// </summary>
    /// <param name="start">起始点坐标</param>
    /// <param name="boards">可移动范围数组</param>
    /// <param name="maxdistance">棋子最大移动距离</param>
    /// <returns></returns>
    public void BFS_Skill(Vector2Int start, int maxdistance,Queue<Vector2Int> path)
    {
        //初始化队列
        Queue<Vector3Int> points = new Queue<Vector3Int>();
        //初始化寻路结果
        int[,] result = new int[20, 20];
        //初始化起点
        result[start.x, start.y] = 1;
        //起点入列
        points.Enqueue(new Vector3Int(start.x, start.y, 0));
        //加入路径
        path.Enqueue(start);
        int x, y, step;
        while (points.Count > 0)
        {
            Vector3Int point = points.Dequeue();
            x = point.x;
            y = point.y;
            step = point.z;
            //是否超出最大步长
            if (step >= maxdistance)
                continue;
            for (int i = 0; i < 4; i++)
            {
                //计算新的坐标点
                int new_x = x + dirX[i], new_y = y + dirY[i];
                Vector2Int new_pos = new Vector2Int(new_x, new_y);
                #region 条件判断
                //①越界
                if (!boardDic.ContainsKey(new_pos))
                    continue;
                //④防止回头
                if (result[new_x, new_y] == 1)
                    continue;
                #endregion
                //新坐标可到入队
                points.Enqueue(new Vector3Int(new_x, new_y, step + 1));
                //加入路径
                path.Enqueue(new_pos);
                //标记为走过
                result[new_x, new_y] = 1;
            }
        }
        //起点初始化
        result[start.x, start.y] = 0;
    }
    public int[,] BFS_Attack(Vector2Int start, int maxdistance)
    {
        //初始化队列
        Queue<Vector3Int> points = new Queue<Vector3Int>();
        //初始化寻路结果
        int[,] result = new int[20, 20];
        //初始化起点
        result[start.x, start.y] = 1;
        //起点入列
        points.Enqueue(new Vector3Int(start.x, start.y, 0));
        int x, y, step;
        while (points.Count > 0)
        {
            Vector3Int point = points.Dequeue();
            x = point.x;
            y = point.y;
            step = point.z;
            //是否超出最大步长
            if (step >= maxdistance)
                continue;
            for (int i = 0; i < 4; i++)
            {
                //计算新的坐标点
                int new_x = x + dirX[i], new_y = y + dirY[i];
                Vector2Int new_pos = new Vector2Int(new_x, new_y);
                #region 条件判断
                //①越界
                if (!boardDic.ContainsKey(new_pos))
                    continue;
                //④防止回头
                if (result[new_x, new_y] == 1)
                    continue;
                #endregion
                //新坐标可到入队
                points.Enqueue(new Vector3Int(new_x, new_y, step + 1));
                //标记为走过
                result[new_x, new_y] = 1;
            }
        }
        //起点初始化
        result[start.x, start.y] = 0;
        return result;
    }
    public int[,] BFS_Move(Vector2Int start, int maxdistance, int team)
    {
        //初始化队列
        Queue<Vector3Int> points = new Queue<Vector3Int>();
        //初始化寻路结果
        int[,] result = new int[20, 20];
        //初始化起点
        result[start.x, start.y] = 1;
        //起点入列
        points.Enqueue(new Vector3Int(start.x, start.y, 0));
        int x, y, step;
        while (points.Count > 0)
        {
            Vector3Int point = points.Dequeue();
            x = point.x;
            y = point.y;
            step = point.z;
            //是否超出最大步长
            if (step >= maxdistance)
                continue;
            for (int i = 0; i < 4; i++)
            {
                //计算新的坐标点
                int new_x = x + dirX[i], new_y = y + dirY[i];
                Vector2Int new_pos = new Vector2Int(new_x, new_y);
                #region 条件判断
                //①越界
                if (!boardDic.ContainsKey(new_pos))
                    continue;
                //②被敌方占领
                if (boardDic[new_pos].BoardType != team && boardDic[new_pos].BoardType != 0)
                    continue;
                //④防止回头
                if (result[new_x, new_y] == 1)
                    continue;
                #endregion
                //新坐标可到入队
                points.Enqueue(new Vector3Int(new_x, new_y, step + 1));
                //标记为走过
                result[new_x, new_y] = 1;
            }
        }
        //起点初始化
        result[start.x, start.y] = 0;
        return result;
    }
    #endregion
    #region A星搜索
    /// <summary>
    /// A*搜索元件
    /// </summary>
    public class AStarBase : System.IComparable<AStarBase>
    {
        int x, y, g, h, f;
        public AStarBase finder;
        public bool close;
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int G { get => g; set => g = value; }
        public int H { get => h; set => h = value; }
        public int F { get => f; set => f = value; }

        //构造方法
        public AStarBase(int x, int y, AStarBase finder)
        {
            X = x;
            Y = y;
            G = 0;
            this.finder = finder;
            close = false;
        }
        //比较坐标
        public bool CompareXY(int x, int y)
        {
            if (X == x && Y == y)
                return true;
            return false;
        }
        //设置观察者
        public void SetWeight(int g, int h, AStarBase finder)
        {
            G = g;
            H = h;
            F = G + H;
            this.finder = finder;
        }
        public Vector2Int vector2()
        {
            return new Vector2Int(X, Y);
        }
        public int CompareTo(AStarBase other)
        {
            if (F < other.F)
                return -1;
            if (F > other.F)
                return 1;
            return 0;
        }
    }
    /// <summary>
    /// 计算格子的观察者
    /// </summary>
    /// <param name="start">起始点坐标</param>
    /// <param name="boards">可移动的范围</param>
    /// <param name="end">目的地坐标</param>
    public void AStar(Vector2Int start, Vector2Int end, ref Stack<Vector3> path, int team)
    {
        //设置数组
        aStarBase = new Dictionary<Vector2Int, AStarBase>();
        //初始化结果
        List<AStarBase> open = new List<AStarBase>();
        //起始点没有观察者
        aStarBase[start] = new AStarBase(start.x, start.y, null);
        //起点入列
        open.Add(aStarBase[start]);
        //有路可走
        while (open.Count > 0)
        {
            //排序
            open.Sort();
            //拿出F值最小的格子
            AStarBase current = open[0];
            //原先高度
            float cuurentHeight = boardDic[current.vector2()].Height;
            //是否到达终点
            if (current.CompareXY(end.x, end.y))
            {
                //生成路径
                PushPath(current, ref path);
                break;
            }
            //遍历周边格子
            for (int i = 0; i < 4; i++)
            {
                //计算新坐标
                int newX = current.X + dirX[i];
                int newY = current.Y + dirY[i];
                Vector2Int newV2 = new Vector2Int(newX, newY);

                //排除意外
                //①越界
                if (!boardDic.ContainsKey(newV2))
                    continue;
                //②障碍物
                if (boardDic[newV2].BoardType != 0 && boardDic[newV2].BoardType != team)
                    continue;
                //赋值
                if (!aStarBase.ContainsKey(newV2))
                    aStarBase[newV2] = new AStarBase(newX, newY, current);
                //①高度差
                if (Math.Abs(boardDic[newV2].Height - cuurentHeight) > 0.5)
                    continue;
                //③排除观察者
                if (aStarBase[newV2].close)
                    continue;
                //计算格子权重
                int G;
                if (dirX[i] == 0 || dirY[i] == 0)
                    G = 10 + current.G;
                else
                    G = 14 + current.G;
                //如果G值为设置 或者 比当前G值大 则重新设置权重和观察者
                if (aStarBase[newV2].G == 0 || G < aStarBase[newV2].G)
                {
                    int H = (end.x - newX > 0 ? end.x - newX : -end.x + newX)
                        + (end.y - newY > 0 ? end.y - newY : -end.y + newY);
                    //设置
                    aStarBase[newV2].SetWeight(G, H, current);
                }
                //是否在待观察数组中
                if (!open.Contains(aStarBase[newV2]))
                    open.Add(aStarBase[newV2]);
            }
            //当前格子观察完毕
            current.close = true;
            //移除当前格子
            open.Remove(current);
        }
    }
    /// <summary>
    /// 计算格子的观察者
    /// </summary>
    /// <param name="start">起始点坐标</param>
    /// <param name="boards">可移动的范围</param>
    /// <param name="end">目的地坐标</param>
    public void AIAStar(Vector2Int start, Vector2Int end, ref Stack<Vector3> path, int team)
    {
        //设置数组
        aStarBase = new Dictionary<Vector2Int, AStarBase>();
        //初始化结果
        List<AStarBase> open = new List<AStarBase>();
        //起始点没有观察者
        aStarBase[start] = new AStarBase(start.x, start.y, null);
        //起点入列
        open.Add(aStarBase[start]);
        //有路可走
        while (open.Count > 0)
        {
            //排序
            open.Sort();
            //拿出F值最小的格子
            AStarBase current = open[0];
            //原先高度
            float cuurentHeight = boardDic[current.vector2()].Height;
            //是否到达终点
            if (current.CompareXY(end.x, end.y))
            {
                //生成路径
                PushPath(current, ref path);
                break;
            }
            //遍历周边格子
            for (int i = 0; i < 4; i++)
            {
                //计算新坐标
                int newX = current.X + dirX[i];
                int newY = current.Y + dirY[i];
                Vector2Int newV2 = new Vector2Int(newX, newY);

                //排除意外
                //①越界
                if (!boardDic.ContainsKey(newV2))
                    continue;
                if (newV2 != end)
                {
                    //②障碍物
                    if (boardDic[newV2].BoardType != 0)
                        continue;
                }
                //赋值
                if (!aStarBase.ContainsKey(newV2))
                    aStarBase[newV2] = new AStarBase(newX, newY, current);
                //①高度差
                if (Math.Abs(boardDic[newV2].Height - cuurentHeight) > 0.5)
                    continue;
                //③排除观察者
                if (aStarBase[newV2].close)
                    continue;

                //计算格子权重
                int G;
                if (dirX[i] == 0 || dirY[i] == 0)
                    G = 10 + current.G;
                else
                    G = 14 + current.G;
                //如果G值为设置 或者 比当前G值大 则重新设置权重和观察者
                if (aStarBase[newV2].G == 0 || G < aStarBase[newV2].G)
                {
                    int H = (end.x - newX > 0 ? end.x - newX : -end.x + newX)
                        + (end.y - newY > 0 ? end.y - newY : -end.y + newY);
                    //设置
                    aStarBase[newV2].SetWeight(G, H, current);
                }
                //是否在待观察数组中
                if (!open.Contains(aStarBase[newV2]))
                    open.Add(aStarBase[newV2]);
            }
            //当前格子观察完毕
            current.close = true;
            //移除当前格子
            open.Remove(current);
        }
    }
    /// <summary>
    /// 路径压栈
    /// </summary>
    /// <param name="Info">终点元件</param>
    private void PushPath(AStarBase Info, ref Stack<Vector3> path)
    {
        //进栈
        path.Push(boardDic[new Vector2Int(Info.X, Info.Y)].vector3);
        if (Info.finder != null)
        {
            //存在观察者则继续压栈
            PushPath(Info.finder, ref path);
        }
    }
    #endregion
}
public class BoardTerrinItem
{
    //二维坐标
    public Vector2Int vector2;
    //三维坐标
    public Vector3 vector3;
    //格子海拔高度
    public float Height;
    //格子类型
    public int BoardType = 1;
    //处在当前格子的物体
    public ChessManager currentChessManaager;

    public BoardTerrinItem(Vector2Int vector2, Vector3 vector3)
    {
        this.vector2 = vector2;//二维坐标
        this.vector3 = vector3;//三维坐标
        Height = vector3.y;//格子海拔
        BoardType = 0;//空地
    }
}
