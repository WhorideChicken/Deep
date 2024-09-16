using UnityEngine;

public class LaserButton : InteractionObject
{
    [SerializeField] private LaserLine _laserLine;

    public override void Interaction()
    {
        base.Interaction();
        _laserLine.StopLaserSystem();
        this.enabled = false;
    }
}
