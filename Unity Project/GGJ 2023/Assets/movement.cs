using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float m_vel;
    public float m_gravity;
    public Transform m_front;
    public Vector3 m_prevVel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var vel = GetComponent<Rigidbody>().velocity;
        Vector3 newVel = Vector3.zero;//transform.forward;
        if (Input.GetKey(KeyCode.D))
        {
            newVel += transform.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            newVel -= transform.right;
        }
        GetComponent<Rigidbody>().velocity += newVel;
        m_prevVel = newVel;
        
        //GetComponent<Rigidbody>().velocity = new Vector3(m_vel, vel.y, 0);
        //GetComponent<Rigidbody>().velocity = 
        RaycastHit result;
        //Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask);
        if (Physics.Raycast(transform.position, -transform.up,out result, 500.0f, 1, QueryTriggerInteraction.UseGlobal))
        {

            //transform.LookAt(m_front, Vector3.up);
            //Debug.DrawLine(transform.position, result.normal * 100 + transform.position, Color.red, 100);
            transform.rotation = Quaternion.FromToRotation(Vector3.up, result.normal);
            Physics.gravity = result.normal*-9.81f;
            //Debug.DrawLine(transform.position, result.normal * 100 + transform.position, Color.green, 100);
            //var angle = Vector3.(transform.up, result.normal);
        }
        //GetComponent<Rigidbody>().velocity+= new Vector3(m_vel, -m_gravity, 0);
    }
}
