using UnityEngine;

public class FireInteraction : InteractionObject
{
    [SerializeField] private Dialog _fireDialog;

    public override void Interaction()
    {
        base.Interaction();
        CanvasManager.Instance.StartDialog(_fireDialog);
    }
}
