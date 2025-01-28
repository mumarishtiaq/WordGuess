using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupManager : ManagerBase
{
    [SerializeField] private GameObject[] _popupTemplates;
    [SerializeField] protected List<PopupBase> _activePopups;

    [ContextMenu("ResolveReferences")]
    public override void ResolveReferences()
    {
        if (_popupTemplates.Length == 0)
            _popupTemplates = Resources.LoadAll<GameObject>("PopupPrefabs");
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

        var parent = GameObject.Find("Canvas").transform;


        if (prefab != null)
        {
            T popupInstance = Instantiate(prefab, parent);
            popupInstance.gameObject.SetActive(false);
            return popupInstance;
        }

        Debug.LogError($"Popup prefab for type {typeof(T).Name} not found!");
        return null;
    }


    [ContextMenu("Test Popup")]
    private void Test()
    {
        OpenPopup<SettingsPopup>(
            popup =>
            {
                Debug.Log("Opened Invoke");
                popup.OnClose.AddListener(() => Debug.Log("On Close Invoke"));
            });
    }
}


