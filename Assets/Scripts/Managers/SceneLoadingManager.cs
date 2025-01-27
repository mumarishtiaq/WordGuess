using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : ManagerBase
{
    //public override void PerformActions()
    //{
       
    //}

    //public override void ReInitialize()
    //{
        
    //}

    public override void ResolveReferences()
    {
        
    }

    public void LoadScene(int sceneIndex, Action onSceneLoaded)
    {
        AsyncOperation async= SceneManager.LoadSceneAsync(sceneIndex);

        async.completed += (operation) =>
        {
            onSceneLoaded?.Invoke();
        };
    }
}
