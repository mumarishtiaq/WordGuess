using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WordGuess;

[RequireComponent(typeof(Button))]
public class InteractiveButton : MonoBehaviour
{
    [SerializeField] private AnimationType _animationType;
    [SerializeField] private bool _isPlaySound = true;
   private Button _button;
    private RectTransform _icon;
    private float duration = 0.5f;
    private float _scaleFactor = 1.1f;

    private void Reset()
    {
        ResolveReferences();
    }
    private void Awake()
    {
        ResolveReferences();

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(Animate);

        KillTweens();
    }

    private void ResolveReferences()
    {
        if(!_button)
            _button = GetComponent<Button>();
        
        if(!_icon)
            _icon = transform.Find("Icon").GetComponent<RectTransform>();

    }

    private void Animate()
    {
        if (_isPlaySound)
            GameManager.Instance.TriggerSound(SoundType.DefaultButton);

       KillTweens();
        switch (_animationType)
        {
            case AnimationType.Scale:
                Scale();
                break;

            case AnimationType.Rotate:
                Rotate();
                break;
            
            case AnimationType.ScaleAndRotate:
                Scale();
                Rotate();
                break;

            default:
                // Handle other cases or do nothing
                break;
        }

    }

    /// <summary>
    /// Scale the button to ScaleFacror and back to 1
    /// </summary>
    private void Scale()
    {
        _button.transform.DOScale(_scaleFactor, duration / 2)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        _button.transform.DOScale(1f, duration / 2).SetEase(Ease.OutQuad);
                    });
    }

    /// <summary>
    /// Perform a clockwise 360-degree rotation for the icon
    /// </summary>
    private void Rotate()
    {
        _icon.DORotate(new Vector3(0, 0, -360), duration, RotateMode.FastBeyond360)
           .SetEase(Ease.OutCubic);
    }

    private void KillTweens()
    {
        _button.DOKill();
        _icon.DOKill();
    }


    private enum AnimationType
    {
        None,
        Scale,
        Rotate,
        ScaleAndRotate,
    }
}
