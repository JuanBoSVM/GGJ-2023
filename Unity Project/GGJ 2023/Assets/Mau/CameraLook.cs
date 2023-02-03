using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float cameraSpeed = 10f;
    float Yaw = 0f;
    float pitch = 0f;
    float XRotation = 0f;
    [SerializeField] Transform m_playerBody;
    //[SerializeField] Transform m_weapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Yaw = Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime;
        pitch = Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime;
        XRotation -= pitch;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(XRotation, Yaw, 0f);
        m_playerBody.Rotate(Vector3.up * Yaw);
        //m_weapon.Rotate(Vector3.up * Yaw);
    }
}
