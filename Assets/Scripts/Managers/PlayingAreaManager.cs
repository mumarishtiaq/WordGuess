using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayingAreaManager : ManagerBase
{
    [SerializeField] private List<CharacterEntity> _characters;
    [SerializeField] private Color _missingColor;
    [SerializeField] private Color _placedColor;
    [SerializeField] private Color _misPlacedColor;

    public override void ResolveReferences()
    {
        var charectersHolder = transform.Find("CharactersHolders");
        if(charectersHolder !=null && _characters.Count != charectersHolder.childCount)
        {
            _characters = new List<CharacterEntity>();
            for (int i = 0; i < charectersHolder.childCount; i++)
            {
                _characters.Add(GetCharecterEntity(charectersHolder.GetChild(i)));
            }
        }
    }

    public override void PerformActions()
    {
        ResetCharecters();
    }
    private CharacterEntity GetCharecterEntity(Transform t)
    {
        return new CharacterEntity
        {
            CharTxt = t.GetComponentInChildren<TextMeshProUGUI>(),
            Dash = t.Find("Dash")?.gameObject,
            Validator = t.GetComponentInChildren<Image>(),
        };
    }

    [ContextMenu("Reset")]
    public void ResetCharecters()
    {
        foreach (var ch in _characters)
        {
            ch.CharTxt.text = string.Empty;
            ch.Dash.SetActive(true);
            SetValidator(ch);
        }
    }

    public void OnInput(string key, int index)
    {
        var ch = _characters[index];
        ch.CharTxt.text = key;
        ch.Dash.SetActive(false);
    }
    public void OnBackSpace(int index)
    {
        var ch = _characters[index];
        ch.CharTxt.text = string.Empty;
        ch.Dash.SetActive(true);
    }

    public string GetGuessedWord()
    {
        return _characters?.Where(c => c.CharTxt != null)
                          .Select(c => c.CharTxt.text)
                          .Aggregate("", (current, next) => current + next) ?? string.Empty;
    }

    public void SetFeedback(ValidationType[] feedback)
    {
        for (int i = 0; i < feedback.Length; i++)
        {
            var c = _characters[i];
            var f = feedback[i];


            SetValidator(c, f);
        }
    }

    private void SetValidator(CharacterEntity charecter,ValidationType validate = ValidationType.None)
    {
        bool state = validate != ValidationType.None;
        var color  = Color.white;

        switch (validate)
        {
            case ValidationType.Placed:
                color = _placedColor;
                break;
            case ValidationType.MisPlaced:
                color = _misPlacedColor;
                break;
            case ValidationType.Missing:
                color = _missingColor;
                break;
            default:
                break;
        }

        
        charecter.Validator.color = color;
        charecter.Validator.gameObject.SetActive(state);
    }


    
}



[System.Serializable]
public class CharacterEntity
{
    public TextMeshProUGUI CharTxt;
    public GameObject Dash;
    public Image Validator;
}
public enum ValidationType
{
    None,
    Placed,
    MisPlaced,
    Missing
}


