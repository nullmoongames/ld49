using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    Rigidbody m_rigidbody;
    [SerializeField]
    GameObject m_cannonballPrefab;
    [SerializeField]
    Transform m_cannonballSpawnpoint;
    [SerializeField]
    float m_minWaitTime = 2f;
    [SerializeField]
    float m_maxWaitTime = 5f;
    [SerializeField]
    float m_cannonballForce = 20f;
    [SerializeField]
    AudioSource m_cannonballAudioSource;
    [SerializeField]
    ParticleSystem m_particleShoot;

    
    private float m_timer;

    private void Start()
    {
        m_timer = Random.Range(m_minWaitTime, m_maxWaitTime);
    }

    private void Update()
    {
        if (m_timer > 0f)
            m_timer -= Time.deltaTime;
        else
        {
            ShootCannonball();
            m_timer = Random.Range(m_minWaitTime, m_maxWaitTime);

            m_cannonballAudioSource.pitch = Random.Range(.8f, 1.2f);
            m_cannonballAudioSource.Play();
        }

    }

    void ShootCannonball()
    {
        GameObject cb = Instantiate(m_cannonballPrefab);
        cb.transform.position = m_cannonballSpawnpoint.position;
        Rigidbody cannonballRigidbody = cb.GetComponent<Rigidbody>();

        cannonballRigidbody.AddForce(Vector3.left * m_cannonballForce, ForceMode.Impulse);
        Destroy(cb, 5f);
        m_rigidbody.AddForce(Vector3.right * m_cannonballForce * Random.Range(3, 4), ForceMode.Impulse);
        m_particleShoot.Play();
    }
}
