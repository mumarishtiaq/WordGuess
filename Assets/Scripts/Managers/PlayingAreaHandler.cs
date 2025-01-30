using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WordGuess;

public class PlayingAreaHandler : HandlerBase
{
    [SerializeField] private Transform _charectersHolder;
    [SerializeField] private List<CharacterEntity> _characters;

    [SerializeField] private Button _submitBtn;
    [SerializeField] private Button _hintBtn;
    [SerializeField] private Button _infoBtn;

    [SerializeField] private Sprite _enableSprite;
    [SerializeField] private Sprite _disableSprite;



    [SerializeField] private ColorPalette _palette;


    [ContextMenu("Resolve References")]
    public override void ResolveReferences()
    {
        if (!_charectersHolder)
            _charectersHolder = transform.Find("CharactersHolders");

        if (_charectersHolder != null && _characters.Count != _charectersHolder.childCount)
        {
            _characters = new List<CharacterEntity>();
            for (int i = 0; i < _charectersHolder.childCount; i++)
            {
                _characters.Add(GetCharecterEntity(_charectersHolder.GetChild(i)));
            }
        }
        if (!_submitBtn)
            _submitBtn = transform.Find("ActionsButtons/SubmitButton").GetComponent<Button>();

        if (!_hintBtn)
            _hintBtn = transform.Find("ActionsButtons/Hint").GetComponentInChildren<Button>();
        
        if (!_infoBtn)
            _infoBtn = transform.Find("ActionsButtons/Info").GetComponentInChildren<Button>();
    }

    public override void PerformActions()
    {
        ResetCharecters();
        _submitBtn.onClick.RemoveAllListeners();
        _submitBtn.onClick.AddListener(() => GameManager.Instance.OnSubmit());
    }
    public override void ReInitialize()
    {
        ResetCharecters();
    }
    private CharacterEntity GetCharecterEntity(Transform t)
    {
        return new CharacterEntity
        {
            CharTxt = t.GetComponentInChildren<TextMeshProUGUI>(),
            Validator = t.GetComponentInChildren<Image>(),
        };
    }

    private void ResetCharecters()
    {
        foreach (var ch in _characters)
        {
            ch.CharTxt.text = string.Empty;
            SetValidator(ch);
            ResetBlink(ch.Validator);
            ch.CharTxt.color = _palette.TxtPrimaryColor;
        }
        Blink(0);
    }

    public void OnInput(string key, int index)
    {
        var ch = _characters[index];
        ch.CharTxt.text = key;

        Blink(Math.Min(index + 1, _characters.Count - 1));
        ResetBlink(_characters[index]?.Validator);
    }

   
    public void OnBackSpace(int index)
    {
        var ch = _characters[index];
        ch.CharTxt.text = string.Empty;
        Blink(index);
        if (index < _characters.Count - 1)
            ResetBlink(_characters[index + 1]?.Validator);
    }
    public string GetGuessedWord()
    {
        return _characters?.Where(c => c.CharTxt != null)
                          .Select(c => c.CharTxt.text)
                          .Aggregate("", (current, next) => current + next) ?? string.Empty;
    }

    public IEnumerator SetFeedback(ValidationType[] feedback, Action onIteration = null, Action onComplete = null)
    {
        for (int i = 0; i < feedback.Length; i++)
        {
            var c = _characters[i];
            var f = feedback[i];


            SetValidator(c, f);
            c.CharTxt.color = _palette.TxtSecondaryColor;
            onIteration?.Invoke();
            yield return new WaitForSeconds(0.3f);

        }
        yield return new WaitForSeconds(0.2f);
        onComplete?.Invoke();
    }

    private void SetValidator(CharacterEntity charecter, ValidationType validate = ValidationType.None)
    {
        bool state = validate != ValidationType.None;
        var color = Color.white;
        var validator = charecter.Validator;

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

        validator.color = color;
        if (state)
        {
            validator.DOKill();
            validator.transform.DOScale(1.3f, 0.3f).
           OnComplete(() =>
           {
               validator.transform.DOScale(1f, 0.3f);
           });
        }
        else
            validator.transform.localScale = Vector3.one;
    }

    public void AnimateFeedback(Transform target, Action OnComplete)
    {
        var charectersToCopy = _charectersHolder.gameObject;

        var copiedCharecters = Instantiate(charectersToCopy, transform);


        copiedCharecters.transform.DOScale(1.05f, 1).
            OnComplete(() =>
            {
                ResetCharecters();
                GameManager.Instance.TriggerSound(SoundType.Feedback);
                var rect = copiedCharecters.GetComponent<RectTransform>();
                var targetRect = target.GetComponent<RectTransform>();

                var localPos = GetTargetPosition(targetRect, rect);

                copiedCharecters.transform.DOScale(0.52f, 1);
                rect.DOLocalMove(localPos, 1).SetEase(Ease.OutBack).
                     OnComplete(() =>
                     {
                         OnComplete?.Invoke();
                         Destroy(copiedCharecters);
                     });

            });
    }

    private Vector2 GetTargetPosition(RectTransform source, RectTransform target)
    {
        Vector3 worldPosition = source.position;
        var localPos = target.parent.InverseTransformPoint(worldPosition);
        return new Vector2(localPos.x , localPos.y);
    }


    private void Blink(int index)
    {
        var blinkObj = _characters[index].Validator;
        ResetBlink(blinkObj);
        blinkObj.DOFade(0.3f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
    }

    private void ResetBlink(Image dash)
    {
        if (dash)
        {
            dash.DOKill();
            dash.DOFade(1, 0);
        }
    }

    public void OnValidateGuessWord(bool isValidLenght, bool isValidWord)
    {
        string txt = isValidLenght ? (isValidWord ? "SUBMIT" : "INVALID") : "SUBMIT";
        Sprite sprite = isValidLenght && isValidWord ? _enableSprite : _disableSprite;

        _submitBtn.SetButtonText(txt);
        _submitBtn.SetSprite(sprite);

        if (isValidLenght && isValidWord)
        {
            _submitBtn.DOKill();
            _submitBtn.transform.DOScale(1.2f, 0.25f).
           OnComplete(() =>
           {
               _submitBtn.transform.DOScale(1f, 0.2f);
           });
        }
    }

    public string testWord = "March";
    [ContextMenu("Hint Test")]
    public void Hint()
    {
        var guessedWord = GetGuessedWord();
        Debug.Log("Guessed Word : " + guessedWord);
        for (int i = 0; i < guessedWord.Length; i++) 
        {
            if (guessedWord[i] != testWord[i] || guessedWord[i] == default)
            {
                Debug.Log($"Hint assigned at index {i}, wrong = {guessedWord[i]}, correct = {testWord[i]}");
                return;
            }
        }
    }
}




[System.Serializable]
public class CharacterEntity
{
    public TextMeshProUGUI CharTxt;
    public Image Validator;
}
public enum ValidationType
{
    None,
    Placed,
    MisPlaced,
    Missing
}




