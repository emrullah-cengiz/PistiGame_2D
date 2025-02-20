using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Dictionary<UIPanelType, UIPanel> _uiPanels;
    
    private UIPanel _currentPanel;
    private UIPanel _loadingPanel;

    private void OnEnable()
    {
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

    private void OnGameLoadingStart()                
    {  
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
        
        _currentPanel = _uiPanels[type];

        _currentPanel.Open(0.1f);
    }

    private void ShowLoading() => _loadingPanel.Open(.3f);
    private void HideLoading() => _loadingPanel.Close(.3f);
}

public enum UIPanelType
{
    Lobby,
    Loading,
    Table
}