using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public partial class Player : MonoBehaviour
{
    #region Flow

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the player
        Init();
    }


    // Update the physics
    void FixedUpdate()
    {
        // Update the directional vectors
        UpdateDirVectors();

        // Slide the player down the slope
        Slide();

        // Process user inputs
        ProcessInputs();
    }

    #endregion

    #region Methods

    // Fetch the required components and save a reference to them
    private void Init()
    {
        // Save a reference to the Rigid Body
        m_RigidBd = GetComponent<Rigidbody>();

        // Save a reference to the Character Controller
        m_CharCtr = GetComponent<CharacterController>();

        // Validate the reference
        if (!m_RigidBd)
        {
            // The reference was invalid
            Debug.LogWarning("Rigid Body Component Missing!");
        }

        // Validate the reference
        if (!m_CharCtr)
        {
            // The reference was invalid
            Debug.LogWarning("Character Controller Component Missing!");
        }

        // Set the directional vectors
        UpdateDirVectors();

        // Set the current speed to the base speed
        m_CurrentSpd = m_BaseSpd;
    }

    // Update the directional vectors based on the player rotation
    private void UpdateDirVectors()
    {
        // Rotate the player's front vector
        m_Front = gameObject.transform.rotation * Vector3.forward;

        // Rotate the player's right vector
        m_Right = gameObject.transform.rotation * Vector3.right;
    }

    // Save the direction the player's trying to go to
    void OnMove(InputValue input)
    {
        // Save the current input
        m_MoveInput = input.Get<Vector2>();

        // Translate the input into speeding up
        if (m_MoveInput.y > 0.0f)
        {
            m_CurrentSpd = m_BaseSpd * m_MoveInput.y * m_MaxSpd;
        }

        else if (m_MoveInput.y < 0.0f)
        {
            m_CurrentSpd = m_BaseSpd * - m_MoveInput.y * m_MinSpd;
        }
    }

    // Save the intention to jump
    void OnJump(InputValue input)
    {
        // Match the variable to the pressed button
        m_WantsToJmp = input.isPressed;
    }

    // Move the player
    void MoveLateral()
    {
        /*// Check if the player's upside down
        if (IsUpsideDown())
        {
            // Invert the x axis
            m_MoveInput.x = -m_MoveInput.x;
        }*/

        // Move to the right
        if (m_MoveInput.x > 0.0f)
        {
            m_CharCtr.Move(m_Right * m_CurrentSpd * Time.deltaTime);
        }

        // Move it to the left
        else if (m_MoveInput.x < 0.0f)
        {
            m_CharCtr.Move(-m_Right * m_CurrentSpd * Time.deltaTime);
        }
    }

    // Make the player jump
    void Jump()
    {
        // Adjust the speed while airborne
        m_CurrentSpd *= m_AirSpdMulti;
    }

    // Slide the player down the slope
    private void Slide()
    {
        // Targeted end position
        Vector3 endPos = Vector3.zero;

        // Push the player forward
        endPos += m_Front * m_CurrentSpd * Time.deltaTime;

        // Pull the player towards the closest direction
        endPos += GravityDir() * m_FallSpd * Time.deltaTime;

        // Save the changes
        m_CharCtr.Move(endPos);
    }

    // Process the user inputs if there are any
    private void ProcessInputs()
    {
        // Validate that there are inputs to process
        if (m_MoveInput != Vector2.zero || m_WantsToJmp != false)
        {
            // The player wants to move
            if (m_MoveInput != Vector2.zero)
            {
                MoveLateral();
            }

            // The player wants to jump
            if (m_WantsToJmp)
            {
                Jump();
            }
        }

        // There was no input, reset the variables
        else
        {
            m_MoveInput = Vector2.zero;
            m_WantsToJmp = false;
            m_CurrentSpd = m_BaseSpd;
        }
    }

    // Calculate if the player's upside down
    private bool IsUpsideDown()
    {
        /*// Compare how similar the up vector and the gravity are
        if ()
        {
        }*/

        return true;
    }

    // Calculate the gravity direction
    private Vector3 GravityDir()
    {
        // Variable to store the current closest direction
        Vector3 closestDir = Vector3.zero;

        // Results of the raycast if it hit anything
        RaycastHit hitRes;

        // How many rays to cast
        int rayAmmnt = 20;

        // Angle step for the rotation
        int angStep = 360 / rayAmmnt;

        // Cast rays around the player to look for the closest ground
        for (int angle = 0; angle < 360; angle += angStep)
        {
            // Convert the angle to radians
            float angleRad = angle * Mathf.PI / 180.0f;

            // Get the X component
            float x = Mathf.Cos(angleRad);

            // Get the Y component
            float y = Mathf.Sin(angleRad);

            // Save the origin from the player
            Vector3 origin = gameObject.transform.position;

            // Save the rotation from the player
            Quaternion rotation = gameObject.transform.rotation;

            // Calculate the direction to aim the ray cast to
            Vector3 target = new Vector3(x, y, 0.0f);

            // Rotate the vector in relation to the player's rotation
            target = rotation * target;

            // Cast a ray in the given direction
            if (Physics.Raycast(origin, target, out hitRes, m_GrndAtrRadius))
            {
                // Compare the magnitude with the current closest point
                if (closestDir.magnitude < hitRes.distance)
                {
                    // The newly-found point is closer, replace the old one
                    closestDir = -hitRes.normal;
                }

                // Visualize the raycast
                Debug.DrawLine(origin, origin + target * m_GrndAtrRadius, Color.red);
            }

            else
            {
                Debug.DrawLine(origin, origin + target * m_GrndAtrRadius, Color.blue);
            }
        }

        // If there wasn't a nearby object detected, go straight down
        return closestDir == Vector3.zero ? -Vector3.up : closestDir;
    }

    #endregion

    #region Members

    // Front vector
    private Vector3 m_Front;

    // Right vector
    private Vector3 m_Right;

    // Closest ground
    private Vector3 m_Ground;

    // Reference to the Rigid Body component
    private Rigidbody m_RigidBd;

    // Physics
    private CharacterController m_CharCtr;
    private Vector2 m_MoveInput;
    private bool m_WantsToJmp;

    // Base speed while sliding
    [SerializeField]
    private float m_BaseSpd = 5.0f;

    // Speed multiplier while airborne
    [SerializeField]
    private float m_AirSpdMulti = 1.0f;

    // Current speed
    private float m_CurrentSpd;

    // Speed multiplier lower limit
    [SerializeField]
    private float m_MinSpd = 0.5f;

    // Speed multiplier upper limit
    [SerializeField]
    private float m_MaxSpd = 2.0f;

    // Jump strength
    [SerializeField]
    private float m_JmpStr = 1.0f;

    // Falling Speed
    [SerializeField]
    private float m_FallSpd = 9.81f;

    // Ground attraction distance
    [SerializeField]
    private float m_GrndAtrRadius = 5.0f;

    #endregion
}
