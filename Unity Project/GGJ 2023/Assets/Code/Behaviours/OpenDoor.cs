using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Vector3 m_InitialPosition;
    private Vector3 m_FinalPosition;
    private float m_MovementProgress = 0.0f;

    [SerializeField]
    private float m_TimeToOpen = 3.0f;

    [SerializeField]
    private float m_Offset = 3.0f;

    private bool m_IsOpening = false;

    // Start is called before the first frame update
    void Start()
    {
        m_InitialPosition = transform.position;
        m_FinalPosition = m_InitialPosition + Vector3.up * m_Offset;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_IsOpening)
        {
            m_MovementProgress += Time.deltaTime;
            m_MovementProgress = Mathf.Min(m_MovementProgress, m_TimeToOpen);
            float t = m_MovementProgress / m_TimeToOpen;
            transform.position = Vector3.Lerp(m_InitialPosition, m_FinalPosition, t);

            if (t == 1.0f)
            {
                m_IsOpening = false;
                m_MovementProgress = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        m_IsOpening = true;
    }
}
