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
        INTERACT
    }

    [SerializeField]
    private float m_playerMovementSpeed = 2f;

    [SerializeField]
    private float m_maxDirectionalSpeed = 5f;

    private Rigidbody m_rigidBody = null;
    private KeyCode[] m_inputButtons = null;

    public void Start()
    {
        m_rigidBody = this.GetComponent<Rigidbody>();
    }

    public void SetInputKeys(KeyCode[] inputButtons)
    {
        m_inputButtons = inputButtons;
    }

    public void UpdateMovement()
    {
        Vector3 velocity = m_rigidBody.velocity;
        bool movingUp = MovementKeyDown(InputKeyRelation.UP);
        bool movingDown = MovementKeyDown(InputKeyRelation.DOWN);

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

        bool movingRight = MovementKeyDown(InputKeyRelation.RIGHT);
        bool movingLeft = MovementKeyDown(InputKeyRelation.LEFT);
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

    private bool MovementKeyDown(InputKeyRelation inputType)
    {
        return Input.GetKey(m_inputButtons[(int)inputType]);
    }
}
