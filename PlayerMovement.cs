using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色移动脚本
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("移动速度系数")]
    public float moveSpeedScale = 10;
    [Header("转身速度")]
    public float rotateSpeed = 10;

    // 人物移动速度范围
    private float minSpeed = 0;
    private float maxSpeed = 4;

    // 玩家动画状态机
    private Animator playerAnimator;
    // 摄像机
    private Transform camera;
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        camera = GameObject.FindWithTag("MainCamera").transform;
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        // 计算移动系数
        float moveScale = Mathf.Abs(hor) > Mathf.Abs(ver) ? Mathf.Abs(hor) : Mathf.Abs(ver);
        // 播放移动动画
        float moveSpeed = Mathf.Lerp(minSpeed, maxSpeed, moveScale);
        playerAnimator.SetFloat("Speed", moveSpeed);
        // 执行位置变换
        if(hor != 0 || ver != 0) {
            // 以镜头方向为正前方，向前移动
            // 移动向量
            Vector3 dir = Vector3.ProjectOnPlane(camera.forward, Vector3.up);
            Vector3 tragetDir = dir * ver + camera.right * hor;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(tragetDir), Time.deltaTime * rotateSpeed);
            transform.position = Vector3.Lerp(transform.position, transform.position + tragetDir, Time.deltaTime * moveSpeed * moveSpeedScale);
        }
    }
}
