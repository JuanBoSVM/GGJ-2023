using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float walkSpeed = 10f;
    Rigidbody m_rigidbody;
    InputManager m_input;
    
    //public Vector3 rotate = new Vector3(1f,0f,0f); 
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_input = SystemManager.m_instance.m_inputs;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 movementInput = m_playerInput.actions["Movement"].ReadValue<Vector2>();
        Moving();
        
    }
    void Moving()
    {
        Vector3 playerVelocity = new Vector3(m_input.m_movementInput.x * walkSpeed, m_rigidbody.velocity.y, m_input.m_movementInput.y * walkSpeed);
        m_rigidbody.velocity = transform.TransformDirection(playerVelocity);
    }
}
