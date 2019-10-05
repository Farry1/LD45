using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : MonoBehaviour
{
    public List<Node> edges = new List<Node>();

    public float movementCost = 1;
    public bool canBeEntered = true;
    public TextMesh stepCounterText;
    public Renderer walkableIndicatorSurface;


    private void Start()
    {
        stepCounterText.text = "0";
    }

    public void IndicateNavigation(int stepsRequired, int maxSteps)
    {
        stepCounterText.text = stepsRequired.ToString();

        if (stepsRequired <= maxSteps)
        {
            walkableIndicatorSurface.gameObject.SetActive(true);
            walkableIndicatorSurface.material.color = Color.green;
        }
        else
        {
            walkableIndicatorSurface.gameObject.SetActive(false);
        }
    }

    public void HideNavigationIndicator()
    {
        walkableIndicatorSurface.gameObject.SetActive(false);
    }

    public float DistanceTo(Vector3 position)
    {
        return Vector3.Distance(transform.position, position);
    }
}
