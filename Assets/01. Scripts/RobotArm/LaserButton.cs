using UnityEngine;

public class LaserButton : InteractionObject
{
    [SerializeField] LaserLine _line;
     public override void Interaction()
    {
        base.Interaction();
        _line.StopLaserSystem();
        this.GetComponent<InteractionObject>().enabled = false;

    }
}
