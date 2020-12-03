using System.Collections;
using UnityEngine;
using UIFrame;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System;

public class Facace : MonoBehaviour
{
    public static Facace instance;
    private PlayableDirector playableDirector;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        LoadUIByScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// 通过场景更新UI
    /// </summary>
    public void LoadUIByScene(string name)
    {
        switch (name)
        {
            case "AnimationScene":
                //开始场景
                playableDirector = GameObject.Find("TimeLine").GetComponent<PlayableDirector>();
                break;
            case "BattleScene":
                //战斗场景
                break;
            case "MainScene":
                //主场景
                LoadMainScene();
                break;
        }
        //设置UI


    }
    private void LoadMainScene()
    {
        UIManager.GetInstance().OpenModule("Main_Panel");
        try
        {
            UIManager.GetInstance().CloseModule("StartPanel");
            UIManager.GetInstance().CloseModule("BattlePanel#");
            UIManager.GetInstance().CloseModule("EndBattlePanel");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    public void LoadBattleScene()
    {
        UIManager.GetInstance().OpenModule("BattlePanel#");
        try
        {
            UIManager.GetInstance().CloseModule("Main_Panel");
            UIManager.GetInstance().CloseModule("SelectHeroPanel");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="name"></param>
    public void LoadScene(string name)
    {
        //切换场景
        StartCoroutine(EnterBattleScene());
        //异步加载场景
        IEnumerator EnterBattleScene()
        {
            //进度条
            int program = 0;
            //异步加载场景
            AsyncOperation op = SceneManager.LoadSceneAsync(name);
            //开启加载场景UI
            UIManager.GetInstance().OpenModule("LoadScenePanel");
            //更新数字
            UIManager.GetInstance().FindWidget("LoadScenePanel", "program#").Text.text = program.ToString();
            //关闭场景直接跳转跳转
            op.allowSceneActivation = false;
            //场景预先加载
            yield return new WaitForEndOfFrame();
            //不摧毁画布
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Canvas"));
            while (program < 70)
            {
                program += UnityEngine.Random.Range(5, 20);
                //更新数字
                UIManager.GetInstance().FindWidget("LoadScenePanel", "program#").Text.text = program.ToString();
                //_log = "加载场景：" + program + "%";
                yield return new WaitForSeconds(0.2f);
            }
            //更新数字
            program = 100;
            UIManager.GetInstance().FindWidget("LoadScenePanel", "program#").Text.text = program.ToString();
            //_log = "加载场景：" + program + "%";
            yield return new WaitForSeconds(1f);
            //加载场景
            op.allowSceneActivation = true;
            //关闭加载场景UI
            UIManager.GetInstance().CloseModule("LoadScenePanel");
            //刷新UI
            LoadUIByScene(name);
        }
    }
    /// <summary>
    /// 开始场景
    /// </summary>
    public void StartUI()
    {
        //暂停TimeLine
        playableDirector.Pause();
        //打开StartPanel模块
        UIManager.GetInstance().OpenModule("StartPanel");
    }
}
