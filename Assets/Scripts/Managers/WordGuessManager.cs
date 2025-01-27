using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WordGuess
{
    public class WordGuessManager : GameBase
    {

        public static WordGuessManager Instance;
        public int _currentCharecterIndex = 0;
        public int _currentTry = 1;
        public string _targetWord = "Apple";
        [Range(4, 6)]
        [SerializeField] private int _wordLenght = 5;
        [SerializeField] private int _totalTries = 6;
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


        //Handlers
        [SerializeField] private PlayingAreaHandler _playArea;
        [SerializeField] private WordHistoryHandler _wordHistory;
       


        public override void SetReferences()
        {
            Instance = this;
            base.SetReferences();

            if (!_playArea)
                _playArea = GetHandler<PlayingAreaHandler>();

            if (!_wordHistory)
                _wordHistory = GetHandler<WordHistoryHandler>();

            _handlers.ToList().ForEach(manager => manager.ResolveReferences());
        }

       public override void OnGameStart()
        {
            _targetWord = JsonConnector.GetRandomWord();
            WordLenght = 5;
            SetReferences();
            base.OnGameStart();
        }
        private void ValidateWord()
        {
            bool validLenght = _currentCharecterIndex == _wordLenght;
            bool isValid = true;
            if (validLenght)
            {
                var guessedWord = _playArea.GetGuessedWord();
                var currentSearchableWords = JsonConnector.GetSearchableWords(guessedWord[0].ToString());

                isValid = Logic.IsValidWord(guessedWord, currentSearchableWords);
            }
            _playArea.OnValidateGuessWord(validLenght, isValid);
            _isSubmitEnabled = validLenght && isValid;
        }
        public override void OnSubmit()
        {
            var soundType = _isSubmitEnabled ? SoundType.DefaultButton : SoundType.Error;

            GameManager.Instance.TriggerSound(soundType);

            if (_currentCharecterIndex == _wordLenght && _isSubmitEnabled)
            {
                _isSubmitEnabled = false;
                var guessedWord = _playArea.GetGuessedWord();
                var feedback = Logic.ValidateGuess(_targetWord, guessedWord);

                StartCoroutine(_playArea.SetFeedback(feedback,
                     onIteration: () => { GameManager.Instance.TriggerSound(SoundType.Feedback); },

                     onComplete: () =>
                     {
                         if (Logic.IsValidGuess(feedback))
                         {
                             // win logic
                             Debug.Log("You Win");
                             GameManager.Instance.TriggerSound(SoundType.Success);
                         }
                         else
                         {
                             _playArea.AnimateFeedback(_wordHistory.transform.GetChild(_currentTry - 1), () =>
                             {
                                 _wordHistory.SetFeedback(_currentTry - 1, guessedWord, feedback);
                                 GameManager.Instance.TriggerSound(SoundType.WordHistory);
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

        public override void OnInput(string key)
        {
            if (_currentCharecterIndex < _wordLenght)
            {
                _playArea.OnInput(key, _currentCharecterIndex);
                _currentCharecterIndex++;
            }
            GameManager.Instance.TriggerSound(SoundType.Keyboard);

            ValidateWord();
        }

        public override void OnCancel()
        {
            if (_currentCharecterIndex > 0)
            {
                _playArea.OnBackSpace(_currentCharecterIndex - 1);
                _currentCharecterIndex--;
            }
            GameManager.Instance.TriggerSound(SoundType.Keyboard);
            ValidateWord();
        }
    }
}




