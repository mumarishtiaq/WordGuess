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

    [SerializeField] private Button _closeBtn;
    public UnityEvent OnClose;

    protected virtual void ResolveReferences()
    {
        //get close Button 
        if(!_closeBtn)
            _closeBtn = transform.Find("Close").GetComponent<Button>();
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
        transform.DOScale(0, 0);
        gameObject.SetActive(true);
        transform.DOScale(1, _openDuration).SetEase(_easeType);
    }
    public void Close()
    {
        OnClose?.Invoke();
        transform.DOScale(0, _closeDuration).SetEase(_easeType).
            OnComplete(() =>{ gameObject.SetActive(false);});
    }
}
