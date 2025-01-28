using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarHandler : HandlerBase
{
    [SerializeField] private Button _homeBtn;
    [SerializeField] private Button _settingsBtn;

    [SerializeField] private TextMeshProUGUI _currentLevelTxt;

    [SerializeField] private TextMeshProUGUI _currentStreakTxt;
    [SerializeField] private TextMeshProUGUI _highestStreak;


    [ContextMenu("ResolveReferences")]
    public override void ResolveReferences()
    {
        if (!_homeBtn)
            _homeBtn = transform.Find("UpperRow/HomeBtn").GetComponent<Button>();

        if (!_settingsBtn)
            _settingsBtn = transform.Find("UpperRow/SettingsBtn").GetComponent<Button>();

        if (!_currentLevelTxt)
            _currentLevelTxt = transform.Find("UpperRow/CurrentLevelText").GetComponent<TextMeshProUGUI>();

        if (!_currentStreakTxt)
            _currentStreakTxt = transform.Find("LowerRow/CurrentStreak").GetComponentInChildren<TextMeshProUGUI>();

        if (!_highestStreak)
            _highestStreak = transform.Find("LowerRow/HighestStreak").GetComponentInChildren<TextMeshProUGUI>();
    }
    public override void PerformActions()
    {
        _homeBtn.onClick.RemoveAllListeners();
        _homeBtn.onClick.AddListener(() => GameManager.Instance.LoadGame());


        _settingsBtn.onClick.RemoveAllListeners();
        _settingsBtn.onClick.AddListener(() => SceneBase.OpenPopup<SettingsPopup>());
    }

    public override void ReInitialize()
    {

    }


}
