using Polyperfect.Common;
using UnityEngine;

public class LaserSystem : InteractionObject
{
    [SerializeField] Dialog _laserDoor;
    private bool IsLaserOn = true;
    public LayerMask playerLayer;

    private bool _isFirst = true;

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
        if (_isFirst)
        {
            CanvasManager.instance.ScreenStartDialog(_laserDoor, () => { CanvasManager.instance.StartCanvasGame(OnSuccess); });
            _isFirst = false;
            return;
        }

        CanvasManager.instance.StartCanvasGame(OnSuccess);
    }

    private void OnSuccess()
    {
        IsLaserOn = false;
        this.gameObject.SetActive(false);
    }
}
