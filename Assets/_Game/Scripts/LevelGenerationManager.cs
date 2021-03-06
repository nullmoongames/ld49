using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crest;

public class LevelGenerationManager : MonoBehaviour
{
    [Header("Boat")]
    public GameObject[] boats;
    private List<GameObject> generatedBoats = new List<GameObject>();

    [Header("Generation Parameters"), Space(10)]
    public int generatedBoatCountEachCycle;
    public float passedBoatDestroyedDistance;

    [Space(20)]
    [UnityEngine.Range(0, 300)] public float forwardMinDistBetweenBoats;
    [UnityEngine.Range(0, 300)] public float forwardMaxDistBetweenBoats;
    [UnityEngine.Range(-30, 30)] public float sideMinDistBetweenBoats, sideMaxDistBetweenBoats;
    [UnityEngine.Range(-30, 30)] public float upMinDistBetweenBoats, upMaxDistBetweenBoats;


    private Transform _lastGeneratedBoat;

    public static LevelGenerationManager instance;

    void Start()
    {
        instance = this;

        StartLevelGeneration();
    }

    void _NewMultipleGeneration()
    {
        for(int i = 0; i < generatedBoatCountEachCycle; i++)
        {
            Vector3 newPos = _lastGeneratedBoat.GetChild(0).position;
            newPos.y = 0;

            GameObject boat = Instantiate(_GetRandomBoat(), _GetInstiantiationPosition(newPos), Quaternion.identity);
            generatedBoats.Add(boat);
            _lastGeneratedBoat = boat.transform;
        }
    }

    void _NewSingleGeneration()
    {
        Debug.Log("New boat");
        Vector3 newPos = _lastGeneratedBoat.GetChild(0).position;
        newPos.y = 0;

        GameObject boat = Instantiate(_GetRandomBoat(), _GetInstiantiationPosition(newPos), Quaternion.identity);
        generatedBoats.Add(boat);
        _lastGeneratedBoat = boat.transform;
    }

    public void NewSingleGeneration()
    {
        _NewSingleGeneration();
    }

    GameObject _GetRandomBoat()
    {
        return boats[Random.Range(0, boats.Length)];
    }

    Vector3 _GetInstiantiationPosition(Vector3 __lastGeneratedBoatEndPoint) 
    {
        //Forward
        __lastGeneratedBoatEndPoint.z += Random.Range(forwardMinDistBetweenBoats, forwardMaxDistBetweenBoats);

        //Side
        __lastGeneratedBoatEndPoint.x = Random.Range(sideMinDistBetweenBoats, sideMaxDistBetweenBoats);

        //Up
        __lastGeneratedBoatEndPoint.y = Random.Range(upMinDistBetweenBoats, upMaxDistBetweenBoats);

        return __lastGeneratedBoatEndPoint;
    }

    public void DestroyBoats() 
    {
        for(int i = 0; i < generatedBoats.Count; i++) 
        {
            Destroy(generatedBoats[i]);
        }

        generatedBoats.Clear();
    }

    public void StartLevelGeneration() 
    {
        _lastGeneratedBoat = GameEventController.instance.GetStartingBoat();

        _NewMultipleGeneration();
    }
}
