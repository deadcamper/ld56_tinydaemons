using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaemonActionListUI : MonoBehaviour
{
    public DaemonActionUI templateAction;

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
        if (list == null || actionUIs.Count == 0)
            return;

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

    public void Populate(DaemonActionList actionlist)
    {
        if (list == actionlist)
            return;

        Clean();

        list = actionlist;

        foreach (DaemonAction act in actionlist.actions)
        {
            DaemonActionUI newAct = GameObject.Instantiate(templateAction, templateAction.transform.parent);
            newAct.gameObject.SetActive(true);
            newAct.SetUp(act);

            actionUIs.Add(newAct);
        }
    }
}
