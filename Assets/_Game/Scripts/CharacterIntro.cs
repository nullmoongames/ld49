using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIntro : MonoBehaviour
{
    AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PlayScreamSound()
    {
        m_audioSource.pitch = Random.Range(.8f, 1.2f);
        m_audioSource.Play();
    }
}
