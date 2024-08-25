using System.Collections.Generic;
using UnityEditor.Rendering;
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

    void Start()
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

    // 다음 문장을 표시하는 메서드
    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            _action?.Invoke();
            EndDialog(); 
            return;
        }

        string sentence = _sentences.Dequeue(); // 다음 문장 가져오기
        _dialogText.text = sentence;            // 텍스트에 문장 표시
    }

    // 다이얼로그 종료
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
