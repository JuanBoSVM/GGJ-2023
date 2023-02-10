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
        //UpdtGravity();

        // Update the target
        //UpdtFrontTarget();

        // Slide the player down the slope
        Slide();

        // Process user inputs
        //ProcessInputs();

        // Update the rotation
        //UpdateRotation(-m_Gravity);

        // Make the player fall
        //Fall();
    }

    // Debug
    private void OnDrawGizmos()
    {
        Debug.Log("DrawGizmo");

        foreach(Node node in m_Nodes)
        {
            // Get the next node index
            int nextIndex = m_Nodes.IndexOf(node) + 1;

            // Clamp the value
            nextIndex = Mathf.Clamp(nextIndex, 0, m_Nodes.Count - 1);

            // Save the next node position
            Vector3 nextNodePos = m_Nodes[nextIndex].transform.position;

            // Draw lines
            Debug.DrawLine(node.transform.position, nextNodePos, Color.blue);

            Debug.Log("Drawing Line");
        }
    }

    #endregion

    #region Methods

    // Fetch the required components and save a reference to them
    private void Init()
    {
        // Save a reference to the Rigid Body
        m_RigidBd = GetComponent<Rigidbody>();

        // Start targeting at the index 0
        m_TarIndex = 0;

        // Get the first target
        m_FrontTarget = GameObject.Find($"Node {m_TarIndex}");

        // Validate the reference
        if (!m_RigidBd)
        {
            // The reference was invalid
            Debug.LogWarning("Rigid Body Component Missing!");
        }

        // Set the current speed to the base speed
        m_CurrentSpd = m_BaseSpd;

        // Spawn the nodes parent
        m_NodesParent = new GameObject("Nodes");

        // Set it position to our own
        m_NodesParent.transform.position = m_Position;

        // Set it as a child
        m_NodesParent.transform.parent = transform.parent;

        // Create the nodes to snap the player to
        CreateNodes();

        // Start the progress at zero
        m_MoveTime = 0.0f;

        // Adjust the time to rotate to account for all the nodes
        m_RotationSpeed /= m_RayAmount;

        // Start at the middle of the ring
        m_CurrentTarget = m_Nodes.Count / 2;
        m_SideTarget = m_Nodes.Count / 2;
    }

    // Save the direction the player's trying to go to
    private void OnMove(InputValue input)
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

    // Move the player
    private void MoveLateral()
    {
        // Save the progress
        m_MoveProgress = 0.0f;

        // First step, decide where to move
        if (m_MoveTime == 0.0f)
        {
            // Move to the right if there's an available node
            if (m_MoveInput.x < 0.0f)
            {
                m_SideTarget = 
                    ++m_SideTarget >= m_Nodes.Count ? 0 : m_SideTarget;                
            }

            // Move it to the left if there's an available node
            else if (m_MoveInput.x > 0.0f)
            {
                m_SideTarget =
                    --m_SideTarget < 0 ? m_Nodes.Count - 1 : m_SideTarget;
            }
        }

        // Add time to the cumulative
        m_MoveTime += Time.deltaTime;

        // Normalize the progress
        m_MoveProgress = m_MoveTime / m_RotationSpeed;

        // Clamp it
        m_MoveProgress = Mathf.Clamp(m_MoveProgress, 0.0f, 1.0f);

        // Get the vectors for the positions
        Vector3 currentPos = m_Nodes[(int)m_CurrentTarget].transform.position;
        Vector3 nextPos = m_Nodes[(int)m_SideTarget].transform.position;

        // Adjust the position
        m_Position = Vector3.Lerp( currentPos, nextPos, m_MoveProgress);

        // Reset the progress
        if (m_MoveProgress == 1.0f)
        {
            m_MoveProgress = 0.0f;
            m_MoveTime = 0.0f;
            m_CurrentTarget = m_SideTarget;
        }
    }

    // Slide the player down the slope
    private void Slide()
    {
        // Targeted end position
        Vector3 endPos = m_Front * m_CurrentSpd * Time.deltaTime;

        // Move the parent
        transform.parent.transform.position += endPos;
    }

    // Process the user inputs if there are any
    private void ProcessInputs()
    {
        // Validate that there are inputs to process
        if (m_MoveInput.x != 0.0f)
        {
            // The player wants to move
            if (m_MoveInput.x != 0.0f)
            {
                MoveLateral();
            }
        }

        // There was no input, reset the speed
        else
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
        // Get the gravity from the current interpolation
        m_Gravity =
            Vector3.Lerp(
                m_Nodes[(int)m_CurrentTarget].gravity,
                m_Nodes[(int)m_SideTarget].gravity,
                m_MoveProgress);
    }

    // Update the target
    private void UpdtFrontTarget()
    {
        // Save the target position
        Vector3 tarpos = m_FrontTarget.transform.position;

        // Build the target vector
        Vector3 target =
            new Vector3(tarpos.x, m_Position.y, tarpos.z);

        // Compare the distances
        if (Vector3.Distance(target, m_Position) < 20.0f)
        {
            // Change the target
            m_FrontTarget = GameObject.Find($"Node {++m_TarIndex}");
        }
    }

    // Rotate the player to match a specific normal
    private void UpdateRotation(Vector3 normal)
    {
        // Rotate the player
        transform.rotation =
            Quaternion.FromToRotation(Vector3.up, normal);

        // Save the target position
        Vector3 tarpPos = m_FrontTarget.transform.position;

        // Build the target vector
        Vector3 target =
            new Vector3(tarpPos.x, m_Position.y, tarpPos.z) - m_Position;

        // Normalize the vector
        target = target.normalized;
    }

    // Fall towards its own gravity
    private bool Fall()
    {
        // If there's ground below, don't make it fall
        if (Physics.Raycast(m_Position, m_Gravity, m_GrndAtrRadius))
        {
            return false;
        }

        // There was no ground below, make it fall
        else
        {
            // Actual fall
            m_Position += m_Gravity * m_FallSpd * Time.deltaTime;

            return true;
        }
    }

    private void CreateNodes()
    {
        // Angle step for the rotation
        float angStep = 360.0f / m_RayAmount;

        // Create the appropriate amount of nodes
        for (float angle = 0.0f; angle < 360.0f; angle += angStep)
        {
            // Convert the angle to radians
            float angleRad = angle * Mathf.PI / 180.0f;

            // Get the X component
            float x = Mathf.Sin(angleRad);

            // Get the Y component
            float y = Mathf.Cos(angleRad);

            // Calculate the direction to aim the node spawn
            Vector3 target = new Vector3(x, y, 0.0f);

            // Rotate the vector in relation to the player's rotation
            target = transform.rotation * target;

            // Spawn the node
            GameObject node = new GameObject();

            // Set its location
            node.transform.position =
                // It starts at the nodes parent
                m_NodesParent.transform.position
                +
                // It then goes to the center of the ring
                m_Up * m_RingRadius
                +
                // Finally, it goes to its final location
                target * m_RingRadius;

            // Set it as a child of m_NodesParent
            node.transform.parent = m_NodesParent.transform;

            // Add a node component
            node.AddComponent<Node>().
                // Set its gravity to match the target
                gravity = target;

            // Add it to the list
            m_Nodes.Add(node.GetComponent<Node>());

            // Change its name
            node.name = $"Node {m_Nodes.IndexOf(node.GetComponent<Node>())}";
        }
    }

    #endregion

    #region Members

    // Front vector
    private Vector3 m_Front
    {
        get
        {
            return transform.forward;
        }
    }

    // Right vector
    private Vector3 m_Right
    {
        get
        {
            return transform.right;
        }
    }

    // Up vector
    private Vector3 m_Up
    {
        get
        {
            return transform.up;
        }
    }

    // Gravity vector
    private Vector3 m_Gravity = Vector3.down;

    // Player position
    private Vector3 m_Position
    {
        get
        {
            return transform.position;
        }

        set
        {
            transform.position = value;
        }
    }

    // Movement progress
    private float m_MoveTime;

    // Time in seconds it takes to travel to do a full loop through the ring
    [SerializeField]
    private float m_RotationSpeed = 1.0f;

    // Amount of rays for the node ring
    [SerializeField]
    private float m_RayAmount = 60.0f;

    // List of nodes required to rotate around the ring
    private List<Node> m_Nodes = new List<Node>();

    // Parent of the nodes
    private GameObject m_NodesParent;

    // Radius to the center of the ring of nodes
    [SerializeField]
    private float m_RingRadius = 50.0f;

    // Feet position
    [SerializeField]
    private GameObject m_Feet;

    // Targets to look towards
    private GameObject m_FrontTarget;
    private float m_CurrentTarget;
    private float m_SideTarget;

    // Target index
    private int m_TarIndex = 0;

    // Reference to the Rigid Body component
    private Rigidbody m_RigidBd;

    // Physics
    private Vector2 m_MoveInput;
    private float m_MoveProgress;

    // Base speed while sliding
    [SerializeField]
    private float m_BaseSpd = 5.0f;

    // Current speed
    private float m_CurrentSpd;

    // Speed multiplier lower limit
    [SerializeField]
    private float m_MinSpd = 0.5f;

    // Speed multiplier upper limit
    [SerializeField]
    private float m_MaxSpd = 2.0f;

    // Falling Speed
    [SerializeField]
    private float m_FallSpd = 9.81f;

    // Ground attraction distance
    [SerializeField]
    private float m_GrndAtrRadius = 5.0f;

    #endregion
}
