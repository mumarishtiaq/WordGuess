using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int _currentCharecterIndex = 0;
    public string _targetWord;
    [Range(4, 6)]
    [SerializeField] private int _wordLenght = 0;
    [SerializeField] private ManagerBase[] _managers;

    private void Reset()
    {
        ResolveReferences();
    }

    private void Awake()
    {
        Instance = this;
        PerformActions();
    }
    private void ResolveReferences()
    {
        _managers = FindObjectsOfType<ManagerBase>();
        _managers.ToList().ForEach(manager => manager.ResolveReferences());
    }

    private void PerformActions()
    {
        _managers.ToList().ForEach(manager => manager.PerformActions());
    }
    public T GetManager<T>() where T : ManagerBase
    {
        return _managers.OfType<T>().FirstOrDefault();
    }



    public void OnKeyPress(string key)
    {
        if (_currentCharecterIndex < _wordLenght)
        {
            var playArea = GetManager<PlayingAreaManager>();

            playArea.OnInput(key,_currentCharecterIndex);

            _currentCharecterIndex++;
        }

    }

    public void OnBackSpace()
    {
        if (_currentCharecterIndex > 0)
        {
            var playArea = GetManager<PlayingAreaManager>();

            playArea.OnBackSpace(_currentCharecterIndex - 1);
            _currentCharecterIndex--;
        }
    }

    public void OnSubmit()
    {
        var logic = GetManager<LogicManager>();
        var playArea = GetManager<PlayingAreaManager>();

        var guessedWord = playArea.GetGuessedWord();
        var feedback = logic.ValidateGuess(guessedWord, _targetWord);

       playArea.SetFeedback(feedback);
        _currentCharecterIndex = 0;
    }



}
