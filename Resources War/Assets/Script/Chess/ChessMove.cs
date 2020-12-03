using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ChessState))]
public class ChessMove : MonoBehaviour
{
    //棋子当前所属格子
    [System.NonSerialized]
    public Vector2Int startPos;
    //移动速度
    [Range(0.2f, 0.5f)]
    public float moveSpeed = 0.4f;
    //棋子管理器
    private ChessManager _chessManager;
    //行走状态
    private bool IsMoving = false;
    //路线
    private Stack<Vector3> path;
    void Start()
    {
        //初始化数组
        path = new Stack<Vector3>();
        //获取棋盘组件
        _chessManager = GetComponent<ChessManager>();
        //设置当前所属格子
        startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
    }
    void Update()
    {
        //点击地板移动
        if (gameObject.scene.name == "MainScene" && Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            //点击格子
            MouseClickBoard();
        }
    }
    private bool IsPointerOverUIObject()
    {//判断是否点击的是UI，有效应对安卓没有反应的情况，true为UI
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        try
        {
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return results.Count > 0;
    }
    /// <summary>
    /// 棋子移动到当前位置
    /// </summary>
    /// <param name="end"></param>
    public void MoveToArrived(Vector2Int end)
    {
        //清空路线
        path.Clear();
        //获取路径
        BoardManager.instance.AStar(
            startPos, //起点
            end, //终点
            ref path,//接受路径
            _chessManager._chessState.team//队伍
            );
        //设置棋盘字典
        BoardManager.instance.UpdateBoardDic(_chessManager, startPos, end);
        //启动移动协成
        if (!IsMoving)
            StartCoroutine(BoardMoveToEnd());
    }
    /// <summary>
    /// 鼠标射线检测
    /// </summary>
    private void MouseClickBoard()
    {
        //射线返回值
        RaycastHit raycastHit;
        //屏幕射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            //点击到棋盘格子
            if (raycastHit.transform.CompareTag("BoardItem"))
            {
                //生成2D终点
                Vector2Int end = new Vector2Int(Mathf.RoundToInt(raycastHit.transform.position.x), Mathf.RoundToInt(raycastHit.transform.position.z));
                //移动到终点
                MoveToArrived(end);
            }
        }
    }
    /// <summary>
    /// 棋子移动协成
    /// </summary>
    /// <returns></returns>
    IEnumerator BoardMoveToEnd()
    {
        //进入移动状态
        IsMoving = true;
        //开启动画
        _chessManager._chessAttack.PlayAnimation("Speed", moveSpeed);
        //遍历所有路径点
        while (path.Count > 0)
        {
            //出栈
            Vector3 current = path.Pop();
            //制作新的起始点
            startPos.x = Mathf.RoundToInt(current.x);
            startPos.y = Mathf.RoundToInt(current.z);
            //获取目标坐标
            Vector3 endpoint = new Vector3(current.x, current.y + 1f, current.z);
            //转向
            PlayerRoll(new Vector2Int((int)endpoint.x, (int)endpoint.z));
            //transform.LookAt(endpoint);
            //高度判断
            float height = endpoint.y - transform.position.y;
            //跳动
            if (height > 0.4f)
            {
                //动画
                _chessManager._chessAttack.PlayAnimation("Speed", 0.2f);
                //动画
                _chessManager._chessAttack.PlayAnimation(_chessManager._chessAttack.JUMP);
                //跳跃
                _chessManager._rigidbody.velocity = Vector3.up * height * 5f;
                //行走
                while (Vector3.Angle(endpoint - transform.position, transform.forward) < 90)
                {
                    transform.position += transform.forward * 0.01f;
                    yield return 0;
                }
            }
            else
            {
                //动画
                _chessManager._chessAttack.PlayAnimation("Speed", moveSpeed);
                //行走
                while (Vector3.Angle(endpoint - transform.position, transform.forward) < 90)
                {
                    //获取移动速度
                    transform.position += transform.forward * moveSpeed / 10f;
                    yield return 0;
                }
            }
        }
        //关闭动画
        _chessManager._chessAttack.PlayAnimation("Speed", 0f);
        //退出移动状态
        IsMoving = false;
    }
    /// <summary>
    /// AI移动
    /// </summary>
    /// <param name="maxlenth"></param>
    /// <param name="newpath"></param>
    public void AIMove(int maxlenth, Queue<Vector3> newpath)
    {
        //启动移动协成
        if (!IsMoving)
            StartCoroutine(AIMoveToEnd(maxlenth, newpath));
    }
    /// <summary>
    /// AI移动协成
    /// </summary>
    /// <param name="maxlenth"></param>
    /// <param name="newpath"></param>
    /// <returns></returns>
    IEnumerator AIMoveToEnd(int maxlenth, Queue<Vector3> newpath)
    {
        //进入移动状态
        IsMoving = true;
        //开启动画
        _chessManager._chessAttack.PlayAnimation("Speed", moveSpeed);
        //保存起始点
        Vector2Int startpoint = startPos;
        //遍历所有路径点
        while (newpath.Count > 0 && maxlenth-- > 0)
        {
            //出栈
            Vector3 current = newpath.Dequeue();
            //制作新的起始点
            startPos.x = Mathf.RoundToInt(current.x);
            startPos.y = Mathf.RoundToInt(current.z);
            //获取目标坐标
            Vector3 endpoint = new Vector3(current.x, current.y + 1f, current.z);
            //转向
            PlayerRoll(new Vector2Int((int)endpoint.x, (int)endpoint.z));
            //高度判断
            float height = endpoint.y - transform.position.y;
            //跳动
            if (height > 0.4f)
            {
                //动画
                _chessManager._chessAttack.PlayAnimation("Speed", 0.2f);
                //动画
                _chessManager._chessAttack.PlayAnimation(_chessManager._chessAttack.JUMP);
                //跳跃
                _chessManager._rigidbody.velocity = Vector3.up * height * 5f;
                //行走
                while (Vector3.Angle(endpoint - transform.position, transform.forward) < 90)
                {
                    transform.position += transform.forward * 0.01f;
                    yield return 0;
                }
            }
            else
            {
                //动画
                _chessManager._chessAttack.PlayAnimation("Speed", moveSpeed);
                //行走
                while (Vector3.Angle(endpoint - transform.position, transform.forward) < 90)
                {
                    //获取移动速度
                    transform.position += transform.forward * moveSpeed / 10f;
                    yield return 0;
                }
            }
        }
        //设置棋盘字典
        BoardManager.instance.UpdateBoardDic(_chessManager, startpoint, startPos);
        //关闭动画
        _chessManager._chessAttack.PlayAnimation("Speed", 0f);
        //暂缓0.5秒
        GetComponent<AIControl>().WaitTime(0.5f);
        //退出移动状态
        IsMoving = false;
    }
    /// <summary>
    /// 玩家转向
    /// </summary>
    /// <param name="current"></param>
    public void PlayerRoll(Vector3 current)
    {
        int CurrentX = Mathf.RoundToInt(transform.position.x);
        int CurrentY = Mathf.RoundToInt(transform.position.z);
        CurrentX -= Mathf.RoundToInt(current.x);
        CurrentY -= Mathf.RoundToInt(current.z);
        if (CurrentX != 0)
            transform.eulerAngles = Vector3.up * (CurrentX > 0 ? -90 : 90);
        if (CurrentY != 0)
            transform.eulerAngles = Vector3.up * (CurrentY > 0 ? 180 : 0);

    }
    /// <summary>
    /// 玩家转向
    /// </summary>
    /// <param name="current"></param>
    public void PlayerRoll(Vector2Int current)
    {
        int CurrentX = Mathf.RoundToInt(transform.position.x);
        int CurrentY = Mathf.RoundToInt(transform.position.z);
        CurrentX -= Mathf.RoundToInt(current.x);
        CurrentY -= Mathf.RoundToInt(current.y);
        if (CurrentX != 0)
            transform.eulerAngles = Vector3.up * (CurrentX > 0 ? -90 : 90);
        if (CurrentY != 0)
            transform.eulerAngles = Vector3.up * (CurrentY > 0 ? 180 : 0);
    }
}

