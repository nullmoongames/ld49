using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crest;

public class LevelGenerationManager : MonoBehaviour
{
    [Header("Boat")]
    public GameObject[] boats;

    [Header("Generation Parameters"), Space(10)]
    public int generatedBoatCountEachCycle;
    public float passedBoatDestroyedDistance;

    [Space(20)]
    [UnityEngine.Range(0, 100)] public float forwardMinDistBetweenBoats;
    [UnityEngine.Range(0, 100)] public float forwardMaxDistBetweenBoats;
    [UnityEngine.Range(-30, 30)] public float sideMinDistBetweenBoats, sideMaxDistBetweenBoats;


    private Transform _lastGeneratedBoat;

    void Awake()
    {
        _lastGeneratedBoat = FindObjectOfType<BoatProbes>().transform;

        _NewGeneration();
    }

    void Update()
    {
        
    }

    void _NewGeneration()
    {
        for(int i = 0; i < generatedBoatCountEachCycle; i++)
        {
            GameObject boat = Instantiate(_GetRandomBoat(), _GetInstiantiationPosition(_lastGeneratedBoat.GetChild(0).position), Quaternion.identity);
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
}
