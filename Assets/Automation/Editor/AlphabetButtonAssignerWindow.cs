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

        Button[] buttons = parentObject.GetComponentsInChildren<Button>();

        int layoutIndex = 0; // Index to track the current position in the keyboardLayout

        for (int i = 0; i < buttons.Length; i++)
        {
            // Check if the button should be excluded (e.g., based on a specific tag or name)
            if (buttons[i].CompareTag("NonKeyboardButton")) // Assuming non-keyboard buttons are tagged as "NonKeyboardButton"
            {
                Debug.Log($"Skipping button at index {i} as it's not part of the keyboard layout.");
                continue;
            }

            // Ensure layoutIndex does not exceed the length of the keyboardLayout
            if (layoutIndex >= keyboardLayout.Length)
            {
                Debug.LogWarning("Not enough letters in the keyboard layout to assign to all buttons.");
                break;
            }

            // Assign the letter to the button if it has a TextMeshProUGUI component
            TextMeshProUGUI buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                string letter = keyboardLayout[layoutIndex].ToString();
                buttonText.text = letter;
                buttons[i].name = $"Button_{letter}";

                layoutIndex++; // Increment the layout index only for valid assignments
                var clickPopTxt = buttons[i].transform.Find("OnClickPop")?.GetComponentInChildren<TextMeshProUGUI>();

                clickPopTxt.text = letter;

            }
            else
            {
                Debug.LogWarning($"Button at index {i} does not have a TextMeshProUGUI component and will be skipped.");
            }
        }


        Debug.Log("Alphabet assigned to buttons successfully.");
    }
}
