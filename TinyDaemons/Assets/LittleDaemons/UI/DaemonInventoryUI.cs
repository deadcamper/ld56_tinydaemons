using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DaemonInventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public DaemonItemButton templateButton;
    private DaemonGame game;

    private List<DaemonItemButton> ActiveButtons = new List<DaemonItemButton>();

    private int lastInventoryCount = -1;

    // Start is called before the first frame update
    void Start()
    {
        game = DaemonGame.GetInstance();

        templateButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        inventoryUI.SetActive(game.selectedListForInventory != null);

        if (game.selectedListForInventory == null)
        {
            // short-cut the inventory setup
            return;
        }

        if (lastInventoryCount != game.inventory.items.Count)
        {
            int newCount = game.inventory.items.Count;
            while (ActiveButtons.Count > newCount)
            {
                Destroy(ActiveButtons.Last().gameObject);
                ActiveButtons.RemoveAt(ActiveButtons.Count-1);
            }

            while (ActiveButtons.Count < newCount)
            {
                DaemonItemButton newButton = Instantiate(templateButton, templateButton.transform.parent);
                newButton.gameObject.SetActive(true);

                ActiveButtons.Add(newButton);
            }

            for(int idx = 0; idx < newCount; idx++)
            {
                ActiveButtons[idx].SetUp(game.inventory.items[idx]);
            }

            lastInventoryCount = newCount;
        }
    }
}
