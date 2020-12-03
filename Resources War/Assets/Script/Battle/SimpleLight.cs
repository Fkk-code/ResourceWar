using UnityEngine;

public class SimpleLight : MonoBehaviour
{
    private Color _color;
    private Light _light;
    private string lightState;
    private BattleManager battleManager;
    void Awake()
    {
        //父类组件
        battleManager = transform.parent.GetComponent<BattleManager>();
        _light = GetComponent<Light>();
        _color = Color.blue;
    }
    public void ChangeState(string s)
    {
        lightState = s;
        switch (s)
        {
            case "移动":
                _color = Color.blue;
                break;
            case "攻击":
                _color = Color.red;
                break;
            case "技能":
                _color = new Color(1f,0.73f,1f);
                break;
        }
        _light.color = _color;
    }
    void OnMouseDown()
    {
        //Debug.Log("被点击了");
    }
    void OnMouseEnter()
    {
        _light.color = Color.yellow;
    }

    void OnMouseExit()
    {
        _light.color = _color;
    }
}
