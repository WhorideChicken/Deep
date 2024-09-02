using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    protected bool IsInteractable = true;
    public bool IsHolding = false;
    public virtual void Interaction()
    {

    }

    public virtual void HoldingInteraction(float value = 0.0f)
    {
    }
}
