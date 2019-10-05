using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : ScriptableObject
{
    public string actionName = "Action";
    public string observerKey = "Action";
    public int actionPointsNeeded = 1;

    public bool actionIsRunning = false;

    public virtual void Execute()
    {
        Debug.Log("Base Action performed");
    }
}
