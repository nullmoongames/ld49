using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRotation : MonoBehaviour
{
    float m_maxRotation = 35f;
    bool m_isRotatedRight;
    float rotationDuration;

    // Start is called before the first frame update
    void Start()
    {
        float random = Random.Range(-1, 1);
        m_isRotatedRight = random > 0;

        ToggleRotationDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleRotationDirection()
    {
        rotationDuration = Random.Range(3, 5);
        Vector3 rotation = transform.eulerAngles;

        m_isRotatedRight = !m_isRotatedRight;
        float randomRotation = Random.Range(15, m_maxRotation);

        float endRotation = m_isRotatedRight ? -randomRotation : randomRotation + 360;

        StartCoroutine(LerpRotation(rotation.z, endRotation));
    }

    IEnumerator LerpRotation(float start, float end)
    {
        Vector3 rotation = transform.eulerAngles;

        float timeElapsed = 0f;
        float percent;

        while(timeElapsed < rotationDuration)
        {
            percent = timeElapsed / rotationDuration;

            rotation.z = Mathf.Lerp(start, end, percent);
            transform.eulerAngles = rotation;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        ToggleRotationDirection();
    }
}
