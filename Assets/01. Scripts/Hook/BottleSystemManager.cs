using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;


public class BottleSystemManager : MonoBehaviour
{
    public static BottleSystemManager Instance;
    public GameObject BottleSystem;

    [SerializeField] private Bottle _bottle;

    public float checkDelay = 1f;

    public UnityAction successBottleGame = null;
    public UnityAction failBottleGame = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    public void StartBottleGame(UnityAction success, UnityAction fail)
    {
        BottleSystem.SetActive(true);
        
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
        BottleSystem.SetActive(false);
    }
}
