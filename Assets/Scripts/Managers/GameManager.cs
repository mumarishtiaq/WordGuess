using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WordGuess
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public int _currentCharecterIndex = 0;
        public int _currentTry = 1;
        public string _targetWord = "Apple";
        [Range(4, 6)]
        [SerializeField] private int _wordLenght = 5;
        [SerializeField] private int _totalTries = 6;
        [SerializeField] private List<string> _currentSearchableWords;
        [SerializeField] private bool _isSubmitEnabled = false;

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
        [SerializeField] private SoundManager _audio;

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
            WordLenght = 5;
            _managers.ToList().ForEach(manager => manager.PerformActions());

            _playArea = GetManager<PlayingAreaManager>();
            _wordHistory = GetManager<WordHistoryManager>();
            _logic = GetManager<LogicManager>();
            _audio = GetManager<SoundManager>();

            JsonConnector.LoadWordsFromJSON(() =>
            {
                _targetWord = JsonConnector.GetRandomWord();
            });
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
            TriggerSound(SoundType.Keyboard);

            ValidateWord();

        }

        private void ValidateWord()
        {
            bool validLenght = _currentCharecterIndex == _wordLenght;
            bool isValid = true;
            if (validLenght)
            {
                var guessedWord = _playArea.GetGuessedWord();
                _currentSearchableWords = JsonConnector.GetSearchableWords(guessedWord[0].ToString());

                isValid = _logic.IsValidWord(guessedWord, _currentSearchableWords);
            }
            _playArea.OnValidateGuessWord(validLenght, isValid);
            _isSubmitEnabled = validLenght && isValid;
        }

        public void OnBackSpace()
        {
            if (_currentCharecterIndex > 0)
            {
                _playArea.OnBackSpace(_currentCharecterIndex - 1);
                _currentCharecterIndex--;
            }
            TriggerSound(SoundType.Keyboard);
            ValidateWord();

        }

        public void OnSubmit()
        {
            var soundType = _isSubmitEnabled ? SoundType.DefaultButton : SoundType.Error;

            TriggerSound(soundType);

            if (_currentCharecterIndex == _wordLenght && _isSubmitEnabled)
            {
                _isSubmitEnabled = false;
                var guessedWord = _playArea.GetGuessedWord();
                var feedback = _logic.ValidateGuess(_targetWord, guessedWord);

                StartCoroutine(_playArea.SetFeedback(feedback,
                     onIteration: () => { TriggerSound(SoundType.Feedback);},

                     onComplete: () =>
                     {
                         if (_logic.IsValidGuess(feedback))
                         {
                             // win logic
                             Debug.Log("You Win");
                             TriggerSound(SoundType.Success);
                         }
                         else
                         {
                             _playArea.AnimateFeedback(_wordHistory.Parent.GetChild(_currentTry - 1), () =>
                             {
                                 _wordHistory.SetFeedback(_currentTry - 1, guessedWord, feedback);
                                 TriggerSound(SoundType.WordHistory);
                                 if (_currentTry < _totalTries)
                                     _currentTry++;

                                 else
                                 {
                                     Debug.Log("You lose");
                                 }
                                 _currentCharecterIndex = 0;
                                 _isSubmitEnabled = true;
                             });
                         }
                     }));
            }
        }

        public void TriggerSound(SoundType soundType)
        {
            _audio.PlaySound(soundType);
        }
    }
}

public enum SoundType
{
    None,
    Keyboard,
    Feedback,
    DefaultButton,
    WordHistory,
    Success,
    Error
}


