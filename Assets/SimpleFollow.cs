using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [Header("Follow Axis")]
    public bool x;
    public bool y, z;

    [Header("Followed Object")]
    public Transform target;

    private Vector3 _newPos;

    void Update()
    { 
        if (x)
            _newPos.x = target.transform.position.x;

        if (y)
            _newPos.y = target.transform.position.y;

        if (z)
            _newPos.z = target.transform.position.z;

        transform.position = _newPos;

    }
}
