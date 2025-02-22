using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using VContainer.Unity;

public class UIManager : MonoBehaviour, IInitializable
{
    ///TODO: Dictionary 
    [SerializeField] private List<UIPanel> _uiPanels;
    
    [SerializeField] private UIPanel _loadingPanel;
    [SerializeField, ReadOnly] private UIPanel _currentPanel;

    private void OnEnable()
    {
        Debug.Log("UIManager::OnEnable");
        
        Event.OnGameLoadingStart += OnGameLoadingStart;
        Event.OnEnterLobby += OnEnterLobby;
        // Event.OnTableSessionStart += OnTableSessionStart;
    }

    private void OnDisable()
    {
        Event.OnGameLoadingStart -= OnGameLoadingStart;
        Event.OnEnterLobby -= OnEnterLobby;
    }
    
    public void Initialize()
    {
        
    }

//     private void Awake()
//     {
// #if UNITY_EDITOR
//         foreach (var panel in _uiPanels) 
//             panel.gameObject.SetActive(false);
// #endif
//     }

    private void OnGameLoadingStart()
    {
        Debug.Log("Game loading..");
        
        ShowLoading();
    }

    private void OnEnterLobby()
    {
        HideLoading();
        OpenPanel(UIPanelType.Lobby);
    }

    private void OpenPanel(UIPanelType type)
    {
        if(_currentPanel)
            _currentPanel.Close();

        _currentPanel = GetPanel(type);

        _currentPanel.Open(0.1f);
        
        _currentPanel.Initialize();
    }

    private void ShowLoading() => _loadingPanel.Open(.3f);
    private void HideLoading() => _loadingPanel.Close(.3f);

    private UIPanel GetPanel(UIPanelType type)
    {
        for (int i = 0; i < _uiPanels.Count; i++)
        {
            if (_uiPanels[i].PanelType == type)
                return _uiPanels[i];
        }
        
        return null;
    }

}

public enum UIPanelType
{
    Lobby,
    Loading,
    Table
}