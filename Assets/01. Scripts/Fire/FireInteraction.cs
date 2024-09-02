using UnityEngine;

public class FireInteraction : InteractionObject
{
    public Dialog fireDialog;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Interaction()
    {
        base.Interaction();
        CanvasManager.instance.ScreenStartDialog(fireDialog);
    }

}
