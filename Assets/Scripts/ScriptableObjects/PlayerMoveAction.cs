using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Actions/Movement")]
public class PlayerMoveAction : Action
{
    

    public override void Execute()
    {
        base.Execute();
        Debug.Log("Test Action performed");
    }



}
