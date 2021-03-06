using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventController : MonoBehaviour
{
    public static GameEventController instance;
    private Transform _actualBoat;

    public float timeBeforeBoatPassedDestruction = 3f;

    private Transform _startingBoat;
    private Crest.OceanRenderer ocean;

    private void Awake()
    {
        ocean = FindObjectOfType<Crest.OceanRenderer>();
        instance = this;
        _startingBoat = GameObject.FindGameObjectWithTag("StartingBoat").transform;
    }
    public void DeathEvent()
    {
        GameManager.instance.ReloadLevel();
        GameManager.instance.SaveHighscore();
        ReloadOcean();
    }

    public void ReloadOcean()
    {
        ocean.enabled = false;
        ocean.enabled = true;
    }

    public void NewEntryBoat(Transform __newActualBot)
    {
        if(_actualBoat != null)
            Destroy(_actualBoat.gameObject, timeBeforeBoatPassedDestruction);

        _actualBoat = __newActualBot;
        LevelGenerationManager.instance.NewSingleGeneration();
    }

    public Transform GetStartingBoat()
    {
        return _startingBoat;
    }
}
