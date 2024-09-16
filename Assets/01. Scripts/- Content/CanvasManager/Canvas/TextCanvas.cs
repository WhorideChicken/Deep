using UnityEngine;

public class TextCanvas : MonoBehaviour, ICanvas
{
    [SerializeField] private GameObject _interactionText;

    public void Show()
    {
        _interactionText.SetActive(true);
    }

    public void Hide()
    {
        _interactionText.SetActive(false);
    }
    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
    public void InteractionText(bool show)
    {
        _interactionText.SetActive(show);
    }
}
