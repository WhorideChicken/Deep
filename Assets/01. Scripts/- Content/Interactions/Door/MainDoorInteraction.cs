using Polyperfect.Common;
using UnityEngine;

public class MainDoorInteraction : InteractionObject
{
    public Dialog EndingX;
    
    public override void Interaction()
    {
        base.Interaction();
        Confirm();
    }

    private async void Confirm()
    {
        await CanvasManager.Instance.FadeOut();
        CanvasManager.Instance.StartDialog(EndingX, GameManager.Instance.GameModeChange);
    }

    private void Cancel()
    {
        
    }
}
