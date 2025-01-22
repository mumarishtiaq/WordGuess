using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "Settings/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    [Space]
    [Header("Text Colors")]
    public Color TxtPrimaryColor;
    public Color TxtSecondaryColor;

    [Space]
    [Header("Validation Colors")]
    public Color NormalColor;
    public Color MissingColor;
    public Color MisPlacedColor;
    public Color PlacedColor;
}
