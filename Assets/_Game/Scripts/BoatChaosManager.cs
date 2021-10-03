using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatChaosManager : MonoBehaviour
{
    [System.Serializable]
    public class BoatProp
    {
        public GameObject[] propVariant;

        [Range(0, 100)]
        public float minimumChaosRequiredToSpawn, maximumChaosRequiredToSpawn;
    }

    [System.Serializable]
    public class OceanProp 
    {
        public GameObject[] propVariant;

        [Range(0, 100)]
        public float minimumChaosRequiredToSpawn, maximumChaosRequiredToSpawn;
    }

    public List<BoatProp> boatProps = new List<BoatProp>();
    public List<OceanProp> oceanProps = new List<OceanProp>();

    public Transform[] boatPropSpawnPoints;
    public Transform[] oceanPropSpawnPoints;

    private float _chaosPercent;

    public void Awake()
    {
        _chaosPercent = GameManager.instance.chaosPercent;

        for(int i = 0; i < boatPropSpawnPoints.Length; i++)
        {
            if(_chaosPercent >= boatProps[i].minimumChaosRequiredToSpawn && _chaosPercent <= boatProps[i].maximumChaosRequiredToSpawn)
                Instantiate(boatProps[i].propVariant[Random.Range(0, boatProps[i].propVariant.Length)], boatPropSpawnPoints[i]);
        }

        for (int i = 0; i < oceanPropSpawnPoints.Length; i++)
        {
            if (_chaosPercent >= oceanProps[i].minimumChaosRequiredToSpawn && _chaosPercent <= oceanProps[i].maximumChaosRequiredToSpawn)
                Instantiate(oceanProps[i].propVariant[Random.Range(0, oceanProps[i].propVariant.Length)], oceanPropSpawnPoints[i]);
        }
    }
}
