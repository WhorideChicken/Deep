using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Threading;
using static Define;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private Text _startText;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    private Vector3 _originalPosition;
    private Coroutine _shakeCoroutine = null;
    private Camera _mainCamera;
    private CancellationTokenSource _cancellationTokenSource;
    private Tween _fadeTween;

    void Start()
    {
        Initialize();
    }

    private async void Initialize()
    {
        _mainCamera = Camera.main;
        _originalPosition = _mainCamera.transform.position;

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        _fadeTween = _startText.DOFade(0.0f, 1.5f).SetLoops(-1, LoopType.Yoyo);
        CanvasManager.Instance.HideCanvas(CanvasType.TimeCanvas);
        await CanvasManager.Instance.FadeIn();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartShake();
        }
    }

    #region Camera Effect
    public void StartShake()
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }

        _shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        _fadeTween.Kill();
        CanvasManager.Instance.FadeOut();
        _startText.gameObject.SetActive(false);

        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            Vector3 randomPoint = _originalPosition + Random.insideUnitSphere * shakeMagnitude;
            randomPoint.z = _originalPosition.z;
            _mainCamera.transform.localPosition = randomPoint;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _mainCamera.transform.localPosition = _originalPosition;
        SceneManager.LoadScene("DeepWater_Ingame");
    }
    #endregion
}
