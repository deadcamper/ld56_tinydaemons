using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DaemonItemButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI title;
    public Image icon;

    public DaemonItem item;

    public void SetUp(DaemonItem item)
    {
        this.item = item;

        title.text = item.GetText();
        icon.sprite = item.GetSprite();
    }
}
