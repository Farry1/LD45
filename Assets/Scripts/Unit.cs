﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{
    public enum UnitState { Idle, Selected, Dead };
    public UnitState unitState = UnitState.Idle;

    public List<Action> availableActions = new List<Action>();
    public int actionPoints = 2;

    public int maxSteps = 1;
    public Node currentNode = null;
    public List<Node> currentPath = null;
    public bool moveActionAvailable = false;


    float t;
    Vector3 startPosition;
    Vector3 target;
    float timeToReachTarget;

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
        // transform.position = Vector3.Lerp(startPosition, target, t);
    }

    IEnumerator MoveNextTile()
    {
        //Remove the old first node and move us to that position
        currentPath.RemoveAt(0);

        //SetMoveDestination(currentPath[0].transform.position, 0.25f);

        transform.position = currentPath[0].transform.position;
        transform.rotation = currentPath[0].transform.rotation;

        currentNode = currentPath[0];

        if (currentPath.Count == 1)
        {
            //Next thingy in path would be our ultimate goal and we're standing on it. So make the path null to end this
            currentPath = null;
            StageUIController.Instance.playerMoveButton.interactable = true;
            //if (unitState == UnitState.Selected)
            //    TileMap.Instance.Dijkstra(currentNode, maxSteps);
        }

        yield return new WaitForSeconds(1f);
    }

    public void Move()
    {
        TileMap.Instance.Clear();
        if (moveActionAvailable && currentPath != null)
        {
            moveActionAvailable = false;
            StartCoroutine(MoveCoroutine());
        }
    }

    IEnumerator MoveCoroutine()
    {
        while (currentPath != null && currentPath.Count() > 1)
        {
            yield return StartCoroutine(MoveNextTile());
        }
    }

    public void GeneratePathTo(Node target)
    {
        if (moveActionAvailable == true)
            currentPath = TileMap.Instance.GeneratePathTo(currentNode, target, maxSteps);
    }


    public void OnSelect()
    {
        if (unitState != UnitState.Selected)
        {
            PlayerUnitController.Instance.SelectUnit(this);
            unitState = UnitState.Selected;

            StageUIController.Instance.CreateActionMenu(availableActions);
            // StageUIController.Instance.playerMoveButton.interactable = !moveActionAvailable;

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