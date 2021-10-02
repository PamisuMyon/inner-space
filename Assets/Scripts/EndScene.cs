using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    public float restartDelay;

    void Start()
    {
        Invoke("Restart", restartDelay);
    }

    void Restart()
    {
        GameManager.Instance.Title();
    }
}
