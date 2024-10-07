using UnityEngine;
using UnityEngine.UI;

public class DaemonActionUI : MonoBehaviour
{
    public Color performingColor = Color.yellow;
    public Color notPerformingColor = Color.gray;

    public Button button;
    public TMPro.TextMeshProUGUI text;

    public Button putInInventoryButton;
    public Button putInTrashButton;

    private DaemonActionList actionList;
    private DaemonAction action;

    public void SetUp(DaemonActionList actionList, DaemonAction daemonAction)
    {
        this.actionList = actionList;
        action = daemonAction;
        text.text = daemonAction.TextName;

        putInInventoryButton.onClick.AddListener(PutInInventory);
        putInTrashButton.onClick.AddListener(PutInTrash);
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

    public void PutInInventory()
    {
        DaemonGame game = DaemonGame.GetInstance();
        if (game.inventory.AtMaxCapacity())
        {
            // Do Nothing
            return;
        }

        GameObject empty = new GameObject();
        DaemonItem_GainAction gain = empty.AddComponent<DaemonItem_GainAction>();
        gain.gainedAction = action;
        game.GainItem(gain);

        actionList.actions.Remove(action);
    }

    public void PutInTrash()
    {
        actionList.actions.Remove(action);
    }
}
