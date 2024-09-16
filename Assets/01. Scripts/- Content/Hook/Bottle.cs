using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Bottle : MonoBehaviour
{
    [SerializeField] private LayerMask _bottleLayer;
    [SerializeField] private float _torqueMagnitude = 1f;
    [SerializeField] private float _throwForce = 10f;

    private Rigidbody _rb;
    private Vector3 _originPos;
    private bool _isThrown = false;
    private UnityAction _endAction;
    private Coroutine _bottleTrackCor;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _originPos = transform.localPosition;
    }

    public void OnEnable()
    {
        ResetBottle();
    }

    public async void ThrowBottle(UnityAction action)
    {
        _endAction = action;
        await Task.Delay(500);

        ResetBottle();
        ApplyThrowForce();

        _isThrown = true;
    }

    private void ResetBottle()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.localPosition = _originPos;
    }
    private void ApplyThrowForce()
    {
        _rb.AddForce(Vector3.up * _throwForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-_torqueMagnitude, _torqueMagnitude),
            0,
            Random.Range(-_torqueMagnitude, _torqueMagnitude)
        );

        _rb.AddTorque(randomTorque, ForceMode.Impulse);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (_isThrown && (_bottleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            StartBottleCheck();
        }
    }

    private void StartBottleCheck()
    {
        if (_bottleTrackCor != null)
        {
            StopCoroutine(_bottleTrackCor);
        }

        _bottleTrackCor = StartCoroutine(BottleStateCheckCoroutine());
    }

    private IEnumerator BottleStateCheckCoroutine()
    {
        yield return new WaitUntil(IsObjectStopped);
        yield return new WaitForSeconds(1.0f);
        _endAction?.Invoke();
        _isThrown = false;
    }

    bool IsObjectStopped()
    {
        return _rb.linearVelocity.magnitude < 0.01f && _rb.angularVelocity.magnitude < 0.01f;
    }

    public bool IsBottleStandingUp()
    {
        float uprightThreshold = 0.9f;
        return Vector3.Dot(transform.up, Vector3.up) > uprightThreshold;
    }


}
