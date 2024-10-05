using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaemonActionListUI : MonoBehaviour
{
    public DaemonActionUI templateAction;

    private List<DaemonActionUI> activeActions = new List<DaemonActionUI>();

    // Start is called before the first frame update
    void Start()
    {
        templateAction.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Clean()
    {
        activeActions.ForEach(act => Destroy(act.gameObject));
        activeActions.Clear();
    }

    public void Populate(List<DaemonAction> actions)
    {
        Clean();

        foreach (DaemonAction act in actions)
        {
            DaemonActionUI newAct = GameObject.Instantiate(templateAction, templateAction.transform.parent);
            newAct.gameObject.SetActive(true);
            newAct.SetUp(act);
        }
    }
}
