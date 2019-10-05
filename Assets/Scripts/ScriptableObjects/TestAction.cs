using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Actions/TestAction")]
public class TestAction : Action
{
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Test Action performed");
    }
}
