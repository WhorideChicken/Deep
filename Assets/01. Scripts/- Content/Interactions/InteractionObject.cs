using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [SerializeField] protected bool _isInteractable = true;
    public bool IsHolding { get; protected set; } = false;

    public virtual void Interaction()
    {
    }

    public virtual void HoldingInteraction(float value = 0.0f)
    {
    }
}
