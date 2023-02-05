using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    private UIDocument m_Doc;
    private Button m_PlayButton;
    private Button m_ExitButton;

    private GameObject m_Player;

    private void Awake()
    {
        m_Doc = GetComponent<UIDocument>();

        m_PlayButton = m_Doc.rootVisualElement.Q<Button>("PlayButton");
        m_PlayButton.clicked += PlayOnButtonClicked;

        m_ExitButton = m_Doc.rootVisualElement.Q<Button>("ExitButton");
        m_ExitButton.clicked += ExitButtonOnClick;

        m_Player = GameObject.Find("Player");
    }

    void PlayOnButtonClicked()
    {
        m_Player.GetComponent<ExitRoom> ().m_IsRotating = true;
    }

    void ExitButtonOnClick()
    {
        Debug.Log("Application quited");
        Application.Quit();
    }

}
