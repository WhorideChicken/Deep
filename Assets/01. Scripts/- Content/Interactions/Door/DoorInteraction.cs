using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private LayerMask _playerLayer;

    private bool _isOpened = false;
    private bool _isPlayerExited = true;
    private Coroutine _doorCheckCoroutine;

    public void OpenDoor()
    {
        if (_isOpened) return;

        _rightDoor.DOLocalMoveX(-1, 0.5f);
        _leftDoor.DOLocalMoveX(1, 0.5f);
        _isOpened = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(other.gameObject.layer, _playerLayer))
        {
            _isPlayerExited = false;
            StopDoorCheck();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsInLayerMask(other.gameObject.layer, _playerLayer))
        {
            _isPlayerExited = true;
            StartDoorCheck();
        }
    }

    public void CloseDoor()
    {
        if (!_isOpened) return;

        _isOpened = false;
        _rightDoor.DOLocalMoveX(0, 0.5f);
        _leftDoor.DOLocalMoveX(0, 0.5f);
    }

    private void StartDoorCheck()
    {
        if (_doorCheckCoroutine != null)
        {
            StopCoroutine(_doorCheckCoroutine);
        }
        _doorCheckCoroutine = StartCoroutine(DoorCheckCoroutine());
    }

    private void StopDoorCheck()
    {
        if (_doorCheckCoroutine != null)
        {
            StopCoroutine(_doorCheckCoroutine);
            _doorCheckCoroutine = null;
        }
    }

    private IEnumerator DoorCheckCoroutine()
    {
        yield return new WaitUntil(() => _isPlayerExited);
        yield return new WaitForSeconds(2.0f);
        CloseDoor();
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) > 0;
    }
}
