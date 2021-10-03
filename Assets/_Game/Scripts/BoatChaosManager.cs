using Sirenix.OdinInspector;
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

    public float heightConsideringBoatIsBug = 75f;
    public Vector3 rotationConsideringBoatIsBug = new Vector3(15, 10, 40);
    private Vector3 _fixedBoatRot;

    private Rigidbody rb;

    public void Start()
    {

        rb = GetComponent<Rigidbody>();
        //GameEventController.instance.ReloadOcean();

        _chaosPercent = GameManager.instance.chaosPercent;

        //for(int i = 0; i < boatPropSpawnPoints.Length; i++)
        //{
        //    if(_chaosPercent >= boatProps[i].minimumChaosRequiredToSpawn && _chaosPercent <= boatProps[i].maximumChaosRequiredToSpawn)
        //        Instantiate(boatProps[i].propVariant[Random.Range(0, boatProps[i].propVariant.Length)], boatPropSpawnPoints[i]);
        //}

        for (int i = 0; i < oceanPropSpawnPoints.Length; i++)
        {
            float change = Random.Range((GameManager.instance.chaosPercent / 100), 1);

            if (change < 0.25f)
                return;

            int rr = Random.Range(0, oceanProps.Count);

            if (_chaosPercent >= oceanProps[rr].minimumChaosRequiredToSpawn && _chaosPercent <= oceanProps[rr].maximumChaosRequiredToSpawn)
                Instantiate(oceanProps[rr].propVariant[Random.Range(0, oceanProps[rr].propVariant.Length)], oceanPropSpawnPoints[i]);
        }
    }

    public void Update()
    {
        //DEBUG
        _fixedBoatRot = transform.eulerAngles;

        //if(transform.position.x < 0)
        //{
        //    Debug.Log("A");
        //}
        //else
        //{
        //    Debug.Log("B");
        //}

        if (transform.position.y >= heightConsideringBoatIsBug)
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }


        if(transform.rotation.x > rotationConsideringBoatIsBug.x) 
        {
            Debug.Log("X is bug");
            _fixedBoatRot.x = 0;
        }

        if(transform.rotation.x < -rotationConsideringBoatIsBug.x)
            _fixedBoatRot.x = 0;

        if (transform.rotation.y > rotationConsideringBoatIsBug.y || transform.rotation.y < -rotationConsideringBoatIsBug.y)
        {
            Debug.Log("Y is bug");
            _fixedBoatRot.y = 0;
        }

        if (transform.rotation.z > rotationConsideringBoatIsBug.z || transform.rotation.z < -rotationConsideringBoatIsBug.z)
        {
            Debug.Log("Z is bug");
            _fixedBoatRot.z = 0;
        }

        transform.eulerAngles = _fixedBoatRot;
    }
}
