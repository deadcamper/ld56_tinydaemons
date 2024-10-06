using System.Collections.Generic;
using UnityEngine;

public class Giblet : MonoBehaviour
{
    private MeshRenderer[] renderers;

    private Dictionary<MeshRenderer, Material> origMaterials = new Dictionary<MeshRenderer, Material>();

    public Material pickupColor;
    public void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer ms in renderers)
        {
            origMaterials[ms] = ms.material;
        }

        DaemonGame game = DaemonGame.GetInstance();

        pickupColor = game.defaultHover;
    }

    private void OnMouseEnter()
    {
        foreach(MeshRenderer ms in renderers)
        {
            ms.material = pickupColor;
        }
    }

    private void OnMouseExit()
    {
        foreach (MeshRenderer ms in renderers)
        {
            ms.material = origMaterials[ms]; // swap it back
        }
    }

    private void OnMouseUpAsButton()
    {
        DaemonGame game = DaemonGame.GetInstance();

        game.OnPickupGib();

        Destroy(gameObject);
    }
}
