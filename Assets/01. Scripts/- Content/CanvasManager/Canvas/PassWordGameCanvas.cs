using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Threading;
using System;

public class PassWordGameCanvas : MonoBehaviour, ICanvas
{
    [SerializeField] private Button[] _numberPad;
    private int[] _passwords;
    private int _passwordCount = 5;
    private int _clickCount = 0;
    private UnityAction _onSuccessAction;

    private bool _isReady = false;
    private CancellationTokenSource _cancellationTokenSource;

    private void Initialize()
    {
        _passwords = new int[] {
            0, 1, 2, 3, 4, 5,
            6, 7, 8, 9, 10,
            11, 12, 13, 14, 15,
            16, 17, 18, 19, 20,
            21, 22, 23, 24
        };

        _clickCount = 0;
        for (int i = 0; i < _numberPad.Length; i++)
        {
            _numberPad[i].transform.parent.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-360, 360));
            Image img = _numberPad[i].GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Initialize();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _cancellationTokenSource?.Cancel();
    }

    public async void StartPasswordGame(UnityAction onSuccessAction = null)
    {
        Initialize();
        ShuffleArray();

        _onSuccessAction = onSuccessAction;

        _isReady = false;
        await Task.Delay(1000);

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = _cancellationTokenSource.Token;

        try
        {
            for (int i = 0; i < _passwordCount; i++)
            {
                var image = _numberPad[_passwords[i]].GetComponent<Image>();

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

    private void ShuffleArray()
    {
        for (int i = 0; i < _passwords.Length; i++)
        {
            int rndIndex = UnityEngine.Random.Range(0, _passwords.Length);
            int temp = _passwords[i];
            _passwords[i] = _passwords[rndIndex];
            _passwords[rndIndex] = temp;
        }
    }

    public void CheckPassword(int number)
    {
        if (!_isReady)
            return;

        if (_clickCount < _passwordCount - 1)
        {
            Image img = _numberPad[number].GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);

            if (_passwords[_clickCount] != number)
            {
                StartPasswordGame(_onSuccessAction);
                return;
            }
            _clickCount++;
        }
        else
        {
            OnSuccess();
        }
    }
    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
    public void CloseGame()
    {
        Hide();
    }

    private void OnSuccess()
    {
        Hide();
        _onSuccessAction?.Invoke();
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
    }
}
