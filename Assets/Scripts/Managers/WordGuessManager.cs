using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WordGuess
{
    public class WordGuessManager : SceneBase
    {

        public static WordGuessManager Instance;
        public int _currentCharecterIndex = 0;
        public int _currentTry = 1;
        public string _targetWord = "Apple";
        [Range(4, 6)]
        [SerializeField] private int _wordLenght = 5;
        [SerializeField] private int _totalTries = 6;
        [SerializeField] private ValidationType[] _wordProgressStatus;
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

        private void Reset()
        {
            SetReferences();
        }
        private void Awake()
        {
            Instance = this;
            OnGameStart();
        }

        public override void SetReferences()
        {
            base.SetReferences();


            if (!_playArea)
                _playArea = GetHandler<PlayingAreaHandler>();

            if (!_wordHistory)
                _wordHistory = GetHandler<WordHistoryHandler>();
        }

       public override void OnGameStart()
        {
            if (!Logic)
                Logic = GameManager.Instance.GetManager<LogicManager>();
            _targetWord = JsonConnector.GetRandomWord();
            WordLenght = 5;
            SetReferences();
            _wordProgressStatus = new ValidationType[WordLenght];
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
                                 UpdateWordProgressStatus(feedback);
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

        public override void OnHint()
        {
            if (Logic.IsValidGuess(_wordProgressStatus))
            {
                Debug.Log("All words are correctly placed ");
                return;
            }
            var hintIndex = 0;
            Char hintLetter = 'a';
            for (int i = 0; i < _wordProgressStatus.Length; i++)
            {
                if (_wordProgressStatus[i] != ValidationType.Placed)
                {
                    hintIndex = i;
                    hintLetter = _targetWord[i];
                    break;
                }
            }
            Debug.Log($"Hint is '{hintLetter}' at index : {hintIndex}");
            _playArea.SetHint(hintIndex,hintLetter);
            _wordProgressStatus[hintIndex] = ValidationType.Placed;

        }
        /// <summary>
        /// this function will update the list of current progress of word, assign feedback where word is correcly placed, updates on each submit pressed
        /// </summary>
        /// <param name="currentFeedback">current feedback from the word guessed by the user</param>
        private void UpdateWordProgressStatus(ValidationType[] currentFeedback)
        {
            for (int i = 0; i < _wordProgressStatus.Length; i++)
            {
                var oldState = _wordProgressStatus[i];
                var newState = currentFeedback[i];
                if(newState == ValidationType.Placed && oldState != ValidationType.Placed)
                    _wordProgressStatus[i] = currentFeedback[i];

            }
        }

    }
}




