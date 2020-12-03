using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class SImpleTool : Editor
{
    [MenuItem("SimpleTool/CreateBaseFolder（创建必要文件夹）")]
    private static void CreateBaseFolder()
    {
        //主体文件夹
        CreateFolder("ResourcesWar");
        //Resources 文件夹
        CreateFolder("ResourcesWar/Resources/GameObject");
        CreateFolder("ResourcesWar/Resources/Sprite");
        CreateFolder("ResourcesWar/Resources/UI");
        //玩家 文件夹
        CreateFolder("ResourcesWar/01Player");
        CreateFolder("ResourcesWar/01Player/01Prefabs");
        CreateFolder("ResourcesWar/01Player/02Script");
        CreateFolder("ResourcesWar/01Player/03Animation");
        CreateFolder("ResourcesWar/01Player/04Animator");
        CreateFolder("ResourcesWar/01Player/05Voice");
        //敌人 文件夹
        CreateFolder("ResourcesWar/02Enemy");
        CreateFolder("ResourcesWar/02Enemy/01Prefabs");
        CreateFolder("ResourcesWar/02Enemy/02Script");
        CreateFolder("ResourcesWar/02Enemy/03Animation");
        CreateFolder("ResourcesWar/02Enemy/04Animator");
        CreateFolder("ResourcesWar/02Enemy/05Voice");
        //地形 文件夹
        CreateFolder("ResourcesWar/03Terrain");
        CreateFolder("ResourcesWar/03Terrain/01Prefabs");
        CreateFolder("ResourcesWar/03Terrain/02Script");
        CreateFolder("ResourcesWar/03Terrain/03Animation");
        CreateFolder("ResourcesWar/03Terrain/04Animator");
        CreateFolder("ResourcesWar/03Terrain/05Voice");
        //UI 文件夹
        CreateFolder("ResourcesWar/04UI");
        CreateFolder("ResourcesWar/04UI/01Prefabs");
        CreateFolder("ResourcesWar/04UI/02Script");
        CreateFolder("ResourcesWar/04UI/03Animation");
        CreateFolder("ResourcesWar/04UI/04Animator");
        CreateFolder("ResourcesWar/04UI/05Voice");
        //道具资源 文件夹
        CreateFolder("ResourcesWar/05Prop");
        //场景 文件夹
        //CreateFolder("ResourcesWar/06Scenes");
    }
    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="folderName"></param>
    private static void CreateFolder(string folderName)
    {
        //制作路径
        string path = Application.dataPath + "/" + folderName;
        //查看路径是否存在
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
    //棋盘父物体
    private static GameObject newBoard;
    //文件名字
    private static string TextName = "BoardJsonText";
    //文件路径
    private static string TextPath = Application.dataPath + "/" + TextName + ".txt";
    [MenuItem("SimpleTool/AlignBoard(对齐棋盘)")]
    private static void AlignBoard()
    {
        //获得棋盘父物体
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        //遍历格子
        for (int i = 0; i < board.transform.childCount; i++)
        {
            //获取格子
            GameObject cube = board.transform.GetChild(i).gameObject;
            cube.transform.localPosition =
                new Vector3(Mathf.Round(cube.transform.localPosition.x),
                cube.transform.localPosition.y,
                Mathf.Round(cube.transform.localPosition.z));
        }
    }
    [MenuItem("SimpleTool/CreateBoardItemToJson（把棋盘转成Json）")]
    private static void CreateBoardItem()
    {
        //获得棋盘父物体
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        //格子列表
        List<CubeItem> AllCube = new List<CubeItem>();
        //遍历格子
        for (int i = 0; i < board.transform.childCount; i++)
        {
            //获取格子
            GameObject cube = board.transform.GetChild(i).gameObject;
            //获取父类
            Object prefabs = PrefabUtility.GetCorrespondingObjectFromSource(cube);
            //父类路径
            string path = AssetDatabase.GetAssetPath(prefabs);
            //创建CubeItem
            CubeItem cubeItem = new CubeItem(cube.transform.position, cube.transform.rotation, path);
            //添加
            AllCube.Add(cubeItem);
        }
        //棋盘对象
        BoardItem boardItem = new BoardItem();
        boardItem.cubeItems = AllCube;
        //获取Json字符创
        string ss = JsonUtility.ToJson(boardItem);
        Debug.Log(ss);
        //生成文件
        File.WriteAllText(TextPath, ss);
    }
    [MenuItem("SimpleTool/CreateBoardItemForJson（把Json转成棋盘）")]
    private static void CreateBoard()
    {
        //获取棋盘父物体
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        //读取文件
        string s = File.ReadAllText(TextPath);
        //新建对象
        BoardItem boardItem = new BoardItem();
        //解析Json
        boardItem = JsonUtility.FromJson<BoardItem>(s);
        //输出结果
        Debug.Log(boardItem.cubeItems.Count);
        //遍历生成
        for (int i = 0; i < boardItem.cubeItems.Count; i++)
        {
            //获取需要加载的资源
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(boardItem.cubeItems[i].path);
            //实例化并生成坐标
            Instantiate(go, boardItem.cubeItems[i].GetPosition(), boardItem.cubeItems[i].GetRotation())
                .transform.SetParent(board.transform);
        }
    }
    [MenuItem("SimpleTool/ShowBoardSize（显示棋盘基本信息）")]
    private static void MoveToZero()
    {
        //获得棋盘父物体
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        //初始化坐标
        int max_x = -100, max_y = -100, min_x = 100, min_y = 100;
        //读取文件
        string s = File.ReadAllText(TextPath);
        //新建对象
        BoardItem boardItem = new BoardItem();
        //解析Json
        boardItem = JsonUtility.FromJson<BoardItem>(s);
        //输出结果
        Debug.Log(boardItem.cubeItems.Count+"个方块");
        //遍历坐标
        for (int i = 0; i < boardItem.cubeItems.Count; i++)
        {
            max_x = max_x < boardItem.cubeItems[i].x ? boardItem.cubeItems[i].x : max_x;
            max_y = max_y < boardItem.cubeItems[i].z ? boardItem.cubeItems[i].z : max_y;

            min_x = min_x > boardItem.cubeItems[i].x ? boardItem.cubeItems[i].x : min_x;
            min_y = min_y > boardItem.cubeItems[i].z ? boardItem.cubeItems[i].z : min_y;
        }
        Debug.Log("左下角坐标：(" + min_x + "," + min_y + ")");
        Debug.Log("右上角坐标：(" + max_x + "," + max_y + ")");
        int BoardSize = (max_x - min_x) * (max_y - min_y);
        Debug.Log("当前棋盘长为：" + (max_x - min_x) + "宽为：" + (max_y - min_y) + "面积为：" + BoardSize);
    }

}
[System.Serializable]
public class BoardItem
{
    public List<CubeItem> cubeItems;
}
[System.Serializable]
public class CubeItem
{
    //资源坐标
    public int x;
    public int y;
    public int z;
    //资源转角
    public float Qx;
    public float Qy;
    public float Qz;
    public float Qw;
    //资源预制体
    public string path;
    public CubeItem(Vector3 v3, Quaternion q, string path)
    {
        x = (int)v3.x;
        y = (int)v3.y;
        z = (int)v3.z;
        Qx = q.x;
        Qy = q.y;
        Qz = q.z;
        Qw = q.w;
        this.path = path;
    }
    //返回三维坐标
    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
    //返回二维坐标
    public Vector2 GetVector()
    {
        return new Vector2Int(x, z);
    }
    //返回四元素
    public Quaternion GetRotation()
    {
        return new Quaternion(Qx, Qy, Qz, Qw);
    }
    //返回路径
    public string GetPath()
    {
        return path;
    }
}

