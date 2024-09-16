using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static Define;

//TODO : Show와 hide를 통일하여 카메라 움직임 막기
public class CanvasManager : Singletone<CanvasManager>
{
    private readonly Dictionary<CanvasType, ICanvas> _canvases = new Dictionary<CanvasType, ICanvas>();

    [SerializeField] private FadeCanvas _fadeCanvas;
    [SerializeField] private TextCanvas _textCanvas;
    [SerializeField] private DialogCanvas _dialogCanvas;
    [SerializeField] private TimCountCanvas _timeCanvas;
    [SerializeField] private PassWordGameCanvas _pwGameCanvas;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;

        RegisterCanvas(CanvasType.FadeCanvas, _fadeCanvas);
        RegisterCanvas(CanvasType.TextCanvas, _textCanvas);
        RegisterCanvas(CanvasType.DialogCanvas, _dialogCanvas);
        RegisterCanvas(CanvasType.TimeCanvas, _timeCanvas);
        RegisterCanvas(CanvasType.PassWordGameCanvas, _pwGameCanvas);
    }

    public void RegisterCanvas(CanvasType key, ICanvas canvas)
    {
        if (!_canvases.ContainsKey(key))
        {
            _canvases.Add(key, canvas);
        }
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private async void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        await FadeIn();
        HideCanvas(CanvasType.PassWordGameCanvas);
        ToggleInteractionText(false);
    }

    #region Canvas Func
    public void ShowCanvas(CanvasType key)
    {
        if (_canvases.TryGetValue(key, out ICanvas canvas))
        {
            canvas.Show();
        }
    }
    public void HideCanvas(CanvasType key)
    {
        if (_canvases.TryGetValue(key, out ICanvas canvas))
        {
            canvas.Hide();
        }
    }
    public bool IsDialogOn()
    {
        if (_canvases.TryGetValue(CanvasType.DialogCanvas, out ICanvas canvas))
        {
            return canvas.IsActive();
        }
        return false;
    }

    public bool IsGameOn()
    {
        if (_canvases.TryGetValue(CanvasType.PassWordGameCanvas, out ICanvas canvas))
        {
            return canvas.IsActive();
        }
        return false;
    }

    public void ToggleInteractionText(bool isShow)
    {
        if (_canvases.TryGetValue(CanvasType.TextCanvas, out ICanvas canvas) && canvas is TextCanvas textCanvas)
        {
            textCanvas.InteractionText(isShow);
        }
    }

    public void StartDialog(Dialog sentence, UnityAction action = null)
    {
        if (_canvases.TryGetValue(CanvasType.DialogCanvas, out ICanvas canvas) && canvas is DialogCanvas dialogCanvas)
        {
            dialogCanvas.StartDialog(sentence, action);
        }
    }

    public async Task FadeOut()
    {
        if (_canvases.TryGetValue(CanvasType.FadeCanvas, out ICanvas canvas) && canvas is FadeCanvas fadeCanvas)
        {
            await fadeCanvas.ScreenFadeOut();
        }
    }

    public async Task FadeIn()
    {
        if (_canvases.TryGetValue(CanvasType.FadeCanvas, out ICanvas canvas) && canvas is FadeCanvas fadeCanvas)
        {
            await fadeCanvas.ScreenFadeIn();
        }
    }

    public void StartCanvasGame(UnityAction action)
    {
        ShowCanvas(CanvasType.PassWordGameCanvas);
        if (_pwGameCanvas != null)
        {
            _pwGameCanvas.StartPasswordGame(action);
        }
    }

    #endregion
}
