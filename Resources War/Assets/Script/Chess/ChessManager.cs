using UnityEngine;
public class ChessManager : MonoBehaviour,System.IComparable<ChessManager>
{
    //棋子移动组件
    [System.NonSerialized]
    public ChessMove _chessMove;
    //棋子攻击组件
    [System.NonSerialized]
    public ChessAttack _chessAttack;
    //棋子信息组件
    [System.NonSerialized]
    public ChessState _chessState;
    //棋子动画组件
    [System.NonSerialized]
    public Animator _animator;
    //棋子刚体组件
    [System.NonSerialized]
    public Rigidbody _rigidbody;
    //棋子碰撞体组件
    [System.NonSerialized]
    public CapsuleCollider _capsuleCollider;
    void Awake()
    {
        //获取当前场景名称
        string sceneName = gameObject.scene.name;
        //组件设置
        switch (sceneName)
        {
            case "BattleScene":
                //战斗场景
                InitComponentForBattle();
                break;
            case "MainScene":
                //主场景
                InitComponentForMain();
                break;
        }
        //修改标签
        gameObject.tag = "Chess";
    }
    /// <summary>
    /// 战斗场景棋子设置
    /// </summary>
    private void InitComponentForBattle()
    {
        //基础组件
        _animator = GetComponent<Animator>();
        //功能组件
        _chessState = GetComponent<ChessState>() ?? gameObject.AddComponent<ChessState>();
        _chessMove = GetComponent<ChessMove>() ?? gameObject.AddComponent<ChessMove>();
        _chessAttack = GetComponent<ChessAttack>() ?? gameObject.AddComponent<ChessAttack>();
    }
    /// <summary>
    /// 主场景棋子设置
    /// </summary>
    private void InitComponentForMain()
    {
        //基础组件
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        //功能组件
        _chessState = GetComponent<ChessState>() ?? gameObject.AddComponent<ChessState>();
        _chessMove = GetComponent<ChessMove>() ?? gameObject.AddComponent<ChessMove>();
        _chessAttack = GetComponent<ChessAttack>() ?? gameObject.AddComponent<ChessAttack>();
        //组件初始化
        //动画组件
        _animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("PlayerBaseAnimator");
        //碰撞体
        _capsuleCollider.center = new Vector3(0, 1f, 0);
        _capsuleCollider.radius = 0.5f;
        _capsuleCollider.height = 2f;
        //刚体
        _rigidbody.constraints = ~RigidbodyConstraints.FreezePositionY;
    }
    public int CompareTo(ChessManager other)
    {
        if (_chessState.Speed < other._chessState.Speed)
            return 1;
        if (_chessState.Speed > other._chessState.Speed)
            return -1;
        return 0;
    }
}
