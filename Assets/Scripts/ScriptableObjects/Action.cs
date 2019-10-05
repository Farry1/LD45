using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : ScriptableObject
{
    public string actionName = "Action";
    public int actionPointsNeeded = 1;

    public virtual void Execute()
    {
        Debug.Log("Base Action performed");
    }
}
