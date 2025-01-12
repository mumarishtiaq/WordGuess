using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int _currentCharecterIndex = 0;
    public int _currentTry = 0;
    public string _targetWord ="Apple";
    [Range(4, 6)]
    [SerializeField] private int _wordLenght = 5;
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
        var wordHistory = GetManager<WordHistoryManager>();

        var guessedWord = playArea.GetGuessedWord();
        var feedback = logic.ValidateGuess(_targetWord, guessedWord);

       playArea.SetFeedback(feedback);
        _currentCharecterIndex = 0;

        wordHistory.SetFeedback(_currentTry, feedback);
        //playArea.ResetCharecters();
        _currentTry++;

    }



}
