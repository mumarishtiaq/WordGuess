using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : PopupBase
{

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
    public override void OnOpen()
    {
        base.OnOpen();

        //OnOpen implementation
    }


}
