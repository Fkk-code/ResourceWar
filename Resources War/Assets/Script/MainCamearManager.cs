using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamearManager : MonoBehaviour
{
    //跟随目标
    public Transform Target;
    //距离
    private float distance = 5;
    //跟随距离范围
    private float maxDistance = 15;
    private float minDistance = 5;
    //横向角度
    private float rot = 0;
    //纵向角度  30d度
    private float roll = 30f * Mathf.PI * 2 / 360;
    //横向旋转速度
    private float rotSpeed = 0.1f;
    //纵向旋转速度
    private float rollSpeed = 0.1f;
    //纵向旋转角度范围
    //private float maxRoll = 90f * Mathf.PI * 2 / 360;
    private float maxRoll = 10;
    //private float minRoll = 20f * Mathf.PI * 2 / 360;
    private float minRoll = 0.1f;


    void LateUpdate()
    {
        //距离控制
        distanceController();
        //横向旋转
        HorizontalRotation();
        //纵向旋转
        VerticalRotation();
        //设置位置
        SetPosition();
    }

    /// <summary>
    /// 距离控制
    /// </summary>
    private void distanceController()
    {
        float ms = Input.GetAxis("Mouse ScrollWheel") * 2f;
        distance -= ms;
        distance = distance > maxDistance ? maxDistance : distance < minDistance ? minDistance : distance;
    }
    /// <summary>
    /// 横向旋转
    /// </summary>
    private void HorizontalRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float w = Input.GetAxis("Mouse X") * rotSpeed;
            rot -= w;
        }
    }

    /// <summary>
    /// 纵向旋转
    /// </summary>
    private void VerticalRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float w = Input.GetAxis("Mouse Y") * rollSpeed;
            roll -= w;
            roll = roll > maxRoll ? maxRoll : roll < minRoll ? minRoll : roll;
        }
    }

    /// <summary>
    /// 设置摄像机的位置
    /// </summary>
    private void SetPosition()
    {
        //目标的坐标
        Vector3 targetPos = Target.transform.position;
        //用三角函数计算相机的位置
        Vector3 cameraPos;
        float d = distance * Mathf.Cos(roll);
        float height = distance * Mathf.Sin(roll);
        cameraPos.x = targetPos.x + d * Mathf.Cos(rot);
        cameraPos.z = targetPos.z + d * Mathf.Sin(rot);
        cameraPos.y = targetPos.y + height;
        Camera.main.transform.position = cameraPos;
        Camera.main.transform.LookAt(Target.transform);
    }
}

