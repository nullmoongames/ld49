using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationManager : MonoBehaviour
{
    [Header("Boat")]
    public GameObject[] boats;

    [Header("Generation Parameters"), Space(10)]
    public int generatedBoatCountEachCycle;
    public float passedBoatDestroyedDistance;

    [Space(20)]
    [Range(0, 5)] public float forwardMinDistBetweenBoats;
    [Range(0, 5)] public float forwardMaxDistBetweenBoats;
    [Range(-3, 3)] public float sideMinDistBetweenBoats, sideMaxDistBetweenBoats;


    private Transform _lastGeneratedBoat;

    void Awake()
    {
        _lastGeneratedBoat = FindObjectOfType<BoatOnOcean>().transform;

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
