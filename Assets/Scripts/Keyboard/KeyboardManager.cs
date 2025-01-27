using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    [SerializeField] private List<KeyBoardButtonEntity> _keyboardButtons;
    [SerializeField] private Button _backSpaceBtn;



    private void Awake()
    {
        ResolveReferences();
        PerformActions();
    }


    [ContextMenu("ResolveReferences")]
    public void ResolveReferences()
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

    public  void PerformActions()
    {
        foreach (var kb in _keyboardButtons)
        {
            kb.Button.onClick.RemoveAllListeners();
            kb.Button.onClick.AddListener(() => StartCoroutine(OnKeyPress(kb)));
        }
        _backSpaceBtn.onClick.AddListener(() => GameManager.Instance.OnCancel());
       


    }

    public void ReInitialize()
    {
        foreach (var btn in _keyboardButtons)
        {
            btn.Reset();
        }
    }

    private IEnumerator OnKeyPress(KeyBoardButtonEntity kb)
    {
        if (!kb.IsNegativeLetter)
        {
            GameManager.Instance.OnInput(kb.Button.GetButtonText());
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

