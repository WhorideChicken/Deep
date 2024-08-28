using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Bottle : MonoBehaviour
{
    public LayerMask bottleLayer;
    public float torqueMagnitude = 1f;
    public float throwForce = 10f;

    private Rigidbody _rb;
    private bool isThrown = false;

    private Coroutine _bottleTrackCor = null;
    private Vector3 _originPos;

    private UnityAction _endAction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _originPos = transform.localPosition;
    }

    public void OnEnable()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        this.transform.localPosition = _originPos;
    }

    public async void ThrowBottle(UnityAction action)
    {
        _endAction = action;
        await Task.Delay(500);

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(Random.Range(-torqueMagnitude, torqueMagnitude), 0,
            Random.Range(-torqueMagnitude, torqueMagnitude));

        _rb.AddTorque(randomTorque, ForceMode.Impulse);
        isThrown = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isThrown && (bottleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            BottleCheckStart();
        }
    }

    private void BottleCheckStart()
    {
        if (_bottleTrackCor != null)
        {
            StopCoroutine(_bottleTrackCor);
            _bottleTrackCor = null;
        }

        _bottleTrackCor = StartCoroutine(BottleStateCheckCoroutine());
    }

    IEnumerator BottleStateCheckCoroutine()
    {
        yield return new WaitUntil(() => IsObjectStopped());
        yield return new WaitForSeconds(1.0f);
        _endAction?.Invoke();
        isThrown = false;
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
