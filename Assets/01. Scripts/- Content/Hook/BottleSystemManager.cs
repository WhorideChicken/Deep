using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;


public class BottleSystemManager : Singletone<BottleSystemManager>
{

    [SerializeField] private Bottle _bottle;

    public GameObject BottleSystem;
    public float CheckDelay = 1f;

    private UnityAction _successBottleGame;
    private UnityAction _failBottleGame;


    public void StartBottleGame(UnityAction success, UnityAction fail)
    {
        BottleSystem.SetActive(true);
        _successBottleGame = success;
        _failBottleGame = fail;
        _bottle.ThrowBottle(CheckBottlePosition);
    }


    public void ThrowBottle()
    {
        _bottle.ThrowBottle(CheckBottlePosition);
    }


    void CheckBottlePosition()
    {
        if (_bottle.IsBottleStandingUp())
        {
            _successBottleGame?.Invoke();
        }
        else
        {
            _failBottleGame?.Invoke();
        }
    }

    public void EndBottleGame()
    {
        BottleSystem.SetActive(false);
    }
}
