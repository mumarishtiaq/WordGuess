using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ManagerBase
{
    [SerializeField] private AudioSource _KeyboardBtnSound;

    public override void PerformActions()
    {
       
    }

    public void PlayKeyBoardButtonSound()
    {
        _KeyboardBtnSound.Play();
    }

    public override void ReInitialize()
    {
        
    }

    public override void ResolveReferences()
    {
        
    }
}
