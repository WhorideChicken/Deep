using UnityEngine;

public class TextCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _interactionText;

    public void InteractionText(bool show)
    {
        _interactionText.SetActive(show);
    }
}
