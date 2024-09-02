using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.Events;

public class FadeCanvas : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;

    public async void ScreenFadeInOut(Task task = null)
    {
        Tween Tween = _fadeImage.DOFade(1.0f, 1.0f);
        await Tween.AsyncWaitForCompletion();

        if(task != null)
            await task;

        Tween = _fadeImage.DOFade(0.0f, 1.0f);
        await Tween.AsyncWaitForCompletion();
    }

    public void ScreenFadeInOut(UnityAction action)
    {
        _fadeImage.DOFade(1.0f, 1.0f).OnComplete(() =>
        {
            action?.Invoke();
            _fadeImage.DOFade(0.0f, 1.0f);
        });
    }

    public Task ScreenFadeOut()
    {
       return _fadeImage.DOFade(1.0f, 1.0f).AsyncWaitForCompletion();
    }
    public Task ScreenFadeIn()
    {
        return _fadeImage.DOFade(0.0f, 1.0f).AsyncWaitForCompletion(); ;
    }
}
