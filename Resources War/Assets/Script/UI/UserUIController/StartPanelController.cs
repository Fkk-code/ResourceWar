using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
using Utilty;

public class StartPanelController : UIControllerBase
{
    //游戏是否已经开始过
    public bool hasStart = false;
    //按钮生成父物体
    private Transform buttonParent;
    //开始生成按钮编号
    private int index;
    //文字组件父类
    UIWidgetsBase titleText;

    /// <summary>
    /// 启动控制器
    /// </summary>
    /// <param name="module"></param>
    public override void ControllerStart(UIModuleBase module)
    {
        base.ControllerStart(module);
        //获取按钮父物体
        buttonParent = _module.FindCurrentModuleWidget("GameButton#").Transform;
        titleText = _module.FindCurrentModuleWidget("GameTitle#");
    }
    //初始化文本数组
    //标题文字数组
    private string[] titleTexts = new string[13] { "R", "e", "s", "o", "u", "r", "c", "e", "s", " ", "W", "a", "r" };

    /// <summary>
    /// 显示标题
    /// </summary>
    public void ShowTitle(int index)
    {
        //找到字体文本 0-12
        if (index < titleTexts.Length)
        {
            titleText.Text.text += titleTexts[index];
        }
        else
        {
            CreateButton();
        }
        
    }
    /// <summary>
    /// 生成按钮
    /// </summary>
    private void CreateButton()
    {
        if (hasStart == true)
        {
            index = 0;
            for (int i = index; i < 4; i++)
            {
                //获取元件预设体路径
                string path = UIConfigurationManager.GetInstance().GetWidgetAssetPathByName("Btn0"+i+"#");
                //通过对象池生成预设体
                ObjectPool.GetInstance().SpawnObject(path, new object[] { buttonParent, _module });
            }
        }
        else
        {
            index = 1;
            for (int i = index; i < 4; i++)
            {
                //获取元件预设体路径
                string path = UIConfigurationManager.GetInstance().GetWidgetAssetPathByName("Btn0" + i + "#");
                //通过对象池生成预设体
                ObjectPool.GetInstance().SpawnObject(path, new object[] { buttonParent, _module });
            }
        }
    }
    private void AddBtnEvent()
    {
        //添加按钮事件
        _module.FindCurrentModuleWidget("UI/Btn01#").Button.onClick.AddListener(
            ()=> {
                Facace.instance.LoadScene("MainScene");
            });
    }
}
