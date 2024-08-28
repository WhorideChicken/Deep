using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] private FadeCanvas _fadeCanvas;
    [SerializeField] private TextCanvas _textCanvas;
    [SerializeField] private DialogCanvas _dialogCanvas;
    [SerializeField] private TimCountCanvas _timeCanvas;

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

    public void ScreenFadeInOut(UnityAction action)
    {
        _fadeCanvas.ScreenFadeInOut(action);
    }

    public void ScreenFadeOut()
    {
        _fadeCanvas.ScreenFadeOut();
    }

    public void ScreenFadeIn()
    {
        _fadeCanvas.ScreenFadeIn();
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
        ScreenInteractionText(false);

    }
}
