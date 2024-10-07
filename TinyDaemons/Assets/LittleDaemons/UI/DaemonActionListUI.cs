using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DaemonActionListUI : MonoBehaviour
{
    public Image titleBackplate;
    public TextMeshProUGUI titleField;
    public DaemonActionUI templateAction;

    [FormerlySerializedAs("addFromInventory")]
    public Button addAction;

    public Color shoppingColor = Color.green;
    public Color performingColor = Color.yellow;
    public Color notPerformingColor = Color.gray;

    private DaemonActionList list;
    private List<DaemonActionUI> actionUIs = new List<DaemonActionUI>();

    private DaemonGame game;

    // Start is called before the first frame update
    void Start()
    {
        templateAction.gameObject.SetActive(false);
        game = DaemonGame.GetInstance();

        addAction.onClick.AddListener(PrimeToAddFromInventory);
    }

    // Update is called once per frame
    void Update()
    {
        if (list == null)
            return;

        addAction.gameObject.SetActive(game.selectedItem || game.selectedItemCost != null);

        titleBackplate.color = (game.selectedListForInventory == list) ? shoppingColor :
            ( list.IsPerforming() ? performingColor : notPerformingColor);

        if (actionUIs.Count != list.actions.Count)
        {
            Populate(list, true);
        }

        for (int i = 0; i < actionUIs.Count; i++)
        {
            actionUIs[i].SetPerforming(list.GetPerformingIndex() == i);
        }
    }

    public void Clean()
    {
        list = null;
        actionUIs.ForEach(act => Destroy(act.gameObject));
        actionUIs.Clear();
    }

    public void Populate(DaemonActionList actionlist, bool force = false)
    {
        if (!force && list == actionlist)
            return;

        Clean();

        list = actionlist;

        titleField.text = list.title;

        foreach (DaemonAction act in actionlist.actions)
        {
            DaemonActionUI newAct = GameObject.Instantiate(templateAction, templateAction.transform.parent);
            newAct.gameObject.SetActive(true);
            newAct.SetUp(actionlist, act);

            actionUIs.Add(newAct);
        }
    }

    private void PrimeToAddFromInventory()
    {
        if (game.selectedListForInventory == list)
        {
            game.selectedListForInventory = null;
        }
        else
        {
            game.selectedListForInventory = list;
        }
    }
}
