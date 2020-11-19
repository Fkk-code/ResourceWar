using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    //鼠标射线碰撞物
    private RaycastHit MouseRayHit;
    //移动速度
    private float p_MoveSpeed;
    //跟随角色的摄像头
    private Camera MainCamera;
    //动画组件
    private Animator p_ain;
    //移动停止开关
    private bool isStop = false;
    //鼠标是否指向怪物
    private Transform tagetEnemy;
    void Start()
    {
        //初始速度
        p_MoveSpeed = 3f;
        //获取主摄像头
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //获取动画控制器
        p_ain = GetComponent<Animator>();
        //初始化
        tagetEnemy = transform;
        //获取移动速度
    }
    void Update()
    {
        //方向移动
        Move();


        //自由旋转
        LookAtMouse();
    }
    void OnDisable()
    {
        p_ain.SetBool(Animator.StringToHash("Walk"), false);
        p_ain.SetBool(Animator.StringToHash("Run"), false);
    }
    //方向移动
    private void Move()
    {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            
            //行动方向
            Vector3 p_forward = new Vector3(h, 0, v);

            Vector3 newPoint = Quaternion.AngleAxis(MainCamera.transform.eulerAngles.y, Vector3.up) * p_forward;

            p_ain.SetBool(Animator.StringToHash("Walk"), true);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //跑步速度
                transform.position += newPoint * p_MoveSpeed * Time.deltaTime;
                //Run（Bool）打开
                p_ain.SetBool(GameConst.PLAYER_ANIMATOR_PARA_RUN, true);
            }
            else
            {
                //走路速度
                transform.position += newPoint * p_MoveSpeed * 0.5f * Time.deltaTime;
                //Run（Bool）关闭
                p_ain.SetBool(GameConst.PLAYER_ANIMATOR_PARA_RUN, false);
            }
        }
        else
        {
            p_ain.SetBool(Animator.StringToHash("Walk"), false);
            p_ain.SetBool(Animator.StringToHash("Run"), false);
        }
    }
    //看向鼠标所在方向
    private void LookAtMouse()
    {
        //创建一条 由鼠标 通过 摄像头 发出的射线
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        //带入鼠标点击位置，通过摄像头 向3d场景发送一条射线
        if (Physics.Raycast(ray.origin, ray.direction, out MouseRayHit))
        {
            //标签判断

            //检测地板 则转向
            if (MouseRayHit.transform.CompareTag("Plane"))
            {

            }
            //看向目标点
            transform.LookAt(new Vector3(MouseRayHit.point.x, transform.position.y, MouseRayHit.point.z));
        }
    }
    //死亡
    public void PlayerDeath()
    {
        //Die（触发器）启动
        p_ain.SetTrigger(GameConst.PLAYER_ANIMATOR_PARA_DIE);
        //关闭玩家控制器脚本
        enabled = false;
    }
}
