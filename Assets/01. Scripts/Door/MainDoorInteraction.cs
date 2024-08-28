using Polyperfect.Common;
using UnityEngine;

public class MainDoorInteraction : InteractionObject
{
    public Dialog endingX;
    public override void Interaction()
    {
        base.Interaction();
        Confirm();
    }


    private void ShowSelectUI()
    {

    }

    private void Confirm()
    {
        CanvasManager.instance.ScreenFadeOut();
        CanvasManager.instance.ScreenStartDialog(endingX, GameManager.Instance.GameModeChange);
    }

    private void Cancel()
    {
        
    }
}
