using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathFire : MonoBehaviour
{
    [SerializeField]
    float m_speed = 10f;

    // Update is called once per frame
    void Update()
    {
        float step = m_speed * Time.deltaTime;

        Vector3 position = transform.position;
        position.z += step;

        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameEventController.instance.DeathEvent();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
