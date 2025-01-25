using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
    [SerializeField] private ManagerBase[] _managers;


    [SerializeField] private SceneLoadingManager _sceneManager;

    private void Reset()
    {
        ResolveReferences();
    }

    private void Awake()
    {

        Instance = this;
        PerformActions();
    }
    private void ResolveReferences()
    {
        _managers = FindObjectsOfType<ManagerBase>();
        _managers.ToList().ForEach(manager => manager.ResolveReferences());
    }

    private void PerformActions()
    {
        _managers.ToList().ForEach(manager => manager.PerformActions());

        JsonConnector.LoadWordsFromJSON(() =>
        {
           
        });
        _sceneManager = GetManager<SceneLoadingManager>();
    }

    public T GetManager<T>() where T : ManagerBase
    {
        return _managers.OfType<T>().FirstOrDefault();
    }

    public void Load()
    {
        LoadGame(GameType.WordGuess);
    }
    public void LoadGame(GameType gameType)
    {
        _sceneManager.LoadScene((int)gameType);
    }
}

public enum GameType
{
    WordGuess = 1,
    WordMatch = 2
}

