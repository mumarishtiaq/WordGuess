using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayingAreaManager : ManagerBase
{
    [SerializeField] private Transform _charectersHolder;
    [SerializeField] private List<CharacterEntity> _characters;

    //[Space]
    //[Header("Text Colors")]
    //[SerializeField] private Color _txtPrimaryColor;
    //[SerializeField] private Color _txtSecondaryColor;

    //[Space]
    //[Header("Validation Colors")]
    //[SerializeField] private Color _normalColor;
    //[SerializeField] private Color _missingColor;
    //[SerializeField] private Color _misPlacedColor;
    //[SerializeField] private Color _placedColor;


    [SerializeField] private ColorPalette _palette;



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
    }

    public override void PerformActions()
    {
        ResetCharecters();
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

    public IEnumerator SetFeedback(ValidationType[] feedback, Action onComplete = null)
    {
        for (int i = 0; i < feedback.Length; i++)
        {
            var c = _characters[i];
            var f = feedback[i];


            SetValidator(c, f);
            c.CharTxt.color = _palette.TxtSecondaryColor;
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


