using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;
    private Rigidbody _rb;
    private Transform _bottle;
    
    private bool isThrown = false;
    private Vector3 _originPos;
    private Coroutine _bottleTrackCor = null;

    public float torqueMagnitude = 1f;
    public float throwForce = 10f;
    public float checkDelay = 1f; // 물병이 세워졌는지 체크하기 전에 대기할 시간
    public LayerMask bottleLayer;

    public UnityAction successBottleGame = null;
    public UnityAction failBottleGame = null;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        _rb = this.GetComponent<Rigidbody>();
        _bottle = this.GetComponent<Transform>();
        _originPos = this.transform.localPosition;
    }

    public void StartBottleGame(UnityAction success, UnityAction fail)
    {
        this.gameObject.SetActive(true);
        Initialize();
        ThrowBottle();

        successBottleGame = success;
        failBottleGame = fail;
    }

    public void Initialize()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        this.transform.localPosition = _originPos;
    }

    async void ThrowBottle()
    {
        await Task.Delay(500);
        _bottle.rotation = Quaternion.Euler(Vector3.zero);

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(Random.Range(-torqueMagnitude, torqueMagnitude), 0,
            Random.Range(-torqueMagnitude, torqueMagnitude));

        _rb.AddTorque(randomTorque, ForceMode.Impulse);
        isThrown = true;
    }

    bool IsBottleStandingUp()
    {
        float uprightThreshold = 0.9f;
        return Vector3.Dot(transform.up, Vector3.up) > uprightThreshold;
    }

    void CheckBottlePosition()
    {
        if (IsBottleStandingUp())
        {
            successBottleGame();
        }
        else
        {
            failBottleGame();
        }

        // 던지기 상태를 리셋
        isThrown = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(isThrown && (bottleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            BottleCheckStart();
        }
    }

    private void BottleCheckStart()
    {
        if(_bottleTrackCor != null)
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
        CheckBottlePosition();
    }

    bool IsObjectStopped()
    {
        return _rb.linearVelocity.magnitude < 0.01f && _rb.angularVelocity.magnitude < 0.01f;
    }

    public void EndBottleGame()
    {
        Initialize();
        this.transform.parent.gameObject.SetActive(false);
    }
}
