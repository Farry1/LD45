using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIController : MonoBehaviour
{
    [Header("PlayerActions")]

    public GameObject actionButtonPrefab;
    public GameObject playerActionsContainer;
    public Button playerMoveButton;

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

    }

    public void SetPlayerActionContainer(bool state)
    {
        playerActionsContainer.SetActive(state);
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
        foreach (Action action in availableActions)
        {
            GameObject actionButton = GameObject.Instantiate(actionButtonPrefab, playerActionsContainer.transform);
            actionButton.GetComponentInChildren<Text>().text = action.name;
            actionButton.GetComponentInChildren<Button>().onClick.AddListener(action.Execute);
        }
    }
}
