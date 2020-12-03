using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UIFrame;
public class InteractiveProp : MonoBehaviour
{
    //弹出信息
    public string _log = "钓鱼";
    public GameObject prefabs;
    public CollectionEnum[] Award;
    public EnemyEnum[] enemyList;
    /// <summary>
    /// 设置战斗场景
    /// </summary>
    public void InitMapScene()
    {
        //设置地板预制体
        GameConst.GetInstance().PrefabCube = prefabs;
        //清空
        GameConst.GetInstance().EnemyPrefab.Clear();
        //敌人数量
        int enemyNum = Random.Range(GameConst.GetInstance().PlayerchessPerfab.Count, GameConst.GetInstance().PlayerchessPerfab.Count+3);
        //制作敌人
        for (int i = 0; i < enemyNum; i++)
        {
            //路径
            string path = "Enemy/" + enemyList[Random.Range(0, enemyList.Length)].ToString();
            //添加敌人预制体
            GameConst.GetInstance().EnemyPrefab.Add(Resources.Load<GameObject>(path));
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        GameConst.GetInstance().activeInfo = this;
        UIManager.GetInstance().OpenModule("SelectHeroPanel");
    }
    public void OnTriggerExit(Collider collider)
    {
        UIManager.GetInstance().CloseModule("SelectHeroPanel");
    }
}
