using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ManagerBase
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;

    [Space]
    [Header("Clips")]
    [SerializeField] private AudioClip _keyboardClip;
    [SerializeField] private AudioClip _feedbackClip;
    [SerializeField] private AudioClip _defaultButtonClip;
    [SerializeField] private AudioClip _wordHistoryClip;
    [SerializeField] private AudioClip _successClip;
    [SerializeField] private AudioClip _errorClip;


    public override void ResolveReferences()
    {
        if (!_sfxSource)
            _sfxSource = transform.Find("SFXSource")?.GetComponent<AudioSource>();
        
        
        if (!_musicSource)
            _musicSource = transform.Find("MusicSource")?.GetComponent<AudioSource>();
    }
    //public override void PerformActions()
    //{
       
    //}

    //public override void ReInitialize()
    //{

    //}
  
    public void PlaySound(SoundType _soundType)
    {
        switch (_soundType)
        {
            case SoundType.Keyboard:
                _sfxSource.PlayOneShot(_keyboardClip);
                break; 
            case SoundType.Feedback:
                _sfxSource.PlayOneShot(_feedbackClip);
                break;
            case SoundType.DefaultButton:
                _sfxSource.PlayOneShot(_defaultButtonClip);
                break;
            case SoundType.WordHistory:
                _sfxSource.PlayOneShot(_wordHistoryClip);
                break;
            case SoundType.Success:
                _sfxSource.PlayOneShot(_successClip);
                break;
            case SoundType.Error:
                _sfxSource.PlayOneShot(_errorClip);
                break;
            default:
                break;
        }
    }
}


