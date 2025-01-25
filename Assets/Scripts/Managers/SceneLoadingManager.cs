using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : ManagerBase
{
    [SerializeField] private string _wordGuessSceneName;
    [SerializeField] private string _wordMatchSceneName;
    public override void PerformActions()
    {
       
    }

    public override void ReInitialize()
    {
        
    }

    public override void ResolveReferences()
    {
        
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);   
    }
}
