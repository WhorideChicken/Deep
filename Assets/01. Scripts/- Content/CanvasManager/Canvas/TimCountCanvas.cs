using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimCountCanvas : MonoBehaviour, ICanvas
{
    [SerializeField] private Text _timerText; 
    [SerializeField] private Image _fillTimeImage; 
    [SerializeField] private Image _fillOxygenImage; 
    [SerializeField] private Button _exitButton;

    [SerializeField] private float _totalTime = 600f; 

    private float _remainingTime;
    private Coroutine _timerCoroutine;

    private void Start()
    {
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }
    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
    public void Show()
    {
        gameObject.SetActive(true);
        ResetUI();
        StartTimer();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        StopTimer();
    }

    private void ResetUI()
    {
        _fillTimeImage.fillAmount = 1.0f;
        _fillOxygenImage.fillAmount = 0.5f;
        _remainingTime = _totalTime;
        UpdateTimerUI();
    }

    private void StartTimer()
    {
        StopTimer(); // 타이머 중복 실행 방지
        _timerCoroutine = StartCoroutine(Countdown());
    }

    private void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }

    private IEnumerator Countdown()
    {
        while (_remainingTime > 0)
        {
            UpdateTimerUI();
            HandleOxygenLevel();

            yield return new WaitForSeconds(1f);
            _remainingTime -= 1f;
        }

        OnTimerEnd();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(_remainingTime / 60F);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);
        _timerText.text = $"{minutes:00}:{seconds:00}";
        _fillTimeImage.fillAmount = _remainingTime / _totalTime;
    }

    private void HandleOxygenLevel()
    {
        if (!GameManager.Instance.OxygenDone)
        {
            _fillOxygenImage.fillAmount -= 0.001f;
            if (_fillOxygenImage.fillAmount <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            _fillOxygenImage.fillAmount = 1.0f;
        }
    }

    private void OnTimerEnd()
    {
        _timerText.text = "00:00";
        _fillTimeImage.fillAmount = 0f;
        GameManager.Instance.GameOver();
    }

    private void OnExitButtonClicked()
    {
        SceneManager.LoadScene("DeepWater_Intro");
    }
}
