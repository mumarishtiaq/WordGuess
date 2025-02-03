using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInventory : MonoBehaviour
{
    protected string _gameKey;

    public int Hints { get; protected set; }
    public int Level { get; protected set; }

    public GameInventory(string gameName)
    {
        _gameKey = gameName;
        LoadInventory();
    }

    public virtual void UseHint()
    {
        if (Hints > 0) Hints--;
        SaveInventory();
    }

    public virtual void AddHints(int amount)
    {
        Hints += amount;
        SaveInventory();
    }

    public virtual void IncreaseLevel()
    {
        Level++;
        SaveInventory();
    }

    protected virtual void LoadInventory()
    {
        Hints = PlayerPrefs.GetInt($"{_gameKey}_Hints", 1);
        Level = PlayerPrefs.GetInt($"{_gameKey}_Level", 1);
    }

    protected virtual void SaveInventory()
    {
        PlayerPrefs.SetInt($"{_gameKey}_Hints", Hints);
        PlayerPrefs.SetInt($"{_gameKey}_Level", Level);
        PlayerPrefs.Save();
    }
}
