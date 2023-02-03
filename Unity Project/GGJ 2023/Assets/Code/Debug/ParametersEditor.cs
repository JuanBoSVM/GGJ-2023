/*
#if UNITY_EDITOR 

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ParametersEditor : EditorWindow
{
    #region Flow

    // Create the menu option to open the window
    [MenuItem("Debug/Parameters")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<ParametersEditor>();
    }

    // Set the default values
    private void OnEnable()
    {
        Init();
    }

    #endregion

    #region Methods

    // Initialize with the default values
    private void Init()
    {
        // Set default values
        m_WhipParams.MaxLenght = 5.0f;


    }

    #endregion

    #region Members

    private WhipParameters m_WhipParams;

    #endregion

}

#endif*/