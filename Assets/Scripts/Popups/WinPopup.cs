using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : PopupBase
{
   [SerializeField] private Button _nextBtn;
    [SerializeField] private TextMeshProUGUI _wordTxt;
    private void Reset()
    {
        ResolveReferences();
    }
    private void Awake()
    {
        ResolveReferences();
        AddListners();
    }
    protected override void ResolveReferences()
    {
        base.ResolveReferences();

        //settings references
    }

    protected override void AddListners()
    {
        base.AddListners();

        //adding listners
    }

    public void OperationsOnNext(string word,Action onNext)
    {
        _nextBtn.onClick.RemoveAllListeners();
        _nextBtn.onClick.AddListener(() => OnNext(onNext));

        _wordTxt.SetText(word);
    }

    private void OnNext(Action onNext)
    {
        onNext();
        Close();
    }
}
