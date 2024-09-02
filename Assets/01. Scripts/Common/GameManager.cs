using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Dialog startGameDialog;
    public Dialog endingDead;
    public Dialog endingZDialog;
    public Dialog guideDialog;
    public Dialog failBottleDialog;

    [SerializeField] CommonCharacterController _player;

    public bool OxyenDone = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        CanvasManager.instance.GUITImeCanvas(true);
        CanvasManager.instance.ScreenStartDialog(startGameDialog, StartBottleGame);
    }



    public void GameModeChange()
    {
        SceneManager.LoadScene("DeepWater_Intro");
    }


    private void StartBottleGame()
    {
        BottleSystemManager.Instance.StartBottleGame(FirstSuccess, FailBottleGame);
    }

    private async void FirstSuccess()
    {
        await CanvasManager.instance.ScreenFadeOut();
        BottleSystemManager.Instance.EndBottleGame();
        CanvasManager.instance.ScreenStartDialog(endingZDialog, GameModeChange);
    }

    private void FailBottleGame()
    {
        BottleSystemManager.Instance.EndBottleGame();
        CanvasManager.instance.ScreenStartDialog(guideDialog);
    }

    public bool IsMovable()
    {
        if (CanvasManager.instance.IsDialogOn())
            return false;
        else if (BottleSystemManager.Instance.BottleSystem.activeSelf)
            return false;
        else if(CanvasManager.instance.IsGameOn())
            return false;
        else
            return true;

    }

    public void GameOver()
    {
        if (!_player.IsDead)
        {
            _player.IsDead = true;
            CanvasManager.instance.ScreenStartDialog(endingDead, GameModeChange);
        }
    }



}
