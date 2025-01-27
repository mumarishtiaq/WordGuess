using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : ManagerBase
{
    [SerializeField] private GameObject[] _popupTemplates;
    [SerializeField] private List<PopupBase> _activePopups;

    [ContextMenu("ResolveReferences")]
    public override void ResolveReferences()
    {
        if (_popupTemplates.Length == 0)
            _popupTemplates = Resources.LoadAll<GameObject>("PopupPrefabs");
    }
}

public enum PopupType
{
    Win,
    Lose,
    Settings,
    Info,
    Shop,
    DailyBonus,
    SpinWheel,
    FreeGift
}

