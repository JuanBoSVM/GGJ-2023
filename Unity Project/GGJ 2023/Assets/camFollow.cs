using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public GameObject m_cam;
    public GameObject m_camRot;
    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_cam.transform.position = transform.position - GetComponent<Rigidbody>().velocity;
        m_camRot.transform.LookAt(transform, Vector3.up);
        //var vel = ;
        
    }
}
