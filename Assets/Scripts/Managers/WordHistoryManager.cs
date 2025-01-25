using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordHistoryManager : ManagerBase
{
    public Transform Parent;
    [SerializeField] private GameObject _wordTemplate;
    [SerializeField] private List<List<CharacterEntity>> _historyWords;

    [SerializeField] private ColorPalette _palette;
    public override void ResolveReferences()
    {
        if (!Parent)
            Parent = GameObject.Find("WordsHistory")?.transform;
    }
    public override void PerformActions()
    {
        PopulateWordHistory(WordGuess.WordGuessManager.Instance.TotalTries);
    }
    public override void ReInitialize()
    {
        PopulateWordHistory(WordGuess.WordGuessManager.Instance.TotalTries);
    }

    private void PopulateWordHistory(int numberOfTries)
    {
        if(Parent && _wordTemplate)
        {
            ClearChildren();
            _historyWords = new List<List<CharacterEntity>>();
            for (int i = 0; i < numberOfTries; i++)
            {
                var word = Instantiate(_wordTemplate, Parent);

                var characters = new List<CharacterEntity>();
                for (int j = 0; j < word.transform.childCount; j++)
                {
                    characters.Add(GetCharecterEntity(word.transform.GetChild(j)));
                }
                _historyWords.Add(characters);

            }
        }
    }
    private CharacterEntity GetCharecterEntity(Transform t)
    {
        return new CharacterEntity
        {
            CharTxt = t.GetComponentInChildren<TextMeshProUGUI>(),
            Validator = t.GetComponentInChildren<Image>(),
        };
    }

    public void SetFeedback(List<CharacterEntity> word,string guessedWord, ValidationType[] feedback)
    {
        for (int i = 0; i < feedback.Length; i++)
        {
            var c = word[i];
            var f = feedback[i];
            c.CharTxt.text = guessedWord[i].ToString();
            SetValidator(c, f);
        }
    }

    public void SetFeedback(int currentTry,string guessedWord, ValidationType[] feedback)
    {
        if(currentTry <_historyWords.Count)
        {
            var charecters = _historyWords[currentTry];
            SetFeedback(charecters,guessedWord, feedback);
        }
    }

    private void SetValidator(CharacterEntity charecter, ValidationType validate = ValidationType.None)
    {
        bool state = validate != ValidationType.None;
        var color = Color.white;

        switch (validate)
        {
            case ValidationType.Placed:
                color = _palette.PlacedColor;
                break;
            case ValidationType.MisPlaced:
                color = _palette.MisPlacedColor;
                break;
            case ValidationType.Missing:
                color = _palette.MissingColor;
                break;
            default:
                color = _palette.NormalColor;
                break;
        }


        charecter.Validator.color = color;
        charecter.Validator.gameObject.SetActive(state);
    }
    private void ClearChildren()
    {
        foreach (Transform c in Parent)
        {
            Destroy(c.gameObject);
        }
    }



}
