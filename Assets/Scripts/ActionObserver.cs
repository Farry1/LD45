using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObserver : MonoBehaviour
{
    [SerializeField]
    public Dictionary<string, bool> runningActions = new Dictionary<string, bool>();
}
