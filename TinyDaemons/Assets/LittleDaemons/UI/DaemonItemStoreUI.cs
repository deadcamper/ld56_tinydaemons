using TMPro;
using UnityEngine;

public class DaemonItemStoreUI : MonoBehaviour
{
    public DaemonItemCostButton itemCostButtonTemplate;

    public TextMeshProUGUI gibletCounter;

    private DaemonGame game;

    // Start is called before the first frame update
    void Start()
    {
        itemCostButtonTemplate.gameObject.SetActive(false);
        game = DaemonGame.GetInstance();
        Setup();
    }

    private void Setup()
    {
        DaemonItemStore store = game.itemStore;

        for (int idx = 0; idx < store.itemCosts.Count; idx++)
        {
            DaemonItemCostButton newButton = Instantiate(itemCostButtonTemplate, itemCostButtonTemplate.transform.parent);
            newButton.gameObject.SetActive(true);

            newButton.Populate(store.itemCosts[idx]);
        }
    }


    // Update is called once per frame
    void Update()
    {
        gibletCounter.text = $"Giblets: Rank {game.gibletPoints}";
    }
}
