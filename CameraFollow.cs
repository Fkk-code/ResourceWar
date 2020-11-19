using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机跟随控制器
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // 跟随目标
    private Transform followTraget;
    // 摄像机旋转球体中心
    private Vector3 followCenter;
    // 球体半径
    private float radius;

    // 竖直夹角，范围为0-180°
    private float verEngle;
    // 水平夹角，范围为0-360°
    private float horEngle;
    // 变化强度
    private float intensity = 2;

    void Start()
    {
        // 找到跟随目标
        followTraget = GameObject.FindWithTag("Player").transform.Find("Camera_Follow");
        // 中心位置
        followCenter = followTraget.position + Vector3.up * 3;
        // 中心指向摄像机的向量
        Vector3 followDir = transform.position - followCenter;
        radius = followDir.magnitude;
        // 计算夹角
        verEngle = Vector3.Angle(Vector3.up, followDir);
        horEngle = Vector3.Angle(Vector3.back, Vector3.ProjectOnPlane(followDir, Vector3.up));
    }

    private void LateUpdate() {
        // 摄像机正常移动
        CameraMove();
    }

    /// <summary>
    /// 摄像头移动方法
    /// </summary>
    private void CameraMove() {
        // 锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        // 球体中心位置，会随着角色的移动变化
        followCenter = followTraget.position + Vector3.up * 3;

        // 鼠标位移每一帧的增量
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        // 计算绝对值，如果小于0.3，不予处理
        float mx = Mathf.Abs(mouseX);
        float my = Mathf.Abs(mouseY);
        // 对角度进行变化
        verEngle += my > 0.3 ? mouseY * intensity : 0;
        verEngle = Mathf.Clamp(verEngle, 10, 170);
        horEngle -= mx > 0.3 ? mouseX * intensity : 0;

        // 获取最佳位置
        Vector3 newPos = GetPosition();
        // 移动到最佳位置
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 10);

        // 摄像机一直看向玩家
        //Vector3 dir = followTraget.position - transform.position;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10);
        transform.LookAt(followTraget);
    }

    /// <summary>
    ///  获取摄像机的最佳位置
    /// </summary>
    private Vector3 GetPosition() {
        // 正常摄像机位置
        Vector3 bestPosition = GetSpherePosition(horEngle, verEngle, radius);
        if(CanHitPlayer(bestPosition)) {
            return bestPosition;
        }
        // 向右偏移90位置
        Vector3 bestRightPosition = GetSpherePosition(horEngle + 30, verEngle, radius);
        if(CanHitPlayer(bestRightPosition)) {
            horEngle += 30;
            return bestRightPosition;
        }
        // 向左偏移90位置
        Vector3 bestLeftPosition = GetSpherePosition(horEngle - 30, verEngle, radius);
        if(CanHitPlayer(bestLeftPosition)) {
            horEngle -= 30;
            return bestLeftPosition;
        }
        // 从玩家处发射射线，获取位置
        return GetPlayerHitPosition(bestPosition);
    }

    /// <summary>
    /// 计算球面位置
    /// </summary>
    /// <param name="horEngle"></param>
    /// <param name="verEngle"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    private Vector3 GetSpherePosition(float horEngle, float verEngle, float radius) {
        // 球面公式，根据两个角度和固定半径计算出在球面上的位置
        float x = radius * Mathf.Sin(Mathf.Deg2Rad * verEngle) * Mathf.Sin(Mathf.Deg2Rad * horEngle);
        float y = radius * Mathf.Cos(Mathf.Deg2Rad * verEngle);
        float z = -radius * Mathf.Sin(Mathf.Deg2Rad * verEngle) * Mathf.Cos(Mathf.Deg2Rad * horEngle);
        return new Vector3(x, y, z) + followCenter;
    }

    /// <summary>
    /// 射线检测，是否击中玩家
    /// </summary>
    private bool CanHitPlayer(Vector3 startPosition) {
        Vector3 dir = followTraget.position - startPosition;
        // 进行射线检测，如果击中玩家，返回true
        RaycastHit hit;
        if(Physics.Raycast(startPosition, dir, out hit, radius * 2)) {
            if(hit.collider.CompareTag("Player")) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 从玩家处发射射线，获取最近位置
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private Vector3 GetPlayerHitPosition(Vector3 targetPosition) {
        // 从玩家处发射射线，获取位置
        Vector3 dir = targetPosition - followTraget.position;
        // 进行射线检测，如果击中玩家，返回true
        RaycastHit hit;
        if(Physics.Raycast(followTraget.position, dir, out hit, radius * 2)) {
            return hit.point;
        } else {
            return targetPosition;
        }
    }
}
