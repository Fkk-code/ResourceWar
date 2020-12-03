using UnityEngine;
using UIFrame;
using Utilty;
using UnityEngine.UI;

public class PlayerInfoPanelController : UIControllerBase
{
    //玩家列表父对象
    Transform playerList;
    //玩家信息View
    UIWidgetsBase[] heroStateUB;
    string[] viewName;
    /// <summary>
    /// 启动控制器
    /// </summary>
    /// <param name="module"></param>
    public override void ControllerStart(UIModuleBase module)
    {
        base.ControllerStart(module);
        //打开背包页面
        OpenKnapsackPane();
        //找到玩家列表父对象
        playerList = _module.FindCurrentModuleWidget("PlayerList#").Transform;
        //获取全部显示UI
        GetHeroUIWB();
        //添加玩家信息按钮
        AddPlyer();
        //初始化显示面板
        ShowHeroState(0);
    }
    private void GetHeroUIWB()
    {
        heroStateUB = new UIWidgetsBase[11];
        viewName = new string[11] {
            "Name#",
            "Level#",
            "Exp#",
            "Profession#",
            "Hp#",
            "Mp#",
            "PA#",
            "PD#",
            "MA#",
            "MD#",
            "Speed#"
        };
        //获取元件
        for (int i = 0; i < 11; i++)
        {
            heroStateUB[i] = _module.FindCurrentModuleWidget(viewName[i]);
        }
    }
    /// <summary>
    /// 打开背包页面
    /// </summary>
    private void OpenKnapsackPane()
    {
        //找到“主手”按钮
        UIWidgetsBase mainHanBtn = _module.FindCurrentModuleWidget("MainHand_Btn#");
        //绑定事件
        mainHanBtn.Button.onClick.AddListener(() =>
        {
            //打开背包页面
            UIManager.GetInstance().OpenModule("KnapsackPanel");
        });
        //找到“装备”按钮
        UIWidgetsBase equipmentBtn = _module.FindCurrentModuleWidget("Equiment_Btn#");
        //绑定事件
        equipmentBtn.Button.onClick.AddListener(() =>
        {
            //打开背包页面
            UIManager.GetInstance().OpenModule("KnapsackPanel");
        });
        //找到“副手”按钮
        UIWidgetsBase offHandBtn = _module.FindCurrentModuleWidget("OffHand_Btn#");
        //绑定事件
        offHandBtn.Button.onClick.AddListener(() =>
        {
            //打开背包页面
            UIManager.GetInstance().OpenModule("KnapsackPanel");
        });
    }
    /// <summary>
    /// 添加玩家
    /// </summary>
    private void AddPlyer()
    {
        //根据玩家数量设置生成预设体的数量
        for (int i = 0; i < GameManager.instance.heroes.Count; i++)
        {
            //获取元件预设体路径
            string path = UIConfigurationManager.GetInstance().GetWidgetAssetPathByName("ChangerPalyer");
            //通过对象池生成预设体
            GameObject go =  ObjectPool.GetInstance().SpawnObject(path,new object[] { playerList,_module});
            //修改名称
            go.name = i.ToString();
            go.GetComponent<UIWidgetsBase>().Button.onClick.RemoveAllListeners();
            //获取组件添加事件
            go.GetComponent<UIWidgetsBase>().Button.onClick.AddListener(() =>
            {
                int num = int.Parse(go.name);
                Debug.Log(num);
                ShowHeroState(num);

            });
            //显示英雄名称
            go.transform.Find("Name").GetComponent<Text>().text = GameManager.instance.heroes[i].HeroName;
            //显示英雄等级
            go.transform.Find("Level").GetComponent<Text>().text = GameManager.instance.heroes[i].Level.ToString();
        }
    }
    private void ShowHeroState(int num)
    {
        //英雄头像
        _module.FindCurrentModuleWidget("PlayerImage#").Image.sprite =
            Resources.Load<Sprite>("Sprite/Hero/" + GameManager.instance.heroes[num].heroEnum.ToString());
        //名字
        heroStateUB[0].Text.text = GameManager.instance.heroes[num].HeroName;
        //等级
        heroStateUB[1].Text.text = GameManager.instance.heroes[num].Level.ToString();
        //经验
        heroStateUB[2].Text.text = GameManager.instance.heroes[num].Exp.ToString();
        //职业
        heroStateUB[3].Text.text = GameManager.instance.heroes[num].heroType.ToString();
        //刷新基础属性
        for (int i = 4; i < heroStateUB.Length; i++)
        {
            heroStateUB[i].Text.text = GameManager.instance.heroes[num].State[i - 4].ToString();
        }
    }
    /// <summary>
    /// 关闭玩家信息页面
    /// </summary>
    public void ClosePlayerPanel()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UIManager.GetInstance().CloseModule("PlayerInfoPanel");
        }
    }
}
