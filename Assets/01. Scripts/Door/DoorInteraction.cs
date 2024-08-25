using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    // L to 1
    // R to -1
    public bool IsOpened = false;
    public LayerMask playerLayer;

    public Transform Rdoor;
    public Transform Ldoor;

    private bool IsExit = true;
    private Coroutine _doorCheckCor = null;
    public void OpenDoor()
    {
        if (IsOpened)
            return;

        Rdoor.DOLocalMoveX(-1, 0.5f);
        Ldoor.DOLocalMoveX(1, 0.5f);
        IsOpened = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            IsExit = true;
            DoorCheckStart();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            IsExit = false;
            DorrCheckStop();
        }
    }

    public void CloseDoor()
    {
        IsOpened = false;
        Rdoor.DOLocalMoveX(0, 0.5f);
        Ldoor.DOLocalMoveX(0, 0.5f);
    }

    private void DoorCheckStart()
    {
        if(_doorCheckCor != null)
        {
            StopCoroutine(_doorCheckCor);
            _doorCheckCor = null;
        }

        _doorCheckCor = StartCoroutine(DoorCheckCoroutine());
    }

    private void DorrCheckStop()
    {
        if (_doorCheckCor != null)
        {
            StopCoroutine(_doorCheckCor);
            _doorCheckCor = null;
        }
    }

    IEnumerator DoorCheckCoroutine()
    {
        yield return new WaitUntil(() => IsExit);
        yield return new WaitForSeconds(2.0f);
        CloseDoor();
    }
}
