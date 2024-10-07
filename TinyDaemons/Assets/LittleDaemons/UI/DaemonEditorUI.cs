using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;

public class DaemonEditorUI : MonoBehaviour
{
    public TextMeshProUGUI namePlate;
    public TextMeshProUGUI healthPlate;
    public TextMeshProUGUI targetPlate;

    public GameObject killSwitch;

    public GameObject listOfListsUI;
    public DaemonActionListUI templateListUI;

    private DaemonGame game;
    private Daemon lastDaemon;

    private List<DaemonActionListUI> activeListUIs = new List<DaemonActionListUI>();

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

        if (!lastDaemon && !object.ReferenceEquals(lastDaemon, null))
        {
            lastDaemon = null;
            Populate(null);
        }

        UpdateForChanges();
    }

    private void Populate(Daemon nextDaemon)
    {
        if (nextDaemon)
        {
            List<DaemonActionList> listOfListData = nextDaemon.AllActions
                .Where(act => act.isVisible)
                .Where(act => act.isModifiable) // TODO - sort, not hide
                .ToList();

            while (activeListUIs.Count > listOfListData.Count)
            {
                Destroy(activeListUIs.Last());
                activeListUIs.RemoveAt(activeListUIs.Count - 1);
            }

            while (activeListUIs.Count < listOfListData.Count)
            {
                var listUI = Instantiate(templateListUI, templateListUI.transform.parent);
                listUI.gameObject.SetActive(true);
                activeListUIs.Add(listUI);
            }

            for(int n = 0; n < activeListUIs.Count; n++)
            {
                activeListUIs[n].Populate(listOfListData[n]);
            }
        }
        else
        {
            // Add three 'blanks'
            while (activeListUIs.Count < 3)
            {
                var listUI = Instantiate(templateListUI, templateListUI.transform.parent);
                listUI.gameObject.SetActive(true);
                activeListUIs.Add(listUI);
            }
        }
        
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

        // Regularly probed because of accessibility
        listOfListsUI.gameObject.SetActive(lastDaemon != null);
        namePlate.text = lastDaemon ? lastDaemon.Name : "Select a Daemon";
        healthPlate.text = lastDaemon ? $"Health: {lastDaemon.Health}/{lastDaemon.TotalHealth}" : "";
    }
}
