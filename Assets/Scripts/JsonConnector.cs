using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonConnector
{

    public static List<LetterGroup> _letterGroups;


    private const string fileName = "TransformedWordList.json";



    public static void LoadWordsFromJSON(Action OnComplete = null)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            // Read the JSON file
            string jsonContent = File.ReadAllText(filePath);


            _letterGroups = JsonUtility.FromJson<LetterGroupsWrapper>($"{{\"Words\":{jsonContent}}}").Words;



            Debug.Log($"Words successfully loaded.");

        }
        else
        {
            Debug.LogError($"JSON file not found at: {filePath}");
        }

        OnComplete?.Invoke();
    }


    public static string GetRandomWord()
    {
        if (_letterGroups == null || _letterGroups.Count == 0)
        {
            Debug.LogError("words list is empty or not loaded.");
            return string.Empty;
        }

        int randomLetterIndex = GetRandom(0, _letterGroups.Count);

        var letterWords = _letterGroups[randomLetterIndex].Words;

        if (letterWords != null || letterWords.Count != 0)
        {
            int randomindex = GetRandom(0, letterWords.Count);
            return letterWords[randomindex];
        }

        return string.Empty;
    }

    public static List<string> GetSearchableWords(string initialLetter)
    {
        var letterGroup = _letterGroups.Find(group => group.Letter.Equals(initialLetter, StringComparison.OrdinalIgnoreCase));

        if (letterGroup != null && letterGroup.Words != null && letterGroup.Words.Count > 0)
            return letterGroup.Words;

        return new List<string>();

    }
    private static int GetRandom(int start, int end)
    {
        return UnityEngine.Random.Range(start, end);
    }
}

[System.Serializable]
public class LetterGroup
{
    public string Letter; // The letter (A, B, C, etc.)
    public List<string> Words; // List of words starting with the letter
}

[System.Serializable]
public class LetterGroupsWrapper
{
    public List<LetterGroup> Words;
}

