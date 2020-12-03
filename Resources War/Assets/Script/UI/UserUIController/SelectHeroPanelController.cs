using System;
using UIFrame;
using UnityEngine;
using UnityEngine.UI;
using Utilty;

public class SelectHeroPanelController : UIControllerBase
{
    public override void ControllerStart(UIModuleBase module)
    {
        base.ControllerStart(module);
        Debug.Log("被打开了123213");
    }

    public void RefreshInfo()
    {
        Debug.Log("被打开了");
        //设置地图名称；
        _module.FindCurrentModuleWidget("MapName#").Text.text = GameConst.GetInstance().activeInfo._log;
        //英雄头像父类
        Transform tff = _module.FindCurrentModuleWidget("HeroList#").transform;
        //清空战斗角色数组
        GameConst.GetInstance().PlayerchessPerfab.Clear();
        //生成
        for (int i = 0; i < GameManager.instance.heroes.Count; i++)
        {
            GameObject go;
            if (i >= tff.childCount)
            {
                //获取元件预设体路径
                string path = UIConfigurationManager.GetInstance().GetWidgetAssetPathByName("HeroToggle#");
                //加载
                go = ObjectPool.GetInstance().SpawnObject(path, new object[] { tff, _module });
            }
            else
            {
                go= tff.GetChild(i).gameObject;
            }
            //设置头像图片
            go.GetComponent<Image>().sprite =
                Resources.Load<Sprite>("Sprite/Hero/" + GameManager.instance.heroes[i].heroEnum.ToString());
            //传参数
            go.transform.GetChild(0).name = i.ToString();
            go.GetComponent<Toggle>().isOn = false;
            go.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            go.GetComponent<Toggle>().onValueChanged.AddListener((addHero) =>
            {
                //收参数
                int nnn = int.Parse(go.transform.GetChild(0).name);
                //刷新实际当前人员
                string ppp = "Hero/" + GameManager.instance.heroes[nnn].heroEnum.ToString();
                GameObject ggg = Resources.Load<GameObject>(ppp);
                if (addHero)
                {
                    GameConst.GetInstance().PlayerchessPerfab.Add(ggg);
                }
                else
                {
                    try
                    {
                        GameConst.GetInstance().PlayerchessPerfab.Remove(ggg);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("没有找到该英雄" + e);
                    }
                }

            });
        }

        //奖励图片
        for (int i = 0; i < GameConst.GetInstance().activeInfo.Award.Length; i++)
        {
            string path = "Sprite/Collection/" + GameConst.GetInstance().activeInfo.Award[i].ToString();
            _module.FindCurrentModuleWidget("Award" + i + "#").Image.sprite = Resources.Load<Sprite>(path);
        }
        //怪物图片
        for (int i = 0; i < GameConst.GetInstance().activeInfo.enemyList.Length; i++)
        {
            string path = "Sprite/Enemy/" + GameConst.GetInstance().activeInfo.enemyList[i].ToString();
            _module.FindCurrentModuleWidget("enemy" + i + "#").Image.sprite = Resources.Load<Sprite>(path);
        }
        //删除原来的事件
        _module.FindCurrentModuleWidget("DepartBtn#").Button.onClick.RemoveAllListeners();
        //添加事件
        _module.FindCurrentModuleWidget("DepartBtn#").Button.onClick.AddListener(() =>
        {
            //设置场景属性
            GameConst.GetInstance().activeInfo.InitMapScene();
            Facace.instance.LoadScene("BattleScene");
        });
    }

    public void UpdatePeople()
    {
        _module.FindCurrentModuleWidget("HeroNum#")
           .Text.text = "Current " + GameConst.GetInstance().PlayerchessPerfab.Count + " / 6";
    }
}
