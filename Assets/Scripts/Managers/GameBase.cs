using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameBase : MonoBehaviour
{
    protected LogicManager Logic;
    public List<HandlerBase> _handlers;
    public virtual void SetReferences()
    {
        if (_handlers.Count == 0)
            _handlers = FindObjectsOfType<HandlerBase>().ToList();

        Logic = GameManager.Instance.GetManager<LogicManager>();

    }
    public virtual void OnGameStart()
    {
        _handlers.ToList().ForEach(manager => manager.PerformActions());
    }

    public abstract void OnInput(string key);
    public abstract void OnCancel();

    public abstract void OnSubmit();

    protected T GetHandler<T>() where T : HandlerBase
    {
        return _handlers.OfType<T>().FirstOrDefault();
    }




}
