using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DaemonItemButton : MonoBehaviour
{
    public Color selectedColor;
    public Color deselectedColor;

    public Button button;
    public Button trashButton;

    public TextMeshProUGUI title;
    public Image icon;

    public DaemonItem item;

    private DaemonGame game;

    public void Start()
    {
        game = DaemonGame.GetInstance();
        button.onClick.AddListener(SelectItem);
        trashButton.onClick.AddListener(TossItemToTrash);
    }

    public void Update()
    {
        if (game.selectedItem && game.selectedItem == item)
        {
            button.image.color = selectedColor;
            CheckForApplyItem();
        }
        else
        {
            button.image.color = deselectedColor;
        }
    }

    public void SetUp(DaemonItem item)
    {
        
        this.item = item;

        title.text = item.GetText();
        icon.sprite = item.GetSprite();

        icon.enabled = ( icon.sprite != null );
    }

    private void CheckForApplyItem()
    {
        if (game.selectedItem && game.selectedItem == item)
        {
            if (game.selectedListForInventory != null)
            {
                // TODO - Commented out for desire of a store instead
                /*
                if (!game.inventory.items.Remove(item))
                {
                    Debug.LogWarning("Tried to ApplyItem for an item that's not in the inventory.");
                    return;
                }
                */

                item.ApplyItem();

                game.selectedListForInventory = null;
                game.selectedItem = null;
            }
        }
    }

    private void SelectItem()
    {
        if (game.selectedItem == item)
        {
            game.selectedItem = null;
        }
        else
        {
            game.selectedItem = item;
        }
    }

    // Unused
    private void TossItemToTrash()
    {
        game.inventory.items.Remove(item);
        Destroy(gameObject);
    }
}
