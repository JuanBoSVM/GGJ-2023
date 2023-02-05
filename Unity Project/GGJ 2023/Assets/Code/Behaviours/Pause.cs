using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    private PlayerControls m_PlayerControls;
    private InputAction m_PauseMenu;

    [SerializeField] private GameObject m_PauseUI;
    [SerializeField] private bool m_IsPaused;

    // Start is called before the first frame update
    void Awake()
    {
        m_PlayerControls = new PlayerControls();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        m_PauseMenu = m_PlayerControls.PauseMenu.Pause;
        m_PauseMenu.Enable();
        m_PauseMenu.performed += Paused;
    }

    private void OnDisable()
    {
        m_PauseMenu.Disable();
    }

    void Paused(InputAction.CallbackContext context)
    {
        m_IsPaused = !m_IsPaused;

        if (m_IsPaused) 
        {
            ActivatePauseMenu();
        }
        else
        {
            DeactivatePauseMenu();
        }
    }

    void ActivatePauseMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        m_PauseUI.SetActive(true);
    }

    public void DeactivatePauseMenu()
    {
        Debug.Log("BUTTON PRESSED");
        Time.timeScale = 1;
        AudioListener.pause = false;
        m_PauseUI.SetActive(false);
        m_IsPaused = false;
    }

    public void Exit()
    {
        Debug.Log("Application quited");
        Application.Quit();
    }

}
