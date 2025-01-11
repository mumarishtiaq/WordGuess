using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetButtonAssignerWindow : EditorWindow
{
    private GameObject parentObject;
    private string keyboardLayout = "QWERTYUIOPASDFGHJKLZXCVBNM";

    [MenuItem("Tools/Alphabet Button Assigner")]
    public static void ShowWindow()
    {
        // Create and show the editor window
        GetWindow<AlphabetButtonAssignerWindow>("Alphabet Button Assigner");
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        // Header
        GUILayout.Label("Alphabet Button Assigner", EditorStyles.boldLabel);

        GUILayout.Space(20);
        // Parent GameObject field
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);

        GUILayout.Space(20);
        // Assign Button
        if (GUILayout.Button("Assign Alphabet to Buttons"))
        {
            AssignAlphabetsToButtons();
        }
    }

    private void AssignAlphabetsToButtons()
    {
        // Validate the parent object
        if (parentObject == null)
        {
            Debug.LogError("Parent GameObject is not assigned!");
            return;
        }

        // Get all Button components in children of the parent object
        Button[] buttons = parentObject.GetComponentsInChildren<Button>();

       

        // Assign letters to buttons
        for (int i = 0; i < keyboardLayout.Length; i++)
        {
            string letter = keyboardLayout[i].ToString();
            buttons[i].name = $"Button_{letter}";

            // Assign the text to the button's Text component
            TextMeshProUGUI buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = letter;
            }
            else
            {
                Debug.LogWarning($"Button {buttons[i].name} does not have a Text component to set the text!");
            }
        }

        Debug.Log("Alphabet assigned to buttons successfully.");
    }
}
