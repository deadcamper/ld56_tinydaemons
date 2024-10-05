using System.Collections.Generic;
using UnityEngine;

public class SelectionCircle : MonoBehaviour
{
    public Material hover;
    public Material selected;
    public MeshRenderer meshRenderer;

    public enum SelectionState
    {
        Hovered,
        Selected
    }

    private HashSet<SelectionState> stateStack = new HashSet<SelectionState>();

    private void Start()
    {
        UpdateBasedOnState();
    }

    public void Select()
    {
        stateStack.Add(SelectionState.Selected);
        UpdateBasedOnState();
    }
    
    public void DeSelect()
    {
        stateStack.Remove(SelectionState.Selected);
        UpdateBasedOnState();
    }

    public void Hover()
    {
        stateStack.Add(SelectionState.Hovered);
        UpdateBasedOnState();
    }

    public void Unhover()
    {
        stateStack.Remove(SelectionState.Hovered);
        UpdateBasedOnState();
    }
    private void UpdateBasedOnState()
    {
        if (stateStack.Count == 0)
        {
            meshRenderer.enabled = false;
            return;
        }

        meshRenderer.enabled = true;
        if (stateStack.Contains(SelectionState.Selected))
        {
            meshRenderer.material = selected;
        }
        else if (stateStack.Contains(SelectionState.Hovered))
        {
            meshRenderer.material = hover;
        }
    }
}
