using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WordGuess;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
    public SceneBase _currentGame;
    //[Space]
    //[SerializeField] private GameBase[] _games;
    [SerializeField] private ManagerBase[] _managers;

    //managers
    [SerializeField] private SceneLoadingManager _sceneManager;
    [SerializeField] private SoundManager _audio;

    public Button _wordGuessButton;
    public Button _wordMatchButton;

    private void Reset()
    {
        ResolveReferences();
    }

    private void Awake()
    {

        Instance = this;
        DontDestroyOnLoad(gameObject);
        PerformActions();

        _wordGuessButton.onClick.RemoveAllListeners();
        _wordMatchButton.onClick.RemoveAllListeners();

        _wordGuessButton.onClick.AddListener(() => LoadGame(GameType.WordGuess));
        _wordMatchButton.onClick.AddListener(() => LoadGame(GameType.WordMatch));
    }
    private void ResolveReferences()
    {
        _managers = FindObjectsOfType<ManagerBase>();
        //_games = FindObjectsOfType<GameBase>();
        _managers.ToList().ForEach(manager => manager.ResolveReferences());
    }

    private void PerformActions()
    {
        JsonConnector.LoadWordsFromJSON();
        _sceneManager = GetManager<SceneLoadingManager>();
        _audio = GetManager<SoundManager>();
    }

    public T GetManager<T>() where T : ManagerBase
    {
        return _managers.OfType<T>().FirstOrDefault();
    }
    
    //private T GetGame<T>() where T : GameBase
    //{
    //    return _games.OfType<T>().FirstOrDefault();
    //}
    public void LoadGame(GameType gameType = GameType.Home)
    {
        //switch (gameType)
        //{
        //    case GameType.Home:
        //        _currentGame = null;
        //        break;
        //    case GameType.WordGuess:
        //        _currentGame = GetGame<WordGuessManager>();
        //        break;
        //    case GameType.WordMatch:
        //        _currentGame = GetGame<WordMatchManager>();
        //        break;
        //    default:
        //        break;
        //}
        _sceneManager.LoadScene((int)gameType, OnSceneLoaded);

    }
    private void OnSceneLoaded()
    {
        Debug.Log("Game started");
       // _currentGame?.OnGameStart();
    }

    public void OnInput(string k)
    {
        _currentGame.OnInput(k);
    } 
    public void OnCancel()
    {
        _currentGame.OnCancel();
    } 
    public void OnSubmit()
    {
        _currentGame.OnSubmit();
    }
    public void OnHint()
    {
        _currentGame.OnHint();
    }

    public void TriggerSound(SoundType soundType)
    {
        _audio.PlaySound(soundType);
    }
}

public enum GameType
{
    Home = 0,
    WordGuess = 1,
    WordMatch = 2
}
public enum SoundType
{
    None,
    Keyboard,
    Feedback,
    DefaultButton,
    WordHistory,
    Success,
    Error
}

