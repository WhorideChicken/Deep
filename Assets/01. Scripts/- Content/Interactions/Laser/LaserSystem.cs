using Polyperfect.Common;
using UnityEngine;

public class LaserSystem : InteractionObject
{
    [SerializeField] private Dialog _laserDoor;
    [SerializeField] private LayerMask _playerLayer;

    private bool _isLaserOn = true;
    private bool _isFirstInteraction = true;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other) && _isLaserOn)
        {
            GameManager.Instance.GameOver();
        }
    }

    public override void Interaction()
    {
        base.Interaction();

        if (_isFirstInteraction)
        {
            CanvasManager.Instance.StartDialog(_laserDoor, StartPasswordGame);
            _isFirstInteraction = false;
        }
        else
        {
            StartPasswordGame();
        }
    }

    private void StartPasswordGame()
    {
        CanvasManager.Instance.StartCanvasGame(OnSuccess);
    }

    private void OnSuccess()
    {
        _isLaserOn = false;
        gameObject.SetActive(false);
    }

    private bool IsPlayer(Collider other)
    {
        return (_playerLayer.value & (1 << other.gameObject.layer)) > 0;
    }
}
