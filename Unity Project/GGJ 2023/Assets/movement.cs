using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float m_vel;
    public float m_velSides;
    public float m_changeTime;
    public Vector3 m_prevVel;
    public Quaternion m_prevUp;
    public Quaternion m_newUp;
    public Transform m_front;

    float m_timeChanging;

    // Start is called before the first frame update
    void Start()
    {
        m_timeChanging = m_changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        var vel = GetComponent<Rigidbody>().velocity;
        Vector3 newVel = Vector3.zero;
        if (Input.GetKey(KeyCode.D))
        {
            newVel -= transform.right* m_velSides;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            newVel += transform.right * m_velSides;
        }
        GetComponent<Rigidbody>().velocity += newVel;
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, -m_vel);
        m_prevVel = newVel;
        
        RaycastHit result;
        if (Physics.Raycast(transform.position, -transform.up,out result, 500.0f, 1, QueryTriggerInteraction.UseGlobal))
        {
            var newFront = Vector3.Cross(result.normal, transform.forward);
            transform.rotation = Quaternion.FromToRotation(Vector3.up, result.normal);

            m_front.transform.localPosition = newFront;
            Physics.gravity = result.normal*-9.81f;

            //m_newUp = Quaternion.FromToRotation(Vector3.up, result.normal);
            //m_timeChanging = 0;
        }
        //m_timeChanging += Time.deltaTime;
        //if (m_timeChanging < m_changeTime)
        //{
        //    //Debug.Log(m_timeChanging / m_changeTime);
        //    transform.rotation = Quaternion.Lerp(m_prevUp, m_newUp, 0.99f);
        //}
        //else
        //{
        //    Debug.Log("changed up");
        //    m_prevUp = m_newUp;
        //}

    }
}
