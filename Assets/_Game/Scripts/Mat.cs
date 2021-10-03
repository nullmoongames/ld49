using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mat : MonoBehaviour
{
    [SerializeField]
    float m_collisionDistanceFromPlayer = 150f;

    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.isKinematic = true;
        m_rigidbody.mass = 100f;
    }

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(40, 250, 142);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_rigidbody.isKinematic = false;
            m_rigidbody.AddForceAtPosition(Vector3.right * Random.Range(-1, 1) * 10f, transform.position, ForceMode.Impulse);
        }
    }
}
