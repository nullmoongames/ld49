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
        public bool attachedToParent;

        [Range(0, 100)]
        public float minimumChaosRequiredToSpawn, maximumChaosRequiredToSpawn;
    }

    [System.Serializable]
    public class OceanProp 
    {
        public GameObject[] propVariant;
        public bool attachedToParent;

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

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(SpawnProps());
    }

    IEnumerator SpawnProps()
    {
        yield return new WaitForEndOfFrame();
        //GameEventController.instance.ReloadOcean();

        _chaosPercent = GameManager.instance.chaosPercent;

        for (int i = 0; i < boatPropSpawnPoints.Length; i++)
        {
            float chance = Random.Range((GameManager.instance.chaosPercent / 100), 1);

            if (chance > 0.20f)
            {
                int rr = Random.Range(0, boatProps.Count);

                if (boatProps[rr].attachedToParent)
                    Instantiate(boatProps[rr].propVariant[Random.Range(0, boatProps[rr].propVariant.Length)], boatPropSpawnPoints[i]);
                else
                    Instantiate(boatProps[rr].propVariant[Random.Range(0, boatProps[rr].propVariant.Length)], boatPropSpawnPoints[i].position, Quaternion.identity);
            }

            //if (_chaosPercent >= boatProps[rr].minimumChaosRequiredToSpawn && _chaosPercent <= boatProps[rr].maximumChaosRequiredToSpawn)
            //{

            //}
        }

        for (int i = 0; i < oceanPropSpawnPoints.Length; i++)
        {
            float chance = Random.Range((GameManager.instance.chaosPercent / 100), 1);

            if (chance > 0.25f)
            {
                int rr = Random.Range(0, oceanProps.Count);

                if (_chaosPercent >= oceanProps[rr].minimumChaosRequiredToSpawn && _chaosPercent <= oceanProps[rr].maximumChaosRequiredToSpawn)
                {
                    if (oceanProps[rr].attachedToParent)
                        Instantiate(oceanProps[rr].propVariant[Random.Range(0, oceanProps[rr].propVariant.Length)], oceanPropSpawnPoints[i]);
                    else
                        Instantiate(oceanProps[rr].propVariant[Random.Range(0, oceanProps[rr].propVariant.Length)], oceanPropSpawnPoints[i].position, Quaternion.identity);
                }
            }
        }
    }

    public void Update()
    {
        //DEBUG
        _fixedBoatRot = transform.eulerAngles;

        if (transform.position.y >= heightConsideringBoatIsBug)
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }


        if(transform.rotation.x > rotationConsideringBoatIsBug.x) 
        {
            //Debug.Log("X is bug");
            _fixedBoatRot.x = 0;
        }

        if(transform.rotation.x < -rotationConsideringBoatIsBug.x)
        {
            //Debug.Log("-X is bug");
            _fixedBoatRot.x = 0;
        }

        if (transform.rotation.y > rotationConsideringBoatIsBug.y)
        {
            Debug.Log("Y is bug");
            _fixedBoatRot.y = 0;
        }

        if (transform.rotation.y < -rotationConsideringBoatIsBug.y)
        {
            Debug.Log("-Y is bug");
            _fixedBoatRot.y = 0;
        }

        if (transform.rotation.z > rotationConsideringBoatIsBug.z)
        {
            Debug.Log("Z is bug");
            _fixedBoatRot.z = 0;
        }

        if(transform.rotation.z < -rotationConsideringBoatIsBug.z)
        {
            Debug.Log("-Z is bug");
            _fixedBoatRot.z = 0;
        }

        transform.eulerAngles = _fixedBoatRot;
    }
}
