using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour
{
    #region Flow

    // Set the default values
    private void Reset()
    {
        // The whip starts retracted
        m_State = WhipState.RETRACTED;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Select the behaviour based on the current state
        switch (m_State)
        {
            case WhipState.EXTENDING:

                // Try to extend the whip
                if (!Extend())
                {
                    // The whip's fully extended, change its state
                    m_State = WhipState.EXTENDED;
                }

                // End this mode's execution
                break;

            case WhipState.EXTENDED:

                // Try to update the whip's active frames
                if (!UpdateExtendedTime())
                {
                    // Time's up, start retracting
                    m_State = WhipState.RETRACTING;
                }

                // End this mode's execution
                break;

            case WhipState.RETRACTING:

                // Try to retract the whip
                if (!Retract())
                {
                    // The whip's fully retracted, change its state
                    m_State = WhipState.RETRACTED;
                }

                // End this mode's execution
                break;

            // Debug Only. Loop through the states
            case WhipState.RETRACTED:

                // Loop the states
                m_State = WhipState.EXTENDING;

                // End this mode's execution
                break;
        }

        // Log the current length
        Debug.Log(Length);
    }

    #endregion

    #region Methods

    // Extend the whip
    private bool Extend()
    {
        // Verify that the whip has room to extend
        if (Length == m_Params.MaxLenght)
        {
            // The extension failed
            return false;
        }

        // The whip can extend
        else
        {
            // Add the appropriate progress to the whip's length
            Length +=
                m_Params.MaxLenght * NormalizeProgress(Time.deltaTime);

            // Clamp the extension to its max length
            Length =
                Mathf.Min(Length, m_Params.MaxLenght);

            // The extension was successful
            return true;
        }
    }

    // Update the timer for the fully extended time
    private bool UpdateExtendedTime()
    {
        // Add the time passed to the timer
        m_Params.TimeActive += Time.deltaTime;

        // Clamp the duration at the maximum value
        m_Params.TimeActive =
            Mathf.Min(m_Params.TimeActive, m_Params.TimeAtMax);

        // If the time active's up, return false, return true otherwise
        return m_Params.TimeActive == m_Params.TimeAtMax ? false : true;
    }

    // Retract the whip
    private bool Retract()
    {
        // Verify that the whip is still out
        if (Length == 0.0f)
        {
            // The whip's already retracted
            return false;
        }

        // The whip can retract
        else
        {
            // Subtract the appropriate progress to the whip's length
            Length -=
                m_Params.MaxLenght * NormalizeProgress(Time.deltaTime);

            // Clamp the retraction to zero
            Length =
                Mathf.Max(Length, 0.0f);

            // The retraction was successful
            return true;
        }
    }

    // Normalize the time passed in relation with the max time to extension
    private float NormalizeProgress(float timePassed)
    {
        return timePassed / m_Params.TimeToMax;
    }

    #endregion

    #region Members

    // Easy access to the current length
    public float Length
    {
        get
        {
            return m_Params.CurrentLength;
        }

        private set
        {
            m_Params.CurrentLength = value;
        }
    }

    // Current state of the whip
    private WhipState m_State;

    // Whip's parameters
    private WhipParameters m_Params;

    #endregion
}
