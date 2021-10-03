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
    [SerializeField]
    AudioSource m_jumpAudioSource;

    Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

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

    public void Jump()
    {
        m_jumpAudioSource.pitch = Random.Range(1.2f, 1.6f);

        m_jumpAudioSource.Play();
    }
}
