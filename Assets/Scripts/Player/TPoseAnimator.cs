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

    public bool AnimationComplete()
    {
        var animation = m_animator.GetCurrentAnimatorStateInfo(0);
        return animation.IsName("TPose") && animation.normalizedTime >= 1f;
    }
}
