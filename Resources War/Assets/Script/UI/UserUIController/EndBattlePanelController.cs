using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
using Utilty;
using UnityEngine.UI;

public class EndBattlePanelController : UIControllerBase
{
    //根物体
    private UIWidgetsBase root;
    //父物体
    private Transform parent;
    //数量
    private int count=3;

    /// <summary>
    /// 启动控制器
    /// </summary>
    /// <param name="module"></param>
    public override void ControllerStart(UIModuleBase module)
    {
        base.ControllerStart(module);
        //找到根物体
        root = _module.FindCurrentModuleWidget("Trophys#");
        CreateTrophy();
        ClosePage();
    }


    /// <summary>
    /// 生成材料
    /// </summary>
    private void CreateTrophy()
    {
        /*
        //遍历奖励列表
        for (int i = 0; i < GameConst.GetInstance().activeInfo.Award.Length; i++)
        {
            //设置父物体
            parent = root.transform.GetChild(i);
            //获取【材料图片】元件预设体路径
            string path = UIConfigurationManager.GetInstance().GetWidgetAssetPathByName("TrophyImg");
            //通过对象池生成预设体
             GameObject go = ObjectPool.GetInstance().SpawnObject(path, new object[] { parent, _module });
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>(
                "Sprite/Collection/"+ GameConst.GetInstance().activeInfo.Award[i].ToString());
            
            //获取【材料数量】元件预设体路径
            string numberPath = UIConfigurationManager.GetInstance().GetWidgetAssetPathByName("TrophyNumber");
            //通过对象池生成预设体
            ObjectPool.GetInstance().SpawnObject(numberPath, new object[] { parent, _module });
            
        }
        */
    }

    /// <summary>
    /// 关闭当前页面，返回上一页面
    /// </summary>
    private void ClosePage()
    {
        //获取关闭按钮
        UIWidgetsBase backBtn = _module.FindCurrentModuleWidget("BackBtn#");
        backBtn.Button.onClick.RemoveAllListeners();
        //添加事件
        backBtn.Button.onClick.AddListener(() =>
        {
            Facace.instance.LoadScene("MainScene");
        });
    }
}
