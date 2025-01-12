using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordHistoryManager : ManagerBase
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _wordTemplate;
    [SerializeField] private List<List<CharacterEntity>> _historyWords;

    private PlayingAreaManager _playAreaManager;
    public override void ResolveReferences()
    {
        if (!_parent)
            _parent = GameObject.Find("WordsHistory")?.transform;
    }
    public override void PerformActions()
    {
        PopulateWordHistory(6);
        _playAreaManager = GameManager.Instance.GetManager<PlayingAreaManager>();
    }

    private void PopulateWordHistory(int numberOfTries)
    {
        if(_parent && _wordTemplate)
        {
            ClearChildren();
            _historyWords = new List<List<CharacterEntity>>();
            for (int i = 0; i < numberOfTries; i++)
            {
                var word = Instantiate(_wordTemplate, _parent);
                var numTxt = word.GetComponentInChildren<TextMeshProUGUI>();
                numTxt.text = (i+1) + ".";

                var charectersHolder = word.transform.Find("Charecters");
                var characters = new List<CharacterEntity>();
                for (int j = 0; j < charectersHolder.childCount; j++)
                {
                    characters.Add(GetCharecterEntity(charectersHolder.GetChild(j)));
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
            Dash = t.Find("Dash")?.gameObject,
            Validator = t.GetComponentInChildren<Image>(),
        };
    }

    public void SetFeedback(List<CharacterEntity> word, ValidationType[] feedback)
    {
        for (int i = 0; i < feedback.Length; i++)
        {
            var c = word[i];
            var f = feedback[i];
            SetValidator(c, f);
        }
    }

    public void SetFeedback(int currentTry, ValidationType[] feedback)
    {
        if(currentTry <_historyWords.Count)
        {
            var charecters = _historyWords[currentTry];
            SetFeedback(charecters, feedback);
        }
    }

    private void SetValidator(CharacterEntity charecter, ValidationType validate = ValidationType.None)
    {
        bool state = validate != ValidationType.None;
        var color = Color.white;

        switch (validate)
        {
            case ValidationType.Placed:
                color = Color.green;
                break;
            case ValidationType.MisPlaced:
                color = Color.yellow;
                break;
            case ValidationType.Missing:
                color = Color.red;
                break;
            default:
                break;
        }


        charecter.Validator.color = color;
        charecter.Validator.gameObject.SetActive(state);
    }
    private void ClearChildren()
    {
        foreach (Transform c in _parent)
        {
            Destroy(c.gameObject);
        }
    }



}
