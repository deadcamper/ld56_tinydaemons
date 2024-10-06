using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DaemonInventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public DaemonItemButton templateButton;

    public TextMeshProUGUI inventoryTitle;

    private List<DaemonItemButton> activeButtons = new List<DaemonItemButton>();
    private DaemonGame game;

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
        if (lastInventoryCount != game.inventory.items.Count)
        {
            int newCount = game.inventory.items.Count;
            while (activeButtons.Count > newCount)
            {
                Destroy(activeButtons.Last().gameObject);
                activeButtons.RemoveAt(activeButtons.Count-1);
            }

            while (activeButtons.Count < newCount)
            {
                DaemonItemButton newButton = Instantiate(templateButton, templateButton.transform.parent);
                newButton.gameObject.SetActive(true);

                activeButtons.Add(newButton);
            }

            for(int idx = 0; idx < newCount; idx++)
            {
                activeButtons[idx].SetUp(game.inventory.items[idx]);
            }

            lastInventoryCount = newCount;

            inventoryTitle.text = $"Action Inventory ( {game.inventory.items.Count} / {game.inventory.MaxCount} )";
        }
    }
}
