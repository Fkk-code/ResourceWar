using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Bag", menuName = "Bag/New Bag")]
public class BagData : ScriptableObject
{
    [Header("背包种类")]
    public ItemType itemType;
    [Header("道具名字")]
    public List<PropItems_Enum> propName;
    [Header("道具数量")]
    public List<int> propNum;

    //给背包赋值
    public void GetData(List<int> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            propName[i] = (PropItems_Enum)data[i];
            propNum[i] = propName[i] != PropItems_Enum.空 ? 1 : 0;
        }
    }
    public void GetData(List<int> data,int aaa)
    {
        for (int i = 0; i < data.Count; i++)
        {
            propName[i] = (PropItems_Enum)data[i];
        }
    }
}
