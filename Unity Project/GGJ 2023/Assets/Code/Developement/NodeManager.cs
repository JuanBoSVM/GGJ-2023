#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeManager : EditorWindow
{
    #region Flow

    // Create the menu option to open the window
    [MenuItem("Debug/Node Manager")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<NodeManager>();
    }

    // Create the buttons
    private void OnGUI()
    {
        // Define how many nodes to create
        m_NodeQuantity =
            EditorGUILayout.IntField(
                "Quantity of nodes",
                m_NodeQuantity);

        // Create the nodes
        if (GUILayout.Button("Create Nodes"))
        {
            CreateNodes(m_NodeQuantity);
        }

        // Delete all the nodes
        if (GUILayout.Button("Delete all the nodes"))
        {
            DestroyAll();
            RenameNodes();
        }

        // Destroy only the selected node
        if (GUILayout.Button("Delete the selected node"))
        {
            DestroySelected();
            RenameNodes();
        }
    }

    #endregion

    #region Methods

    // Create the selected amount of nodes
    private void CreateNodes(int amount)
    {
        // Parent of the nodes
        GameObject pathNodes = GameObject.Find("Path Nodes");

        // Check if there's a parent for the nodes already
        if (pathNodes == null)
        {
            // Create a parent for the nodes
            pathNodes = new GameObject("Path Nodes");
        }

        // Create the nodes
        for (int i = 0; i < amount; i++)
        {
            // Create the new node
            GameObject node = new GameObject();

            // Add it to the list
            m_Nodes.Add(node);

            // Change its name
            node.name = $"Node {m_Nodes.IndexOf(node)}";

            // Set its parent
            node.transform.parent = pathNodes.transform;
        }
    }

    // Destroy the selected node
    private void DestroySelected()
    {
        // Get the selected node
        GameObject[] nodes = Selection.gameObjects;

        // Loop through all the selected game objects
        foreach (GameObject node in nodes)
        {
            // Make sure its a node on the list
            if (m_Nodes.Contains(node))
            {
                // Take it out from the list
                m_Nodes.RemoveAt(m_Nodes.IndexOf(node));

                // Destroy it
                DestroyImmediate(node);
            }
        }
    }

    // Rename the nodes to their ID number
    private void RenameNodes()
    {
        // Loop through the nodes
        foreach (GameObject node in m_Nodes)
        {
            // Rename it to match its index
            node.name = $"Node {m_Nodes.IndexOf(node)}";
        }
    }

    // Destroy every node on the list
    private void DestroyAll()
    {
        // Loop through the list of nodes
        foreach (GameObject node in m_Nodes)
        {
            // Destroy the current node
            DestroyImmediate(node);
        }

        // Clear the list
        m_Nodes.Clear();
    }

    #endregion

    #region Members

    // List with all the nodes
    private List<GameObject> m_Nodes = new List<GameObject>();

    // Amount of nodes to create
    private int m_NodeQuantity;

    #endregion

}

#endif