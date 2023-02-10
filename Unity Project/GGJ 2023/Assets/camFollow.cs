using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public GameObject m_toFollow;
    public GameObject m_toLook;
    public Vector3 m_offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_toFollow.transform.position + m_offset;
        transform.LookAt(m_toLook.transform);
        //var vel = ;

    }
}
