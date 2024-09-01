using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//TODO : Show와 hide를 통일하여 카메라 움직임 막기
public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] private FadeCanvas _fadeCanvas;
    [SerializeField] private TextCanvas _textCanvas;
    [SerializeField] private DialogCanvas _dialogCanvas;
    [SerializeField] private TimCountCanvas _timeCanvas;
    [SerializeField] private PassWordGameCanvas _pwGameCanvas;

    private void Awake()
    {
        if(instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;

        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsDialogOn()
    {
        return _dialogCanvas.isDialogActive;
    }
    public bool IsGameOn()
    {
        return _pwGameCanvas.gameObject.activeSelf;
    }
    public void ScreenFadeInOut(UnityAction action)
    {
        _fadeCanvas.ScreenFadeInOut(action);
    }

    public Task ScreenFadeOut()
    {
        return _fadeCanvas.ScreenFadeOut();
    }

    public Task ScreenFadeIn()
    {
        return _fadeCanvas.ScreenFadeIn();
    }

    public void ScreenInteractionText(bool isShow)
    {
        _textCanvas.InteractionText(isShow);
    }

    public void ScreenStartDialog(Dialog sentence, UnityAction action = null)
    {
        _dialogCanvas.StartDialog(sentence, action);
    }

    public void GUITImeCanvas(bool isOn)
    {
        _timeCanvas.gameObject.SetActive(isOn);
    }

    public void StartCanvasGame(UnityAction action)
    {
        _pwGameCanvas.gameObject.SetActive(true);
        _pwGameCanvas.StartPasswordGame(action);
    }

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        Debug.Log("change");
        ScreenFadeIn();
        _pwGameCanvas.gameObject.SetActive(false);
        ScreenInteractionText(false);

    }
}
