using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventController : MonoBehaviour
{
    public static GameEventController instance;
    private Transform _actualBoat;

    public float timeBeforeBoatPassedDestruction = 3f;

    private void Awake()
    {
        instance = this;
        _actualBoat = FindObjectOfType<Crest.BoatProbes>().transform;
    }
    public void DeathEvent()
    {
        GameManager.instance.ReloadLevel();
        Debug.Log("Death.");
        //DEATH UI
        //TITTLE SCREEN
        //PLAY AGAIN
        //QUIT
    }


    public void NewEntryBoat(Transform __newActualBot)
    {
        Destroy(_actualBoat, timeBeforeBoatPassedDestruction);
        _actualBoat = __newActualBot;
    }
}
