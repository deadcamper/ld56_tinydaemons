using UnityEngine;
using UnityEngine.UI;

public class DaemonActionUI : MonoBehaviour
{
    public Button button;
    public TMPro.TextMeshProUGUI text;

    private DaemonAction action;

    public void SetUp(DaemonAction daemonAction)
    {
        action = daemonAction;

        text.text = daemonAction.TextName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (action != null)
        {
            if (action.IsPerforming)
            {
                button.image.color = Color.yellow;
            }
            else
            {
                button.image.color = Color.gray;
            }
        }
    }
}
