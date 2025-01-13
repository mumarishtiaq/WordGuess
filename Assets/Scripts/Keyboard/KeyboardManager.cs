using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : ManagerBase
{
   [SerializeField] private Button[] _keyboardButtons;
   [SerializeField] private Button _backSpaceBtn;
   [SerializeField] private Button _submitBtn;

    
    public override void ResolveReferences()
    {
        if(_keyboardButtons.Length == 0)
            _keyboardButtons = transform.Find("KeyboardButtons").GetComponentsInChildren<Button>();
    }

    public override void PerformActions()
    {
        foreach(var btn in _keyboardButtons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => GameManager.Instance.OnKeyPress(btn.GetButtonText()));
        }
        _backSpaceBtn.onClick.AddListener(() => GameManager.Instance.OnBackSpace());
        _submitBtn.onClick.AddListener(() => GameManager.Instance.OnSubmit());
    }

    public override void ReInitialize()
    {
        //TODO : code to set two negative keywords on game reinitialize
    }
}
