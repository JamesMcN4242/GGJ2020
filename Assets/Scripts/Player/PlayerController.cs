using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private enum InputKeyRelation
    {
        UP = 0,
        LEFT,
        DOWN,
        RIGHT,
        INTERACT,
        T_POSE,
        SHOCK
    }

    private int k_interactableLayerMask = 1 << 8;

    [SerializeField]
    private float m_stunRadius = 2.0f;

    [SerializeField]
    private float m_timeToUnlockShocking = 4.0f;

    [SerializeField]
    private float m_playerMovementSpeed = 2f;

    [SerializeField]
    private float m_maxDirectionalSpeed = 5f;

    [SerializeField]
    private float m_interactRadius = 30.0f;

    [SerializeField]
    private float m_maxTimePlayerStunned = 4f;

    private Rigidbody m_rigidBody = null;
    private Collider[] m_overlappedColliders = null;
    private KeyCode[] m_inputButtons = null;
    private GameObject m_interactingObject = null;
    private InteractableObject m_interactScript = null;
    private Animator m_animator = null;
    private int m_scoreToAbsorb = 0;
    private float m_timeTillShockAble = 0f;
    private RectTransform m_stunRect = null;
    private float m_stunnedForTime = 0f;
    private GameObject m_stunnedEffect = null;

    public void Start()
    {
        m_timeTillShockAble = m_timeToUnlockShocking;
        m_rigidBody = this.GetComponent<Rigidbody>();
        m_animator = GetComponentInChildren<Animator>();
        m_animator.Play("Idle");
        m_stunnedEffect = transform.Find("StunnedVFX").gameObject;
        m_stunnedEffect.SetActive(false);
    }

    public void SetStunUI(RectTransform stunUI)
    {
        m_stunRect = stunUI;
    }

    private bool IsStunned()
    {
        return m_stunnedForTime > 0f;
    }

    public void SetInputKeys(KeyCode[] inputButtons)
    {
        m_inputButtons = inputButtons;
    }

    public void UpdatePlayerControls()
    {
        UpdateUIForStunGun();

        if(IsStunned())
        {
            m_stunnedForTime -= Time.deltaTime;
            if(m_stunnedForTime <= 0f)
            {
                m_stunnedEffect.SetActive(false);
            }
        }
        else
        {
            UpdateNonStunnedControls();
        }

    }

    private void UpdateNonStunnedControls()
    {
        if(InputKeyDown(InputKeyRelation.T_POSE))
        {
            m_animator.Play("TPose");
            m_rigidBody.velocity = Vector3.zero;
        }

        if(IsTPosing() == false)
        {
            UpdateInteractions();
            if(!IsInteracting)
            {
                UpdateMovement();
                if(CanStunAndWantsTo())
                {
                    StunPlayers();
                }
            }
        }
    }

    public void SetStunned()
    {
        m_stunnedForTime = m_maxTimePlayerStunned;
        m_stunnedEffect.SetActive(true);
        m_rigidBody.velocity = Vector3.zero;
    }

    private void StunPlayers()
    {
        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        foreach(PlayerController p in players)
        {
            if(p != this && Vector3.Distance(transform.position, p.transform.position) <= m_stunRadius)
            {
                p.SetStunned();
            }
        }
        m_timeTillShockAble = m_timeToUnlockShocking;
    }

    public bool CanStunAndWantsTo()
    {
        return m_timeTillShockAble <= 0f && InputKeyDown(InputKeyRelation.SHOCK);
    }

    public void UpdateUIForStunGun()
    {
        if(m_timeTillShockAble > 0.0f)
        {
            m_timeTillShockAble -= Time.deltaTime;
            if(m_timeTillShockAble <= 0.0f)
            {
                m_timeTillShockAble = 0.0f;
            }
            m_stunRect.anchorMax = new Vector2(1.0f, m_timeTillShockAble/m_timeToUnlockShocking);
            m_stunRect.offsetMax = new Vector2(m_stunRect.offsetMax.x, 0f);
        }
    }

    public int ConsumeScore()
    {
        int score = m_scoreToAbsorb;
        m_scoreToAbsorb = 0;
        return score;
    }

    private bool IsTPosing()
    {
        var animation = m_animator.GetCurrentAnimatorStateInfo(0);
        return animation.IsName("TPose") && animation.normalizedTime < 1.0f;
    }

    private void UpdateInteractions()
    {
        if(InputKeyDown(InputKeyRelation.INTERACT))
        {
            if(!IsInteracting)
            {
                m_interactingObject = FindObjectsToInteractWith();
                if(IsInteracting)
                {
                    m_interactScript = m_interactingObject.GetComponent<InteractableObject>();
                    m_interactScript.SetInteractedWith(true);
                    m_animator.Play("Repair");
                }
            }
            else if(m_interactScript.UpdateRepairTiming(Time.deltaTime))
            {
                m_scoreToAbsorb += m_interactScript.m_scoreForRebuild;
                m_interactingObject = null;
                m_interactScript = null;
                m_animator.Play("Idle");
            }
        }
        else if (IsInteracting)
        {
            m_interactScript.SetInteractedWith(false);
            m_interactScript = null;
            m_interactingObject = null;
            m_animator.Play("Idle");
        }
    }

    private void UpdateMovement()
    {
        Vector3 velocity = m_rigidBody.velocity;
        bool movingUp = InputKeyDown(InputKeyRelation.UP);
        bool movingDown = InputKeyDown(InputKeyRelation.DOWN);

        bool movingBothVerticals = movingUp && movingDown;
        if(!movingBothVerticals)
        {
            if(movingUp)
            {
                velocity = new Vector3(velocity.x, velocity.y, Mathf.Min(velocity.z + (m_playerMovementSpeed * Time.deltaTime), m_maxDirectionalSpeed));
            }
            else if (movingDown)
            {
                velocity = new Vector3(velocity.x, velocity.y, Mathf.Max(velocity.z - (m_playerMovementSpeed * Time.deltaTime), -m_maxDirectionalSpeed));
            }
        }

        bool movingRight = InputKeyDown(InputKeyRelation.RIGHT);
        bool movingLeft = InputKeyDown(InputKeyRelation.LEFT);
        bool movingBothHorizontal = movingRight && movingLeft;
        if(!movingBothHorizontal)
        {
            if(movingRight)
            {
                velocity = new Vector3(Mathf.Min(velocity.x + (m_playerMovementSpeed * Time.deltaTime), m_maxDirectionalSpeed), velocity.y, velocity.z);
            }
            else if (movingLeft)
            {
                velocity = new Vector3(Mathf.Max(velocity.x - (m_playerMovementSpeed * Time.deltaTime), -m_maxDirectionalSpeed), velocity.y, velocity.z);
            }
        }

        m_rigidBody.velocity = velocity;
        if(SpeedLessThanAnimationThreshold() == false)
        {
            transform.forward = velocity;
        }
        string animationName = (!SpeedLessThanAnimationThreshold() ? "Walk" : "Idle");
        if(animationName == "Idle" && m_animator.GetCurrentAnimatorStateInfo(0).IsName("TPose"))
        {
            return;
        }
        m_animator.Play(animationName);
    }

    public void PlayTPose()
    {
        m_animator.Play("TPose");
    }

    private bool SpeedLessThanAnimationThreshold()
    {
        float marginOfError = 0.6f;
        return m_rigidBody.velocity.x < marginOfError && m_rigidBody.velocity.x > -marginOfError
        && m_rigidBody.velocity.z < marginOfError && m_rigidBody.velocity.z > -marginOfError;
    }

    private bool InputKeyDown(InputKeyRelation inputType)
    {
        return Input.GetKey(m_inputButtons[(int)inputType]);
    }

    private bool IsInteracting
    {
        get { return m_interactingObject != null; }
    }

    private GameObject FindObjectsToInteractWith()
    {
        m_overlappedColliders = Physics.OverlapSphere(this.transform.position, m_interactRadius, k_interactableLayerMask);
        if(m_overlappedColliders != null && m_overlappedColliders.Length > 0)
        {
            InteractableObject closest = m_overlappedColliders[0].GetComponent<InteractableObject>();

            for(int i = 1; i< m_overlappedColliders.Length; i++)
            {
                if(Vector3.Distance(closest.transform.position, this.transform.position) 
                > Vector3.Distance(m_overlappedColliders[i].transform.position, this.transform.position) || closest.IsRepairedAlready())
                {
                    InteractableObject interact = m_overlappedColliders[i].GetComponent<InteractableObject>();
                    if(interact != null && !interact.IsRepairedAlready())
                    {
                        closest = interact;
                    }
                }
            }
            return closest.IsRepairedAlready() ? null : closest.gameObject;
        }
        return null;
    }
}