using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimCountCanvas : MonoBehaviour
{
    public Text timerText; // 타이머 텍스트를 표시할 UI Text 컴포넌트
    public Image fillImage; // fillAmount를 조절할 이미지
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
            // 시간 계산 (분 : 초)
            int minutes = Mathf.FloorToInt(remainingTime / 60F);
            int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);

            // "MM:SS" 형식으로 텍스트 설정
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // 이미지의 fillAmount를 남은 시간에 비례하여 조절 (0 ~ 1)
            fillImage.fillAmount = remainingTime / totalTime;

            // 1초 기다림
            yield return new WaitForSeconds(1f);

            // 남은 시간 감소
            remainingTime -= 1f;
        }

        // 타이머가 끝났을 때 최종 상태 업데이트
        timerText.text = "00:00";
        fillImage.fillAmount = 0f;

        //TODO GameOver
        GameManager.Instance.GameOver();
    }
}
