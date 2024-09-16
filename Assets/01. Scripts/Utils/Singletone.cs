using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance => instance;

    private static T instance;

    public bool IsDDO = false;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this.GetComponent<T>();

            if (IsDDO)
                DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}