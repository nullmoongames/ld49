using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.y += 1;

        transform.eulerAngles = currentRotation;
    }
}
