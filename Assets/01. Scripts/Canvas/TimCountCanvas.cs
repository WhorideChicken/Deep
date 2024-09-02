using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimCountCanvas : MonoBehaviour
{
    public Text timerText; // 타이머 텍스트를 표시할 UI Text 컴포넌트
    public Image fillImage; // fillAmount를 조절할 이미지
    public Image fillOxygenImage;
    public float totalTime = 600f; // 총 시간 (10분 = 600초)

    public Button exitButton;
    
    private float remainingTime;
    private Coroutine _timerCor = null;

    void Start()
    {
        exitButton.onClick.AddListener(() => {
            SceneManager.LoadScene("DeepWater_Intro");
        });

    }

    private void OnEnable()
    {
        fillImage.fillAmount = 1.0f;
        fillOxygenImage.fillAmount = 0.5f;
        StartTimerCoroutine();
    }

    private void StartTimerCoroutine()
    {
        if(_timerCor != null)
        {
            StopCoroutine(_timerCor);
            _timerCor = null;
        }

        _timerCor = StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        float remainingTime = totalTime; // 남은 시간을 초기화

        while (remainingTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60F);
            int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            fillImage.fillAmount = remainingTime / totalTime;
            if (!GameManager.Instance.OxyenDone)
            {
                fillOxygenImage.fillAmount -= 0.001f;
                if (fillOxygenImage.fillAmount <= 0)
                {
                    break;
                }
            }
            else
                fillOxygenImage.fillAmount = 1.0f;

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        // 타이머가 끝났을 때 최종 상태 업데이트
        timerText.text = "00:00";
        fillImage.fillAmount = 0f;

        //TODO GameOver
        GameManager.Instance.GameOver();
    }
}
