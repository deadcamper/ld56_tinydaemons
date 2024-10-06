using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DaemonActionListUI : MonoBehaviour
{
    public Image titleBackplate;
    public TextMeshProUGUI titleField;
    public DaemonActionUI templateAction;

    public Color performingColor = Color.yellow;
    public Color notPerformingColor = Color.gray;

    private DaemonActionList list;
    private List<DaemonActionUI> actionUIs = new List<DaemonActionUI>();

    // Start is called before the first frame update
    void Start()
    {
        templateAction.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (list == null)
            return;

        titleBackplate.color = list.IsPerforming() ? performingColor : notPerformingColor;

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
}
