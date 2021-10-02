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
    [UnityEngine.Range(0, 100)] public float forwardMinDistBetweenBoats;
    [UnityEngine.Range(0, 100)] public float forwardMaxDistBetweenBoats;
    [UnityEngine.Range(-30, 30)] public float sideMinDistBetweenBoats, sideMaxDistBetweenBoats;


    private Transform _lastGeneratedBoat;

    public static LevelGenerationManager instance;

    void Start()
    {
        instance = this;

        StartLevelGeneration();
    }

    void _NewGeneration()
    {
        for(int i = 0; i < generatedBoatCountEachCycle; i++)
        {
            GameObject boat = Instantiate(_GetRandomBoat(), _GetInstiantiationPosition(_lastGeneratedBoat.GetChild(0).position), Quaternion.identity);
            generatedBoats.Add(boat);
            _lastGeneratedBoat = boat.transform;
        }
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

        _NewGeneration();
    }
}
