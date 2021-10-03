using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
    public float timeBfrDestruction;

    private void Awake()
    {
        Destroy(this, timeBfrDestruction);   
    }
}
