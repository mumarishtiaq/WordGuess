using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : ManagerBase
{
    public int Coins { get; private set; }
    public override void ResolveReferences()
    {
        
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.Save();
    }

    public bool SpendCoins(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.Save();
        return true;
    }

    private void LoadCoins()
    {
        Coins = PlayerPrefs.GetInt("Coins", 0);
    }
}
