using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPoseAnimator : MonoBehaviour
{
    private Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.Play("Idle");
    }

    public void PlayTPose()
    {
        m_animator.Play("TPose");
    }
}
