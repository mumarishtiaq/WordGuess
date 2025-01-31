using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SceneBase : MonoBehaviour
{
    [SerializeField] private GameObject[] _popupTemplates;
    [SerializeField] protected List<PopupBase> _activePopups;

    [SerializeField] private Stack<GameObject> _test;

    protected LogicManager Logic;
    public List<HandlerBase> _handlers = new List<HandlerBase>();
    private Transform _canvas;
    public virtual void SetReferences()
    {
        if (_handlers.Count == 0)
        {
            _handlers = FindObjectsOfType<HandlerBase>().ToList();

            _handlers.ToList().ForEach(handler =>
            {
                handler.ResolveReferences();
                handler.SceneBase = this;
            });
        }

        if(!_canvas)
            _canvas = GameObject.Find("Canvas")?.transform;


        if (_popupTemplates == null)
            _popupTemplates = Resources.LoadAll<GameObject>("PopupPrefabs");
    }
    public virtual void OnGameStart()
    {
        _handlers.ToList().ForEach(manager => manager.PerformActions());
        GameManager.Instance._currentGame = this;
    }

    public abstract void OnInput(string key);
    public abstract void OnCancel();

    public abstract void OnSubmit();

    public abstract void OnHint();

    protected T GetHandler<T>() where T : HandlerBase
    {
        return _handlers.OfType<T>().FirstOrDefault();
    }


    public void OpenPopup<T>(Action<T> onOpened = null, bool darkenBackground = false) where T : PopupBase
    {
        if (_popupTemplates.Length == 0)
        {
            Debug.LogError("Popup Templates empty");
            return;
        }

        T existingPopup = _activePopups.OfType<T>().FirstOrDefault();

        if (existingPopup != null)
        {
            existingPopup.OnOpen();
            onOpened?.Invoke(existingPopup);
            return;
        }

        T newPopup = CreatePopup<T>();
        if (newPopup != null)
        {
            _activePopups.Add(newPopup);
            newPopup.OnOpen();

            // Invoke the callback if provided
            onOpened?.Invoke(newPopup);
        }
    }
    private T CreatePopup<T>() where T : PopupBase
    {
        // Instantiate the popup prefab dynamically (replace with your prefab 

        T prefab = _popupTemplates.
             Select(go => go.GetComponent<T>())
             .FirstOrDefault(component => component != null);

        if (prefab != null)
        {
            T popupInstance = Instantiate(prefab, _canvas);
            popupInstance.gameObject.SetActive(false);
            return popupInstance;
        }

        Debug.LogError($"Popup prefab for type {typeof(T).Name} not found!");
        return null;
    }




}
