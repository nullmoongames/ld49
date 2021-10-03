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

    public void Footstep(string side)
    {
        GameObject ps;
        if (side == "right")
        {
            ps = Instantiate(m_footstepParticlePrefab, m_footstepRightPosition);
        } else
        {
            ps = Instantiate(m_footstepParticlePrefab, m_footstepLeftPosition);
        }

        Destroy(ps, 1f);
    }
}
