using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singletone<GameManager>
{
    public Dialog startGameDialog;
    public Dialog endingDead;
    public Dialog endingZDialog;
    public Dialog guideDialog;
    public Dialog failBottleDialog;

    [SerializeField] private CommonCharacterController _player;

    public bool OxygenDone { get; set; } = false;

    private void Start()
    {
        CanvasManager.Instance.ShowCanvas(Define.CanvasType.TimeCanvas);
        CanvasManager.Instance.StartDialog(startGameDialog, StartBottleGame);
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
        await CanvasManager.Instance.FadeOut();
        BottleSystemManager.Instance.EndBottleGame();
        CanvasManager.Instance.StartDialog(endingZDialog, GameModeChange);
    }

    private void FailBottleGame()
    {
        BottleSystemManager.Instance.EndBottleGame();
        CanvasManager.Instance.StartDialog(guideDialog);
    }

    public bool IsMovable()
    {
        return !CanvasManager.Instance.IsDialogOn() &&
               !BottleSystemManager.Instance.BottleSystem.activeSelf &&
               !CanvasManager.Instance.IsGameOn();
    }

    public void GameOver()
    {
        if (!_player.IsDead)
        {
            _player.IsDead = true;
            CanvasManager.Instance.StartDialog(endingDead, GameModeChange);
        }
    }
}
