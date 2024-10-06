using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DaemonEditorUI : MonoBehaviour
{
    public TextMeshProUGUI namePlate;
    public TextMeshProUGUI healthPlate;
    public TextMeshProUGUI targetPlate;

    public GameObject killSwitch;

    public DaemonActionListUI templateListUI;

    private DaemonGame game;
    private Daemon lastDaemon;

    private List<DaemonActionListUI> activeListUIs = new List<DaemonActionListUI>();

    private const int NUM_LISTS = 4;

    private void Start()
    {
        templateListUI.gameObject.SetActive(false);
        game = FindObjectOfType<DaemonGame>();

        Populate(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (game.selectedDaemon != lastDaemon)
        {
            Daemon nextDaemon = game.selectedDaemon;

            Populate(nextDaemon);

            lastDaemon = nextDaemon;
        }

        UpdateForChanges();
    }

    private void Populate(Daemon nextDaemon)
    {
        if (nextDaemon && activeListUIs.Count == 0)
        {
            for(int i = 0; i < NUM_LISTS; i++)
            {
                var listUI = Instantiate(templateListUI, templateListUI.transform.parent);
                listUI.gameObject.SetActive(true);
                activeListUIs.Add(listUI);
            }
        }

        if (nextDaemon == null)
        {
            activeListUIs.ForEach(listUI => listUI.Clean());
        }
        else
        {
            activeListUIs[0].Populate(nextDaemon.OnIdle);
            activeListUIs[1].Populate(nextDaemon.OnHuntingEnemy);
            activeListUIs[2].Populate(nextDaemon.OnAttack);
            activeListUIs[3].Populate(nextDaemon.OnHurt);
        }

        namePlate.text = nextDaemon ? nextDaemon.Name : "Select a Daemon";
        healthPlate.text = nextDaemon ? $"Health: {nextDaemon.Health}/{nextDaemon.TotalHealth}" : "";
    }

    private void UpdateForChanges()
    {
        if (lastDaemon != null)
        {
            healthPlate.text = $"Health: {lastDaemon.Health}/{lastDaemon.TotalHealth}";
        }
        killSwitch.gameObject.SetActive(lastDaemon?.Health > 0);

        targetPlate.text = "";
        if (lastDaemon && lastDaemon.enemy)
        {
            targetPlate.text = $"Target: {lastDaemon.enemy.Name}";
        }
    }
}
