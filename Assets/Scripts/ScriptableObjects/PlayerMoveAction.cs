using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Actions/Movement")]
public class PlayerMoveAction : Action
{
    Unit unit
    {
        get
        {
            return PlayerUnitController.Instance.selectedPlayerUnit;
        }
    }


    public override void Execute()
    {
        unit.actionState = Unit.ActionState.MoveSelection;        
    }
}
