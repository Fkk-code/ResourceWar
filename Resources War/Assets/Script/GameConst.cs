using System.Collections.Generic;
using UnityEngine;
using Utilty;
/// <summary>
/// Ӣ��Ԥ����ö��
/// </summary>
public enum HeroEnum
{
    WizardBlue,
    WizardPurple,
    WizardRed
}
/// <summary>
/// ����Ԥ����ö��
/// </summary>
public enum EnemyEnum
{
    //����
    BatGreen,
    BatPink,
    BatRed,
    //������
    CactusMagenta,
    CactusOrange,
    CactusPurple
}
/// <summary>
/// �ռ�ƷԤ����ö��
/// </summary>
public enum CollectionEnum
{
    iһ����Ʊ,
    i������,
    i������,
    i��,
    i﮿�,
    i﮿�ʯ,
    i�ƽ�,
    i��ȸ��,
    i��ͭ,
    i��ˮ��,
    i��ɫ��ʵ,
    i�̿�֮ʯ,
    i����֮ʯ,
    i��ʿ֮ʯ,
    iħ��֮ʯ
}
public  class GameConst: Singleton<GameConst>
{
    public static int OPEN_ANI_PARAMETER;
    private GameConst()
    {
        OPEN_ANI_PARAMETER = Animator.StringToHash("Open");
        PlayerchessPerfab = new List<GameObject>();
        EnemyPrefab = new List<GameObject>();
    }
    //Lua·��
    public string path = Application.dataPath + "\\Script/Hero";
    //����Ԥ����
    public GameObject PrefabCube;
    //����Ԥ����
    public List<GameObject> EnemyPrefab;
    //���Ԥ����
    public List<GameObject> PlayerchessPerfab;
    //ս����ͼ��Ϣ
    public InteractiveProp activeInfo;
    //�˺ű��
    public int Id;
    //�˺Ž��
    public int Money = 0;
    //�˺���ʯ
    public int Jewel = 0;
}
