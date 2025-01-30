using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupBase : MonoBehaviour
{
    [SerializeField] private float _openDuration = 0.3f;
    [SerializeField] private float _closeDuration = 0.2f;
    [SerializeField] private Ease _easeType = Ease.InOutSine;
    [SerializeField] private Transform _panel;
    [SerializeField] private Image _darkBG;

    [SerializeField] private Button _closeBtn;
    public UnityEvent OnClose;

    protected virtual void ResolveReferences()
    {
        //get close Button 

        if (!_panel)
            _panel = transform.Find("MainPanel");
        
        if (!_darkBG)
            _darkBG = transform.Find("DarkBG")?.GetComponent<Image>();

        if(!_closeBtn)
            _closeBtn = _panel.Find("Close").GetComponent<Button>();
    }
    protected virtual void AddListners()
    { 
        if(_closeBtn)
        {
            _closeBtn.onClick.RemoveAllListeners();
            _closeBtn.onClick.AddListener(Close);
        }
            
    }
    public virtual void OnOpen()
    {
        //setting panel scale and bg alpha to 0
        _panel.DOScale(0, 0);
        _darkBG.DOFade(0, 0);

        gameObject.SetActive(true);

        //setting panel scale and bg alpha to original
        _panel.DOScale(1, _openDuration).SetEase(_easeType);
        _darkBG?.DOFade(0.5f, _openDuration/2);
    }
    public void Close()
    {
        OnClose?.Invoke();
        _panel.DOScale(0, _closeDuration).SetEase(_easeType).
            OnComplete(() =>{ gameObject.SetActive(false);});
        _darkBG?.DOFade(0, _closeDuration/ 2);
    }
}
