using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageUIController : MonoBehaviour
{

    [Header("UI Prefabs")]
    public GameObject actionButtonPrefab;
    public GameObject unitPanelPrefab;


    [Header("UI Container")]
    public GameObject playerActionsContainer;
    public GameObject playerInfoContainer;

    [Header("UI Buttons")]
    public Button playerMoveButton;

    [Header("Ingame Menu")]
    public GameObject InGameMenuContainer;


    private static StageUIController _instance;
    public static StageUIController Instance { get { return _instance; } }

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
        playerMoveButton.interactable = true;

    }

    // Update is called once per frame
    void Update()
    {
        InGameMenu();
    }

    public void SetPlayerActionContainer(bool state)
    {
        playerActionsContainer.SetActive(state);
    }

    public void CreateUnitPanel()
    {
        Debug.Log("Creating Player Info Panel");
        foreach (Unit unit in PlayerUnitController.Instance.units)
        {
            GameObject unitPanel = GameObject.Instantiate(unitPanelPrefab, playerInfoContainer.transform);
            unit.relatedUIPanel = unitPanel;

            //actionButton.GetComponentInChildren<Text>().text = action.actionName;
            //actionButton.GetComponentInChildren<Button>().onClick.AddListener(action.Execute);
        }
    }

    public void ClearPlayerActions()
    {
        Button[] buttons = playerActionsContainer.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            Destroy(b.gameObject);
        }
    }

    public void CreateActionMenu(List<Action> availableActions)
    {
        Debug.Log("Creating Actions Menu");
        foreach (Action action in availableActions)
        {
            GameObject actionButton = GameObject.Instantiate(actionButtonPrefab, playerActionsContainer.transform);
            actionButton.GetComponentInChildren<Text>().text = action.actionName;
            actionButton.GetComponentInChildren<Button>().onClick.AddListener(action.Execute);
        }
    }

    bool toggleIngameMenu = false;
    void InGameMenu()
    {
        InGameMenuContainer.SetActive(toggleIngameMenu);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleIngameMenu = !toggleIngameMenu;
        }
    }
}
