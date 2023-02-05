using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

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
        // Update the gravity
        UpdtGravity();

        // Slide the player down the slope
        Slide();

        // Update the target
        UpdtTarget();

        // Process user inputs
        ProcessInputs();

        // Make the player fall
        Fall();
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

        // Start targeting at the index 0
        m_TarIndex = 0;

        // Get the first target
        m_Target = GameObject.Find($"Node {m_TarIndex}");

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

        // Set the current speed to the base speed
        m_CurrentSpd = m_BaseSpd;
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
            m_CurrentSpd = m_BaseSpd * -m_MoveInput.y * m_MinSpd;
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

        // Amount to move the player upwards
        Vector3 moveAmnt = m_Up * m_JmpStr * m_CurrentSpd * Time.deltaTime;

        // Move the player up
        m_CharCtr.Move(moveAmnt);
    }

    // Slide the player down the slope
    private void Slide()
    {
        // Targeted end position
        Vector3 endPos = Vector3.zero;

        // Push the player forward
        endPos += m_Front * m_CurrentSpd * Time.deltaTime;

        // Save the changes
        m_CharCtr.Move(endPos);
    }

    // Process the user inputs if there are any
    private void ProcessInputs()
    {
        // Validate that there are inputs to process
        if (m_MoveInput.x != 0.0f || m_WantsToJmp != false)
        {
            // The player wants to move
            if (m_MoveInput.x != 0.0f)
            {
                MoveLateral();

                // Reset the variables
            }

            // The player wants to jump
            if (m_WantsToJmp)
            {
                Jump();

                // Reset the variables
                m_WantsToJmp = false;
            }
        }

        // There was no input, reset the speed
        else if (m_MoveInput == Vector2.zero && !m_WantsToJmp)
        {
            m_CurrentSpd = m_BaseSpd;
        }
    }

    // Calculate if the player's upside down
    private bool IsUpsideDown()
    {
        return m_Up.y < 0.0f ? true : false;
    }

    // Calculate the gravity direction
    private void UpdtGravity()
    {
        // Variable to store the current closest direction
        RaycastHit closestDir = new RaycastHit();

        // Results of the raycast if it hit anything
        RaycastHit hitRes = new RaycastHit();

        // How many rays to cast
        float rayAmmnt = 20.0f;

        // Angle step for the rotation
        float angStep = 180.0f / rayAmmnt;

        // Winning target, for debugging porpoises
        Vector3 winTarget = Vector3.zero;

        // Cast rays around the player to look for the closest ground
        for (float angle = 90.0f; angle < 270.0f; angle += angStep)
        {
            // Convert the angle to radians
            float angleRad = angle * Mathf.PI / 180.0f;

            // Get the X component
            float x = Mathf.Sin(angleRad);

            // Get the Y component
            float y = Mathf.Cos(angleRad);

            // Save the rotation from the player
            Quaternion rotation = gameObject.transform.rotation;

            // Calculate the direction to aim the ray cast to
            Vector3 target = new Vector3(x, y, 0.0f);

            // Rotate the vector in relation to the player's rotation
            target = rotation * target;

            // Cast a ray in the given direction
            if (Physics.Raycast(m_Position, target, out hitRes, m_GrndAtrRadius))
            {
                // Compare the distances
                if (
                    closestDir.distance > hitRes.distance
                    ||
                    closestDir.distance == 0.0f)
                {
                    // The newly-found point is closer, replace the old one
                    closestDir = hitRes;

                    // Save the current target as a winner
                    winTarget = target;
                }
            }
        }

        // If there wasn't a nearby object detected, go straight down
        if (closestDir.normal == Vector3.zero)
        {
            // If there wasn't a nearby object detected, go straight down
            m_Gravity = Vector3.down;
        }

        else
        {
            // Go to the closest surface
            m_Gravity = -closestDir.normal;
        }
    }

    // Update the target
    private void UpdtTarget()
    {
        // Save the target position
        Vector3 tarpos = m_Target.transform.position;

        // Build the target vector
        Vector3 target =
            new Vector3(tarpos.x, m_Position.y, tarpos.z);

        // Compare the distances
        if (Vector3.Distance(target, m_Position) < 20.0f)
        {
            // Change the target
            m_Target = GameObject.Find($"Node {++m_TarIndex}");
        }
    }

    // Rotate the player to match a specific normal
    private void RotateWithVector(Vector3 normal)
    {
        // Rotate the player
        gameObject.transform.rotation = 
            Quaternion.FromToRotation(Vector3.up, normal);

        // Save the target position
        Vector3 tarpos = m_Target.transform.position;

        // Build the target vector
        Vector3 target =
            new Vector3(tarpos.x, m_Position.y, tarpos.z) - m_Position;

        // Normalize the vector
        target = target.normalized;

        // Rotate towards the next target
        gameObject.transform.rotation *=
            Quaternion.FromToRotation(
                Vector3.forward,
                target);
    }

    // Fall towards its own gravity
    private bool Fall()
    {
        // Results of the raycasts if they hit anything
        RaycastHit hitRes;
        RaycastHit groundDist;

        // Stick it to the center of the tube if there's ground
        if (Physics.Raycast(m_Position, m_Gravity, out hitRes, m_GrndAtrRadius))
        {
            // Verify if the player has the right rotation
            if (m_Up != hitRes.normal)
            {
                // Rotate to match the ground normal
                RotateWithVector(hitRes.normal);
            }

            // If it's already on the ground, stop pulling
            if (
                Physics.Raycast(
                    m_Feet.transform.position,
                    m_Gravity,
                    out groundDist,
                    m_GrndAtrRadius
                    ))
            {
                // Compare the distance
                if (groundDist.distance < 2.0f)
                {
                    // End the process
                    return false;
                }
            }

            // Stick it to the ground
            m_CharCtr.Move(m_Gravity * hitRes.distance);
        }

        // There was no ground below, rotate it upright
        else
        {
            // Rotate to be upright
            RotateWithVector(Vector3.up);
        }

        // Move the player downwards
        m_CharCtr.Move(m_Gravity * m_FallSpd * Time.deltaTime);

        // Fell successfully
        return true;
    }

    #endregion

    #region Members

    // Front vector
    private Vector3 m_Front
    {
        get
        {
            return gameObject.transform.forward;
        }
    }

    // Right vector
    private Vector3 m_Right
    {
        get
        {
            return gameObject.transform.right;
        }
    }

    // Up vector
    private Vector3 m_Up
    {
        get
        {
            return gameObject.transform.up;
        }
    }

    // Gravity vector
    private Vector3 m_Gravity = Vector3.down;

    // Player position
    private Vector3 m_Position
    {
        get
        {
            return gameObject.transform.position;
        }
    }

    // Feet position
    [SerializeField]
    private GameObject m_Feet;

    // Target to look towards
    private GameObject m_Target;

    // Target index
    private int m_TarIndex = 0;

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
    private float m_JmpStr = 20.0f;

    // Falling Speed
    [SerializeField]
    private float m_FallSpd = 9.81f;

    // Ground attraction distance
    [SerializeField]
    private float m_GrndAtrRadius = 5.0f;

    #endregion
}
