using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int _currentCharecterIndex = 0;
    public int _currentTry = 1;
    public string _targetWord = "Apple";
    [Range(4, 6)]
    [SerializeField] private int _wordLenght = 5;
    [SerializeField] private int _totalTries = 6;

    public int WordLenght
    {
        get => _wordLenght;
        set
        {
            _wordLenght = value;
            _totalTries = value + 1;
        }
    } 
    public int TotalTries
    {
        get => _totalTries;
        set => _totalTries = value;

    }



    [SerializeField] private ManagerBase[] _managers;

    //managers
    [SerializeField] private PlayingAreaManager _playArea;
    [SerializeField] private WordHistoryManager _wordHistory;
    [SerializeField] private LogicManager _logic;

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

        _playArea = GetManager<PlayingAreaManager>();
        _wordHistory = GetManager<WordHistoryManager>();
        _logic = GetManager<LogicManager>();
    }
    public void ReInitialize()
    {
        _managers.ToList().ForEach(manager => manager.ReInitialize());
    }
    public T GetManager<T>() where T : ManagerBase
    {
        return _managers.OfType<T>().FirstOrDefault();
    }



    public void OnKeyPress(string key)
    {
        if (_currentCharecterIndex < _wordLenght)
        {
            _playArea.OnInput(key, _currentCharecterIndex);
            _currentCharecterIndex++;
        }

    }

    public void OnBackSpace()
    {
        if (_currentCharecterIndex > 0)
        {
            _playArea.OnBackSpace(_currentCharecterIndex - 1);
            _currentCharecterIndex--;
        }
    }

    public void OnSubmit()
    {
        var guessedWord = _playArea.GetGuessedWord();
        var feedback = _logic.ValidateGuess(_targetWord, guessedWord);

        _playArea.SetFeedback(feedback);
        _currentCharecterIndex = 0;

        if (_logic.IsValidGuess(feedback))
        {
            // win logic
            Debug.Log("You Win");
        }
        else
        {
            _wordHistory.SetFeedback(_currentTry-1, guessedWord, feedback);
            if (_currentTry < _totalTries)
            {
                StartCoroutine(Test());
                _currentTry++;
            }
            else
            {
                Debug.Log("You lose");
            }
        }



    }
    private IEnumerator Test()
    {
        yield return new WaitForSeconds(0.5f);
        _playArea.ReInitialize();

    }

    


}
