using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    public List<LetterGroup> data;
    [ContextMenu("LoadWordsFromJson")]
    void LoadWordsFromJson()
    {
         JsonConnector.LoadWordsFromJSON(()=>
         {
             data = JsonConnector._letterGroups;
         });
    }

    [ContextMenu("TestRandom")]
    void TestRandom()
    {
      Debug.Log(JsonConnector.GetRandomWord());
    }
}
