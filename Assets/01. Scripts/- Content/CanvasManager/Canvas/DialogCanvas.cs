using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogCanvas : MonoBehaviour, ICanvas
{
    [SerializeField] public GameObject _dialogUI;
    [SerializeField] private Text _dialogText;

    private UnityAction _action;
    private Dialog currentDialog;
    private Queue<string> _sentences = new Queue<string>();


    private void Update()
    {
        if (IsActive() && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }
    public bool IsActive()
    {
        return _dialogUI.activeSelf; 
    }

    public void Show()
    {
        _dialogUI.SetActive(true);
    }

    public void Hide()
    {
        _dialogUI.SetActive(false);
    }

    public void StartDialog(Dialog dialog, UnityAction action = null)
    {
        Show();
        _action = action;
        currentDialog = dialog;

        _sentences.Clear();
        foreach (var sentence in dialog.sentences)
        {
            _sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            _action?.Invoke();
            Hide();
            return;
        }

        _dialogText.text = _sentences.Dequeue();
    }
}
