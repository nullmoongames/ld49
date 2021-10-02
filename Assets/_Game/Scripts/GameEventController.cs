using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventController : MonoBehaviour
{
    public static GameEventController instance;
    private Transform _actualBoat;

    public float timeBeforeBoatPassedDestruction = 3f;

    private Transform _startingBoat;

    private void Awake()
    {
        instance = this;
        Crest.BoatProbes _crestBoat = FindObjectOfType<Crest.BoatProbes>();

        if (_crestBoat != null)
        {
            _actualBoat = _crestBoat.transform;
            _startingBoat = _actualBoat;
        }
    }
    public void DeathEvent()
    {
        GameManager.instance.ReloadLevel();
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

    public Transform GetStartingBoat()
    {
        return _startingBoat;
    }
}
