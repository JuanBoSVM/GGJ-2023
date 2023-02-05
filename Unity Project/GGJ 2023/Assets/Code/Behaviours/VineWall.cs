using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VineWall : MonoBehaviour, IInteractable
{
    #region Flow

    // Fetch the required components
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Try to extend 
        if (!Extend())
        {
            Age();
        }
    }

    #endregion

    #region Methods

    public bool Interact(GameObject otherObject)
    {

        return false;
    }

    // Extend the grab hook
    private bool Extend()
    {
        // Validate that the vine can still extend
        if (m_CurrentLength == m_MaxLength || m_CurrentWidth == m_MaxWidth)
        {
            // The vine wall has reached its limit
            return false;
        }

        // Extend the vine
        m_CurrentLength += m_SecondsToMax * Time.deltaTime;
        m_CurrentWidth += m_SecondsToMax * Time.deltaTime;

        // Clamp the values to the max if they're over it
        m_CurrentLength = Mathf.Min(m_CurrentLength, m_MaxLength);
        m_CurrentWidth = Mathf.Min(m_CurrentWidth, m_MaxWidth);

        gameObject.transform.localScale = Vector3.one * m_CurrentLength;
        
        // The extension was successful
        return true;
    }

    // Make the vine wall age and eventually die
    private void Age()
    {
        // Validate if the vine wall can still age
        if (m_CurrentAge < m_LifeSpan)
        {
            // Age the vine wall
            m_CurrentAge += Time.deltaTime;

            // Clamp the age to its life span
            m_CurrentAge = Mathf.Min(m_CurrentAge, m_LifeSpan);
        }

        // The vine wall has to start dying
        m_DecayProgress += (m_DecayProgress + Time.deltaTime) / m_LifeSpan;

        // If the vine's completely dead, destroy it
        if (m_DecayProgress == 1.0f)
        {
            //DestroyImmediate(gameObject);
            m_CurrentAge = 0;
            m_CurrentLength = 0;
            m_CurrentWidth = 0;
        }

    }

    #endregion

    #region Members

    // Seconds the vine wall extension's will take
    public float m_SecondsToMax;

    // Seconds the vine will take to die
    public float m_SecondsToDecay;

    // Vine wall's life span in seconds
    public float m_LifeSpan;
    
    // Vine wall's length
    public float m_MaxLength;

    // Vine wall's width
    public float m_MaxWidth;

    // Vine wall's current age
    public float m_CurrentAge;

    // Current length
    public float m_CurrentLength;

    // Current Width
    public float m_CurrentWidth;

    // How close the vine wall's from dying [Normalized; 0 = Alive, 1 = Dead]
    public float m_DecayProgress;

    #endregion
}
