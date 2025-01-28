using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordMatchManager : SceneBase
{

    private void Reset()
    {
        SetReferences();
    }
    private void Awake()
    {
        OnGameStart();
    }

    public override void SetReferences()
    {
        base.SetReferences();
    }
    public override void OnGameStart()
    {
        Debug.Log("Word Match started");
    }
    public override void OnInput(string key)
    {

    }

    public override void OnCancel()
    {
        
    }

    public override void OnSubmit()
    {
        
    }
}
