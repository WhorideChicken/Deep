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
    public Dialog endingZDialog;

    [SerializeField] CommonCharacterController _player;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    void Start()
    {
        CanvasManager.instance.ScreenFadeIn();
        CanvasManager.instance.GUITImeCanvas(true);
        CanvasManager.instance.ScreenStartDialog(startGameDialog, StartBottleGame);
        //StartBottleGame();
    }



    private void GameModeChange()
    {
        SceneManager.LoadScene("DeepWater_Intro");
    }
     

    private void StartBottleGame()
    {
        DiceManager.Instance.StartBottleGame(FirstSuccess, FailBottleGame);
    }

    private void FirstSuccess()
    {
        DiceManager.Instance.EndBottleGame();
    }

    private void FailBottleGame()
    {
        DiceManager.Instance.EndBottleGame();
    }

    public bool IsMovable()
    {
        if (CanvasManager.instance.IsDialogOn())
            return false;
        else if (DiceManager.Instance.gameObject.activeSelf)
            return false;
        else
            return true;

    }

    public void GameOver()
    {
        _player.CheckDeath(true);
        //_scenarioEnvets.Enqueue(() => {
        //    CanvasManager.instance.ScreenStartDialog("나는 결국 죽었다...");
        //});
        //_scenarioEnvets.Enqueue(() => {
        //    CanvasManager.instance.ScreenStartDialog("최선을 다해봤지만 결국 죽는다...");
        //});
        //_scenarioEnvets.Enqueue(() => {
        //    CanvasManager.instance.ScreenStartDialog("이게 꿈이길 바란다...");
        //});
        //_scenarioEnvets.Enqueue(() => {
        //    GameModeChange();
        //});
    }
}
