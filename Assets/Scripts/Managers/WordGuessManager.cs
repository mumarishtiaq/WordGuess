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
        [SerializeField] private List<int> _occupiedIndexes = new List<int>();

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

        [ContextMenu("Restart Game")]
       public override void OnGameStart()
        {
            if (!Logic)
                Logic = GameManager.Instance.GetManager<LogicManager>();
            _targetWord = JsonConnector.GetRandomWord();
            WordLenght = 5;
            SetReferences();
            _wordProgressStatus = new ValidationType[WordLenght];
            _occupiedIndexes = new List<int>();
            _currentCharecterIndex = 0;
            _currentTry = 1;

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

                             OpenPopup<WinPopup>(popup =>
                             {
                                 popup.OperationsOnNext(_targetWord,ReInitializeGameSettings);
                             });
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
                while (_currentCharecterIndex < _wordLenght && _occupiedIndexes.Contains(_currentCharecterIndex))
                {
                    _currentCharecterIndex++;
                }
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
                do
                {
                    _currentCharecterIndex--; 
                }
                while (_currentCharecterIndex > 0 && _occupiedIndexes.Contains(_currentCharecterIndex));

                if (!_occupiedIndexes.Contains(_currentCharecterIndex))
                {
                    _playArea.OnBackSpace(_currentCharecterIndex);
                }
            }
            GameManager.Instance.TriggerSound(SoundType.Keyboard);
            ValidateWord();
        }

        public override void OnHint()
        {
            if (Logic.IsValidGuess(_wordProgressStatus))
            {
                //TODO : Popup or Toast
                Debug.LogWarning("All words are correctly placed ");
                return;
            }
            int successCount = _wordProgressStatus.Count(c => c == ValidationType.Placed);
            if(successCount >= WordLenght - 1)
            {
                //TODO : Popup or Toast
                Debug.LogWarning("Hint Unavailable");
                return ;
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
            _occupiedIndexes.Add(hintIndex);
            Debug.Log($"Hint is '{hintLetter}' at index : {hintIndex}");
            _playArea.SetHint(hintIndex,hintLetter);
            _wordProgressStatus[hintIndex] = ValidationType.Placed;
            ValidateWord();
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

        private void ReInitializeGameSettings()
        {
            _targetWord = JsonConnector.GetRandomWord();
            _wordProgressStatus = new ValidationType[WordLenght];
            _occupiedIndexes = new List<int>();
            _currentCharecterIndex = 0;
            _currentTry = 1;

            base.OnGameStart();
        }

    }
}




