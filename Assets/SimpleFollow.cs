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

    private Vector3 _newPos;

    void Update()
    {
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
