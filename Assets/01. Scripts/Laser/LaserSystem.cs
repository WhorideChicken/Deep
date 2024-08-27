using Polyperfect.Common;
using UnityEngine;

public class LaserSystem : InteractionObject
{
    private bool IsLaserOn = true;
    public LayerMask playerLayer;
    private void OnTriggerEnter(Collider other)
    {
        if (((playerLayer.value & (1 << other.gameObject.layer)) > 0) && IsLaserOn)
        {
            GameManager.Instance.GameOver();
        }
    }

    public override void Interaction()
    {
        base.Interaction();
        OnSuccess();
    }

    private void OnSuccess()
    {
        IsLaserOn = false;
        this.gameObject.SetActive(false);
    }
}
