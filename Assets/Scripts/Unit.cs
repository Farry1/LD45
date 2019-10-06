using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, ISelectable
{
    public enum UnitState { Idle, Selected, Dead };
    public UnitState unitState = UnitState.Idle;

    public enum ActionState { MoveSelection, None, Moving }
    public ActionState actionState;

    public int actionPoints = 2;
    public int maxSteps = 1;

    public Node currentNode = null;
    public List<Node> currentPath = null;
    public bool moveActionAvailable = false;

    float t;
    Vector3 startPosition;
    Vector3 target;
    float timeToReachTarget;

    public List<Action> actions = new List<Action>();

    public GameObject relatedUIPanel;

    private void Start()
    {
        startPosition = target = transform.position;

        if (currentNode == null)
        {
            currentNode = FindClosestNode();
        }

        transform.position = currentNode.transform.position;
    }

    Node FindClosestNode()
    {
        Node[] nodes = FindObjectsOfType<Node>();

        Node closestNode = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (Node n in nodes)
        {
            Vector3 diff = n.transform.position - position;
            float currDistance = diff.sqrMagnitude;
            if (currDistance < distance)
            {
                closestNode = n;
                distance = currDistance;
            }
        }
        return closestNode;
    }

    private void Update()
    {
        t += Time.deltaTime / timeToReachTarget;

        bool rightMouseUp = Input.GetMouseButtonUp(1);


        switch (unitState)
        {
            case UnitState.Idle:
                break;

            case UnitState.Selected:

                switch (actionState)
                {
                    case ActionState.None:
                        if (rightMouseUp)
                            PlayerUnitController.Instance.UnselectSelectedUnits();
                        break;


                    case ActionState.MoveSelection:
                        if (rightMouseUp)
                        {
                            actionState = ActionState.None;
                            currentPath = null;
                        }

                        break;
                }

                break;

            case UnitState.Dead:
                break;
        }

        transform.position = Vector3.Lerp(startPosition, target, t);
    }

    IEnumerator MoveNextTile()
    {
        //Remove the old first node and move us to that position
        currentPath.RemoveAt(0);

        SetMoveDestination(currentPath[0].transform.position, 0.45f);

        //transform.position = currentPath[0].transform.position;
        transform.rotation = currentPath[0].transform.rotation;

        currentNode = currentPath[0];

        if (currentPath.Count == 1)
        {
            //Next thingy in path would be our ultimate goal and we're standing on it. So make the path null to end this
            currentPath = null;
            actionState = ActionState.None;
            StageUIController.Instance.playerMoveButton.interactable = true;
            if (unitState == UnitState.Selected && actionPoints > 0)
                TileMap.Instance.Dijkstra(currentNode, maxSteps);
        }

        yield return new WaitForSeconds(0.5f);
    }


    public void Move()
    {
        TileMap.Instance.Clear();
        if (actionState != ActionState.Moving &&
            currentPath != null &&
            actionPoints > 0)
        {
            actionState = ActionState.Moving;
            StartCoroutine(MoveCoroutine());
            actionPoints--;
            relatedUIPanel.transform.Find("TurnsText").GetComponent<Text>().text = actionPoints.ToString();
        }
    }



    IEnumerator MoveCoroutine()
    {
        while (currentPath != null && currentPath.Count() > 1)
        {
            yield return StartCoroutine(MoveNextTile());
        }
    }

    public void PrecalculatePathTo(Node target)
    {
        if (unitState == UnitState.Selected &&
            actionState == ActionState.MoveSelection &&
            actionPoints > 0)
        {
            currentPath = TileMap.Instance.GeneratePathTo(currentNode, target, maxSteps);
        }
    }

    public void OnSelect()
    {
        if (unitState != UnitState.Selected)
        {
            PlayerUnitController.Instance.SelectUnit(this);
            unitState = UnitState.Selected;

            StageUIController.Instance.CreateActionMenu(actions);
            // StageUIController.Instance.playerMoveButton.interactable = !moveActionAvailable;

            if (actionPoints > 0)
                TileMap.Instance.Dijkstra(currentNode, maxSteps);
        }
    }

    public void SetMoveDestination(Vector3 destination, float time)
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        target = destination;
    }
}

