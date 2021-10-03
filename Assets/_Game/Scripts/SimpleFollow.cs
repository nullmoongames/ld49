using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [Header("Follow Axis")]
    public bool x;
    public bool y, z;

    [Header("Offset")]
    public Vector3 offset;

    [Header("Followed Object")]
    public Transform target;
    public bool forceStartingPosSet;

    private Vector3 _newPos;
    private Vector3 _startingPos;

    private void Awake()
    {
        _startingPos = transform.position;
    }

    void Update()
    {
        //if(forceStartingPosSet)


        _newPos = transform.position;

        if (x)
            _newPos.x = target.transform.position.x + offset.x;

        if (y)
            _newPos.y = target.transform.position.y + offset.y;

        if (z)
            _newPos.z = target.transform.position.z + offset.z;

        transform.position = _newPos;
    }
}
