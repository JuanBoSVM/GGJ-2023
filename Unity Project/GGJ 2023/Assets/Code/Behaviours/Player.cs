using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public partial class Player : MonoBehaviour
{
    // Player Variables

    private CharacterController m_CharacterController;

    private Vector3 m_PlayerVelocity;
    private bool m_GroundedPlayer;
    
    private float m_Speed = 5.0f;
    private float m_JumpHeight = 1.0f;
    private float m_Gravity = -9.81f;

    private Vector2 m_PlayerMovementInput;

    private bool m_HasJumped;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player created");
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        MovePlayer();
        if (m_HasJumped)
        {
            Jumped();
        }
        else
        {

        m_PlayerVelocity.y += m_Gravity * Time.deltaTime;
        }
    }

    void MovePlayer()
    {
        Vector3 movement = new Vector3(m_PlayerMovementInput.x, 0.0f, m_PlayerMovementInput.y);

        m_CharacterController.SimpleMove(movement * m_Speed);

    }

    void OnMove(InputValue iv)
    {
        Debug.Log("Movement Pressed");
        m_PlayerMovementInput = iv.Get<Vector2>();
    }

    void OnJump(InputValue iv)
    {



        m_HasJumped = true;
    }

    void Jumped()
    {
        Debug.Log("Jumping");

        m_GroundedPlayer = m_CharacterController.isGrounded;

        // Resets velocity if still
        if (!m_GroundedPlayer)
        {
            m_PlayerVelocity.y += Mathf.Sqrt(m_JumpHeight * -3.0f * m_Gravity);
            m_CharacterController.Move(m_PlayerVelocity * Time.deltaTime * m_Speed);

            m_PlayerVelocity = Vector3.zero;

            Debug.Log(m_PlayerVelocity.y);


        }
        else
        {
            m_HasJumped = false;

        }
    }
}
