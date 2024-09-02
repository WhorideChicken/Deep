using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Threading;
using System;

public class PassWordGameCanvas : MonoBehaviour
{
    [SerializeField] Button[] _numberPad;
    private int[] passwords;
    private int _passwordCount = 5;
    private int _clickCount = 0;
    private UnityAction _action;

    private bool _isReady = false;
    private CancellationTokenSource _cancellationTokenSource;

    private void Initialize()
    {
        passwords = new int[] {
            0, 1, 2, 3, 4, 5,
            6, 7, 8, 9, 10,
            11, 12, 13, 14, 15,
            16, 17, 18, 19, 20,
            21, 22, 23, 24
        };

        _clickCount = 0;
        for (int i = 0; i < _numberPad.Length; i++)
        {
            Image _img = _numberPad[i].GetComponent<Image>();
            _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, 0f);
        }
    }

    public async void StartPasswordGame(UnityAction action = null)
    {
        Initialize();
        SuffleArray();

        _action = action;

        _isReady = false;
        await Task.Delay(1000);

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = _cancellationTokenSource.Token;
        try
        {
            for (int i = 0; i < _passwordCount; i++)
            {
                var image = _numberPad[passwords[i]].GetComponent<Image>();

                // 애니메이션 시작 및 취소 가능하도록 설정
                await image.DOFade(1.0f, 0.25f).AsyncWaitForCompletion(cancellationToken);
                await image.DOFade(0.0f, 0.25f).AsyncWaitForCompletion(cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("애니메이션이 취소되었습니다.");
        }
        catch (Exception ex)
        { 
            Debug.LogError($"예상치 못한 에러가 발생했습니다: {ex.Message}");
        }

        _isReady = true;
    }

    private void SuffleArray()
    {
        for (int i = 0; i < passwords.Length; i++)
        {
            int rndIndex = UnityEngine.Random.Range(0, passwords.Length);
            int temp = passwords[i];
            passwords[i] = passwords[rndIndex];
            passwords[rndIndex] = temp;
        }
    }


    public void CheckPassword(int number)
    {
        if (!_isReady)
            return;
        
        if (_clickCount < _passwordCount-1)
        {
            Image _img = _numberPad[number].GetComponent<Image>();
            _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, 1f);

            if (passwords[_clickCount] != number)
            {
                StartPasswordGame();
                return;
            }
            _clickCount++;
        }
        else
        {
            Success();
        }
    }

    public void CloseGame()
    {
        Initialize();
        _cancellationTokenSource?.Cancel();
        this.gameObject.SetActive(false);
    }

    private void Success()
    {
        this.gameObject.SetActive(false);
        _cancellationTokenSource?.Cancel();
        _action?.Invoke();
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
    }
}
