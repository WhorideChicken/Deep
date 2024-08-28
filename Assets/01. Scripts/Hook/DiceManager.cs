using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;

    private Bottle _bottle;

    public float checkDelay = 1f;

    public UnityAction successBottleGame = null;
    public UnityAction failBottleGame = null;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

    }

    public void StartBottleGame(UnityAction success, UnityAction fail)
    {
        this.gameObject.SetActive(true);
        ThrowBottle();

        successBottleGame = success;
        failBottleGame = fail;
    }


    public void ThrowBottle()
    {
        _bottle.ThrowBottle(CheckBottlePosition);
    }


    void CheckBottlePosition()
    {
        if (_bottle.IsBottleStandingUp())
        {
            successBottleGame();
        }
        else
        {
            failBottleGame();
        }

    }


    public void EndBottleGame()
    {
      
    }
}
