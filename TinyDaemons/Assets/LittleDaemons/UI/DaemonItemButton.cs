using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DaemonItemButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI title;
    public Image icon;

    public DaemonItem item;

    public void Start()
    {
        button.onClick.AddListener(ApplyItem);
    }

    public void SetUp(DaemonItem item)
    {
        this.item = item;

        title.text = item.GetText();
        icon.sprite = item.GetSprite();
    }

    public void ApplyItem()
    {
        DaemonGame game = DaemonGame.GetInstance();

        if (!game.inventory.items.Remove(item))
        {
            Debug.LogWarning("Tried to ApplyItem for an item that's not in the inventory.");
            return;
        }

        item.ApplyItem();

        game.selectedListForInventory = null;
    }
}
