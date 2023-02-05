using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExitRoom : MonoBehaviour
{
    private Quaternion m_InitialRotation;
    private Quaternion m_FinalRotation = Quaternion.Euler(0f,136.735f,0f);
    private float m_RotationProgress = 0.0f;

    private Vector3 m_InitialPosition;
    private Vector3 m_FinalPosition;
    private float m_MovementProgress = 0.0f;

    [SerializeField]
    private float m_TimeToRotate = 5.0f;

    [SerializeField]
    private float m_TimeToWalk = 5.0f;

    [SerializeField]
    private float m_Offset = 5.0f;

    public bool m_IsWalking = false;
    public bool m_IsRotating = false;

    // Start is called before the first frame update
    void Start()
    {
        m_InitialPosition = transform.position;
        m_InitialRotation = transform.rotation;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_IsRotating)
        {
            m_RotationProgress += Time.deltaTime;
            m_RotationProgress = Mathf.Min(m_RotationProgress, m_TimeToRotate);
            float tR = m_RotationProgress / m_TimeToRotate;
            transform.rotation = Quaternion.Lerp(m_InitialRotation, m_FinalRotation, tR);

            if (tR == 1.0f)
            {
                m_FinalPosition = m_InitialPosition + transform.forward * m_Offset;

                m_IsRotating = false;
                m_RotationProgress = 0.0f;
                m_IsWalking = true;
            }
        }

        if (m_IsWalking)
        {
            m_MovementProgress += Time.deltaTime;
            m_MovementProgress = Mathf.Min(m_MovementProgress, m_TimeToWalk);
            float t = m_MovementProgress / m_TimeToWalk;
            transform.position = Vector3.Lerp(m_InitialPosition, m_FinalPosition, t);

            if (t == 1.0f)
            {
                m_IsWalking = false;
                m_MovementProgress = 0.0f;
            }
        }
    }
}
