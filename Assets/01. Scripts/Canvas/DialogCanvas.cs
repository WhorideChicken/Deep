using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogCanvas : MonoBehaviour
{
    [SerializeField] public GameObject dialogUI;
    [SerializeField] private Text _dialogText;

    private UnityAction _action;
    private Dialog currentDialog; 
    private Queue<string> _sentences;
    public bool isDialogActive;

    void Awake()
    {
        isDialogActive = false;
        _action = null;
        _sentences = new Queue<string>();
    }

    void Update()
    {
        if (isDialogActive && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }


    public void StartDialog(Dialog dialog, UnityAction action = null)
    {
        dialogUI.SetActive(true);   
        isDialogActive = true;     

        _sentences.Clear();

        _action = action;
        currentDialog = dialog;

        foreach (string sentence in currentDialog.sentences)
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
            EndDialog(); 
            return;
        }

        string sentence = _sentences.Dequeue(); 
        _dialogText.text = sentence;            
    }


    void EndDialog()
    {
        dialogUI.SetActive(false);  
        _dialogText.text = string.Empty;

        _sentences.Clear();
        currentDialog = null;
        _action = null;
        isDialogActive = false;     
    }
}
