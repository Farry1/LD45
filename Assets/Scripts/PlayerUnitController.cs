using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerUnitController : MonoBehaviour
{
    [HideInInspector] public List<Unit> units = new List<Unit>();
    [HideInInspector] public Unit selectedPlayerUnit = null;

    private static PlayerUnitController _instance;
    public static PlayerUnitController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Unit[] unitsArray = FindObjectsOfType<Unit>();
        foreach (Unit u in unitsArray)
        {
            units.Add(u);
        }

        StageUIController.Instance.CreateUnitPanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedPlayerUnit != null && 
            selectedPlayerUnit.currentPath != null &&
            selectedPlayerUnit.actionState != Unit.ActionState.Moving)
        {

            int currentNode = 0;

            while (currentNode < selectedPlayerUnit.currentPath.Count - 1)
            {
                Vector3 start = selectedPlayerUnit.currentPath[currentNode].transform.position;
                Vector3 end = selectedPlayerUnit.currentPath[currentNode + 1].transform.position;
                Debug.DrawLine(start, end, Color.cyan);
                currentNode++;
            }

        }
    }


    public void UnselectSelectedUnits()
    {
        StageUIController.Instance.SetPlayerActionContainer(false);

        StageUIController.Instance.ClearPlayerActions();

        Unit[] selectedUnits = units.Where(u => u.unitState == Unit.UnitState.Selected).ToArray();
        foreach (Unit unit in selectedUnits)
        {
            unit.unitState = Unit.UnitState.Idle;
        }

        selectedPlayerUnit = null;
        TileMap.Instance.Clear();
    }

    public Unit GetSelectedUnit()
    {
        Unit[] selectedUnits = units.Where(u => u.unitState == Unit.UnitState.Selected).ToArray();
        return selectedUnits[0];
    }

    public void SelectUnit(Unit unit)
    {
        UnselectSelectedUnits();
        StageUIController.Instance.SetPlayerActionContainer(true);
        selectedPlayerUnit = unit;
        unit.unitState = Unit.UnitState.Selected;
    }

    public void EnableMoveAction()
    {
        if (selectedPlayerUnit != null)
        {
            selectedPlayerUnit.moveActionAvailable = true;
            StageUIController.Instance.playerMoveButton.interactable = false;
        }
    }
}
