using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class GameManager : MonoBehaviour
{
    //本体
    public static GameManager instance;
    void Awake()
    {
        instance = instance == null ? this : instance;
        //初始化
        heroes = new List<Hero>();
        //读取信息
        //如果没有信息
        if (heroes.Count == 0)
        {
            //制作测试数据
            //制作一个英雄
            heroes.Add(new Hero("Wizard", HeroType.Wizard, 1, 0, HeroEnum.WizardBlue));
            heroes.Add(new Hero("Wizard", HeroType.Wizard, 1, 0, HeroEnum.WizardPurple));
            heroes.Add(new Hero("Wizard", HeroType.Wizard, 1, 0, HeroEnum.WizardRed));
            heroes.Add(new Hero("Wizard", HeroType.Wizard, 1, 0, HeroEnum.WizardRed));
            heroes.Add(new Hero("Wizard", HeroType.Wizard, 1, 0, HeroEnum.WizardPurple));
            //设置目标
            targetHero = heroes[0].heroEnum;
        }
    }
    void Start()
    {
        //制作大地图英雄
        Vector3 point = new Vector3(10, 5, 4);
        //加载英雄
        GameObject heroModel = Resources.Load<GameObject>("Hero/"+targetHero.ToString());
        //生成英雄
        heroModel = Instantiate(heroModel, point, Quaternion.identity);
        //添加必要组件
        heroModel.AddComponent<ChessManager>();
        //获取摄像机
        camearManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamearManager>();
        //摄像机跟随
        camearManager.Target = heroModel.transform;
        camearManager.enabled = true;
    }

    //摄像头焦点英雄
    public HeroEnum targetHero;
    //摄像头
    public MainCamearManager camearManager;
    //账号佣兵信息
    public List<Hero> heroes;

}
public enum HeroType
{
    Barbarian = 1,
    Archer,
    Knight,
    Wizard
}
public class Hero
{
    //英雄名称
    public string HeroName;
    //英雄职业
    public HeroType heroType;
    //英雄等级
    public int Level;
    //当前经验
    public int Exp;
    //英雄数据路径
    public int[] State;
    //英雄形象枚举
    public HeroEnum heroEnum;
    //构造
    public Hero(string heroName, HeroType heroType, int level, int exp, HeroEnum heroEnum)
    {
        HeroName = heroName;
        this.heroType = heroType;
        Level = level;
        Exp = exp;
        this.heroEnum = heroEnum;
        State = new int[10];
        InitState();
    }
    //lua文件
    LuaState lua = null;
    //lua方法
    LuaFunction func = null;
    public void InitState()
    {
        for (int i = 0; i < 7; i++)
        {
            State[i] = GetHeroStateByLua(i+1);
        }
    }
    public int GetHeroStateByLua(int statenum)
    {
        //新建
        lua = new LuaState();
        lua.Start();
        //设置lua路径
        lua.AddSearchPath(GameConst.GetInstance().path);
        //加载lua
        lua.DoFile("HeroBaseState.lua");
        //获取Lua方法
        func = lua.GetFunction("GetState");
        //获得当前等级当前职业 某个属性的值
        int num = func.Invoke<int, int, int, int>((int)heroType, Level, statenum);
        //方法值
        return num;
    }
}