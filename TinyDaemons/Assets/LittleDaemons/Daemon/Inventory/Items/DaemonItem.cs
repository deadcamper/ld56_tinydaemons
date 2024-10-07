using UnityEngine;

public abstract class DaemonItem : MonoBehaviour
{
    public abstract Sprite GetSprite();

    public abstract string GetText();

    public abstract void ApplyItem();
}
