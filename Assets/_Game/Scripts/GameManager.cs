using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void ReloadLevel()
    {

    }

    public void DeathEvent()
    {
        //DEATH UI
        //TITTLE SCREEN
        //PLAY AGAIN
        //QUIT
    }
}
