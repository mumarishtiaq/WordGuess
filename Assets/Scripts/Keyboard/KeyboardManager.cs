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
    [SerializeField] private Button _backSpaceBtn;
   





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

       



    }

    public override void PerformActions()
    {
        foreach (var kb in _keyboardButtons)
        {
            kb.Button.onClick.RemoveAllListeners();
            kb.Button.onClick.AddListener(() => StartCoroutine(OnKeyPress(kb)));
        }
        _backSpaceBtn.onClick.AddListener(() => WordGuess.GameManager.Instance.OnBackSpace());
       


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

