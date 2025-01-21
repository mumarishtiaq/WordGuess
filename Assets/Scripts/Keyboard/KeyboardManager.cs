using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : ManagerBase
{
    [SerializeField] private List<KeyBoardButtonEntity> _keyboardButtons;
    [SerializeField] private Button _hintBtn;
    [SerializeField] private Button _backSpaceBtn;
    [SerializeField] private Button _submitBtn;

    [SerializeField] private Sprite _enableSprite;
    [SerializeField] private Sprite _disableSprite;





    [ContextMenu("ResolveReferences")]
    public override void ResolveReferences()
    {
        if (_keyboardButtons.Count == 0)
        {
            var allBtns = transform.Find("KeyboardButtons").GetComponentsInChildren<Button>().ToList();

            _keyboardButtons = new List<KeyBoardButtonEntity>();

            foreach (Button btn in allBtns)
            {
                if (btn.name.ToLower().Contains("backspace"))
                    _backSpaceBtn = btn;

                else if (btn.name.ToLower().Contains("hint"))
                    _hintBtn = btn;

                else
                {
                    var keyboardBtn = new KeyBoardButtonEntity
                    {
                        Button = btn,
                        Fill = btn.transform.Find("Fill").gameObject,
                        Clickpop = btn.transform.Find("OnClickPop").gameObject,
                        NegativeIcon = btn.transform.Find("NegativeIcon").gameObject

                    };

                    _keyboardButtons.Add(keyboardBtn);
                    keyboardBtn.Reset();

                }


            }
        }

        if (!_submitBtn)
            _submitBtn = transform.Find("SubmitButton").GetComponent<Button>();



    }

    public override void PerformActions()
    {
        foreach (var kb in _keyboardButtons)
        {
            kb.Button.onClick.RemoveAllListeners();
            kb.Button.onClick.AddListener(() => StartCoroutine(OnKeyPress(kb)));
        }
        _backSpaceBtn.onClick.AddListener(() => WordGuess.GameManager.Instance.OnBackSpace());
        _submitBtn.onClick.AddListener(() => WordGuess.GameManager.Instance.OnSubmit());


    }

    public override void ReInitialize()
    {
        //TODO : code to set two negative keywords on game reinitialize
    }

    private IEnumerator OnKeyPress(KeyBoardButtonEntity kb)
    {
        if (!kb.IsNegativeLetter)
        {
            WordGuess.GameManager.Instance.OnKeyPress(kb.Button.GetButtonText());
            kb.OperationsOnClick(true);
            yield return new WaitForSeconds(0.2f);
            kb.OperationsOnClick();
        }
    }

    public void OnValidateGuessWord(bool isValidLenght, bool isValidWord)
    {
        string txt = isValidLenght ? (isValidWord ? "Submit" : "Invalid"): "Submit";
        Sprite sprite = isValidLenght && isValidWord ? _enableSprite : _disableSprite;

        _submitBtn.SetButtonText(txt);
        _submitBtn.SetSprite(sprite);

        if(isValidLenght && isValidWord)
        {
            _submitBtn.DOKill();
            _submitBtn.transform.DOScale(1.2f, 0.25f).
           OnComplete(() =>
           {
               _submitBtn.transform.DOScale(1f, 0.2f);
           });
        }
    }

}

[System.Serializable]
public class KeyBoardButtonEntity
{
    public Button Button;
    public GameObject Fill;
    public GameObject Clickpop;
    public GameObject NegativeIcon;

    private bool _isNegativeLetter = false;
    public bool IsNegativeLetter
    {
        get => _isNegativeLetter;
        set
        {
            NegativeIcon.SetActive(value);
            _isNegativeLetter = value;
        }
    }

    public void Reset()
    {
        Fill.SetActive(false);
        Clickpop.SetActive(false);
        IsNegativeLetter = false;
    }

    public void OperationsOnClick(bool status = false)
    {
        Fill.SetActive(status);
        Clickpop.SetActive(status);
    }
}

