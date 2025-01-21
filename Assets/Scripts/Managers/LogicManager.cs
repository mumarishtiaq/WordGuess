using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogicManager : ManagerBase
{

    public override void ResolveReferences()
    {
        //TODO : not implemented yet, may use in future implementations
        
    }

    public override void PerformActions()
    {
        //TODO : not implemented yet, may use in future implementations

    }
    public override void ReInitialize()
    {
        //TODO : not implemented yet, may use in future implementations
    }
    public ValidationType[] ValidateGuess(string targetWord, string guessedWord)
    {
        var feedback = new ValidationType[targetWord.Length];

        if (guessedWord.Length != targetWord.Length)
            return null;

        targetWord = targetWord.ToLower();
        guessedWord = guessedWord.ToLower();

        // Dictionary to track remaining occurrences of each character in the target word
        var charCounts = new Dictionary<char, int>();

        foreach (var c in targetWord)
        {
            if (!charCounts.ContainsKey(c))
                charCounts[c] = 0;
            charCounts[c]++;
        }

        // First pass: Identify Placed characters
        for (int i = 0; i < guessedWord.Length; i++)
        {
            if (guessedWord[i] == targetWord[i])
            {
                feedback[i] = ValidationType.Placed;
                charCounts[guessedWord[i]]--; // Reduce count for matched character
            }
        }

        // Second pass: Identify MisPlaced and Missing characters
        for (int i = 0; i < guessedWord.Length; i++)
        {
            if (feedback[i] == ValidationType.Placed)
                continue; // Skip already matched characters

            var c = guessedWord[i];

            if (charCounts.ContainsKey(c) && charCounts[c] > 0)
            {
                feedback[i] = ValidationType.MisPlaced;
                charCounts[c]--; // Reduce count for matched character
            }
            else
            {
                feedback[i] = ValidationType.Missing;
            }
        }

        return feedback;
    }

    public bool IsValidGuess(ValidationType[] feedback)
    {
        return feedback.All(f => f == ValidationType.Placed);
    }
    //will implement AI
    public bool IsValidWord(string guessedWord,List<string> words)
    {
        return words.Contains(guessedWord);
    }


    #region Debugging

    public string targetWord;
    public string guessedWord;
    // Test function to validate the implementation
    [ContextMenu("Test Word Guess")]
    private void TestWordGuess()
    {
        var feedback = ValidateGuess(targetWord, guessedWord);

        for (int i = 0; i < feedback.Length; i++)
        {
            Debug.Log($"{guessedWord[i]} is {feedback[i]}");
        }
    }
    #endregion Debugging



}
