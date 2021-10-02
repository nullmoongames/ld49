using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float m_force = 10f;
    [SerializeField]
    private GameObject m_destructionParticleSystem;

    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();

            player.HitObstacle(m_force);

            GameObject _ps = Instantiate(m_destructionParticleSystem, null);
            Destroy(_ps, 5f);

            m_rigidbody.AddExplosionForce(100, transform.position, .9f);

            Destroy(this.gameObject);
        }
    }
}
