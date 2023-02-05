using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuController : MonoBehaviour
{
    private UIDocument m_Doc;
    private Button m_ContinueButton;
    private Button m_ExitButton;

    private void Awake()
    {
        m_Doc = GetComponent<UIDocument>();

        m_ContinueButton = m_Doc.rootVisualElement.Q<Button>("ContinueButton");
        m_ContinueButton.clicked += PlayOnButtonClicked;

        m_ExitButton = m_Doc.rootVisualElement.Q<Button>("ExitButton");
        m_ExitButton.clicked += ExitButtonOnClick;
    }

    void PlayOnButtonClicked()
    {
        SceneManager.LoadScene("Interior_Arbol");
    }

    void ExitButtonOnClick()
    {
        Debug.Log("Application quited");
        Application.Quit();
    }

}
