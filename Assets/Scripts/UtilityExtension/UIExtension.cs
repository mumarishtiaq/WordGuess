using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtension
{
    public static void SetButtonText(this Button button, string txt)
    {
        TextMeshProUGUI textComponent = button.GetComponentInChildren<TextMeshProUGUI>();

        if (textComponent != null)
            textComponent.text = txt;
    }

    public static string GetButtonText(this Button button)
    {
        TextMeshProUGUI textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        return textComponent.text;
    }

    public static void SetText(this TextMeshProUGUI textMesh, string txt)
    {
        if (textMesh != null)
            textMesh.text = txt;
    }
}
