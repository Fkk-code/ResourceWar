using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrame;
public class BattlePanelModule : UIModuleBase
{
    BattlePanelCotroller battlePanelController;
    BattleManager.ShowChessState BMSC;
    BattleManager.ShowChessState _BMSC;
    void Start()
    {
        if (battlePanelController == null)
        {
            battlePanelController = new BattlePanelCotroller();
            //绑定控制器
            BindController(battlePanelController);
        }
         _BMSC = BattleManager.instance.showChessState;
        Charge();
    }

    void Update()
    {
        BMSC = BattleManager.instance.showChessState;
        if (BMSC != _BMSC)
        {
            _BMSC = BMSC;
            Charge();
        }
    }
    public void Charge()
    {
        switch (_BMSC)
        {
            case BattleManager.ShowChessState.普通:
                battlePanelController.ChargeView(3);
                break;
            case BattleManager.ShowChessState.选择行动:
                battlePanelController.ChargeView(2);
                break;
            case BattleManager.ShowChessState.敌方回合:
                battlePanelController.ChargeView(1);
                break;
        }
    }
}
