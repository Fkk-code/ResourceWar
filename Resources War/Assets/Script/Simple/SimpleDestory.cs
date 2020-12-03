using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDestory : MonoBehaviour
{
    public float DestoryTime = 2f;
    void Start()
    {
        Destroy(gameObject, DestoryTime);
    }

}
