using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.Events;

public class FadeCanvas : MonoBehaviour, ICanvas
{
    [SerializeField] private Image _fadeImage;

    public void Show()
    {
        ScreenFadeIn().ConfigureAwait(false);
    }

    public void Hide()
    {
        ScreenFadeOut().ConfigureAwait(false);
    }

    public async Task ScreenFadeIn()
    {
        await _fadeImage.DOFade(0.0f, 1.0f).AsyncWaitForCompletion();
    }

    public async Task ScreenFadeOut()
    {
        await _fadeImage.DOFade(1.0f, 1.0f).AsyncWaitForCompletion();
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    //TODO : 개선 필요
    public void ScreenFadeInOut(UnityAction action)
    {
        _fadeImage.DOFade(1.0f, 1.0f).OnComplete(() =>
        {
            action?.Invoke();
            _fadeImage.DOFade(0.0f, 1.0f);
        });
    }


}
