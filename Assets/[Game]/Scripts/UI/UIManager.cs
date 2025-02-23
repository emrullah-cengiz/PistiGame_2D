using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using VContainer.Unity;

public class UIManager : MonoBehaviour
{
    ///TODO: Dictionary 
    [SerializeField] private List<UIPanel> _uiPanels;
    
    [SerializeField] private UIPanel _loadingPanel;
    [SerializeField, ReadOnly] private UIPanel _currentPanel;

    private void OnEnable()
    {
        Event.OnEnterLobby += OnEnterLobby;
        Event.OnTableSessionStart += OnTableSessionStart;
    }
    private void OnDisable()
    {
        Event.OnEnterLobby -= OnEnterLobby;
        Event.OnTableSessionStart += OnTableSessionStart;
    }

    private void OpenPanel(UIPanelType type)
    {
        if(_currentPanel)
            _currentPanel.Close();

        _currentPanel = GetPanel(type);

        _currentPanel.Open(0.1f);
        
        _currentPanel.Initialize();
    }

    private void OnEnterLobby() => OpenPanel(UIPanelType.Lobby);
    private void OnTableSessionStart() => OpenPanel(UIPanelType.Table);

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