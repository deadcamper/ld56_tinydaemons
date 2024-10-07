using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DaemonItemCostButton : MonoBehaviour
{
    public Color selectedColor;
    public Color deselectedColor;

    public Button button;
    public TextMeshProUGUI cost;

    public TextMeshProUGUI title;
    public Image icon;

    public DaemonItemCost itemCost;

    private DaemonGame game;

    public void Start()
    {
        game = DaemonGame.GetInstance();
        button.onClick.AddListener(SelectItem);
    }

    public void Update()
    {
        if (game.selectedItemCost == itemCost)
        {
            button.image.color = selectedColor;
            game.myLearning.HasClickedActionFromStore = true;
            CheckForApplyItem();
        }
        else
        {
            button.image.color = deselectedColor;
        }

        button.interactable = itemCost.cost <= game.gibletPoints;
    }

    public void Populate(DaemonItemCost itemCost)
    {
        this.itemCost = itemCost;

        title.text = itemCost.action.TextName;
        icon.sprite = null; // Never made a sprite...

        cost.text = itemCost.cost.ToString();

        icon.enabled = ( icon.sprite != null );
    }

    private void CheckForApplyItem()
    {
        if (game.selectedItemCost == itemCost)
        {
            if (game.selectedListForInventory != null)
            {
                itemCost.BuyItem();

                game.selectedListForInventory = null;
                game.selectedItemCost = null;
                game.myLearning.HasAddedAction = true;
            }
        }
    }

    private void SelectItem()
    {
        game.selectedItemCost = (game.selectedItemCost == itemCost) ? null : itemCost;
    }

}
