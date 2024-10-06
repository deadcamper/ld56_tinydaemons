using UnityEngine;
using UnityEngine.UI;

public class DaemonActionUI : MonoBehaviour
{
    public Color performingColor = Color.yellow;
    public Color notPerformingColor = Color.gray;

    public Button button;
    public TMPro.TextMeshProUGUI text;

    private DaemonAction action;

    public void SetUp(DaemonAction daemonAction)
    {
        action = daemonAction;

        text.text = daemonAction.TextName;
    }

    public void SetPerforming(bool IsPerforming)
    {
        if (IsPerforming)
        {
            button.image.color = performingColor;
        }
        else
        {
            button.image.color = notPerformingColor;
        }
    }
}
