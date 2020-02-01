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
        USE
    }

    [SerializeField]
    private float m_playerMovementSpeed = 2f;

    [SerializeField]
    private float m_maxDirectionalSpeed = 5f;

    [SerializeField]
    private float m_interactRadius = 30.0f;

    private Rigidbody m_rigidBody = null;
    private Collider[] m_overlappedColliders = new Collider[5];
    private KeyCode[] m_inputButtons = null;
    private GameObject m_interactingObject = null;

    public void Start()
    {
        m_rigidBody = this.GetComponent<Rigidbody>();
    }

    public void SetInputKeys(KeyCode[] inputButtons)
    {
        m_inputButtons = inputButtons;
    }

    public void UpdatePlayerControls()
    {
        UpdateInteractions();
        if(!IsInteracting)
        {
            UpdateMovement();
        }
    }

    private void UpdateInteractions()
    {
        if(InputKeyDown(InputKeyRelation.INTERACT))
        {
            if(!IsInteracting)
            {
                m_interactingObject = FindObjectsToInteractWith();
                if(IsInteracting)
                {Debug.Log("ITS YA BOI AND HE HAS A FRIEND!");
                    
                }
            }
            else
            {
                //TODO: Update the interacting with object
            }
        }
        else if (IsInteracting)
        {
            m_interactingObject = null;
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
        int collisions = Physics.OverlapSphereNonAlloc(this.transform.position, m_interactRadius, m_overlappedColliders, 0, QueryTriggerInteraction.Ignore);
        return collisions > 0 ? m_overlappedColliders[0].gameObject : null;
    }
}