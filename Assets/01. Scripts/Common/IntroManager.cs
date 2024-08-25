using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] Text _startText;
    public float shakeDuration = 0.0f;
    public float shakeMagnitude = 0.2f;
    private Vector3 originalPosition;
    private Coroutine shakeCor = null;
    private Camera _mainCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCam = Camera.main;
        _startText.DOFade(0.0f, 1.5f).SetLoops(-1,LoopType.Yoyo);
        originalPosition = _mainCam.transform.position;
        CanvasManager.instance.GUITImeCanvas(false);
        CanvasManager.instance.ScreenFadeIn();
    }

    // Update is called once per frame
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
        if (shakeCor != null)
        {
            StopCoroutine(shakeCor);
            shakeCor = null;
        }

        shakeCor = StartCoroutine(Shake());
        StartCoroutine(Shake());

    }

    private IEnumerator Shake()
    {
        CanvasManager.instance.ScreenFadeOut();
        _startText.gameObject.SetActive(false);
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            randomPoint.z = originalPosition.z;

            _mainCam.transform.localPosition = randomPoint;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        SceneManager.LoadScene("DeepWater_Ingame");
        _mainCam.transform.localPosition = originalPosition;
    }
    #endregion
}
