using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorFunctions : MonoBehaviour
{
    [SerializeField]
    private Transform m_footstepLeftPosition;
    [SerializeField]
    private Transform m_footstepRightPosition;
    [SerializeField]
    private GameObject m_footstepParticlePrefab;

    [SerializeField]
    AudioSource m_audioSourceLeft;
    [SerializeField]
    AudioSource m_audioSourceRight;

    public void Footstep(string side)
    {
        GameObject ps;

        m_audioSourceLeft.pitch = Random.Range(.7f, 1.3f);
        m_audioSourceRight.pitch = Random.Range(.7f, 1.3f);

        if (side == "right")
        {
            ps = Instantiate(m_footstepParticlePrefab, m_footstepRightPosition);
            m_audioSourceRight.Play();
        } else
        {
            ps = Instantiate(m_footstepParticlePrefab, m_footstepLeftPosition);
            m_audioSourceLeft.Play();
        }

        Destroy(ps, 1f);
    }
}
