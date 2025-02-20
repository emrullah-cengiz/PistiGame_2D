using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LobbyUIPanel : UIPanel
{
    [SerializeField] private HorizontalLayoutGroup _roomLayoutGroup;
    
    [SerializeField, Space] private RoomUIView _roomUIViewPrefab;

    #region Injects

    [Inject] private GameSettings _settings;
    [Inject] private IObjectResolver _objectResolver;

    #endregion

    public override void Initialize()
    {
        base.Initialize();

        SetupRoomViews().Forget();
    }

    private async UniTask SetupRoomViews()
    {
        var views = await InstantiateAsync(_roomUIViewPrefab, _settings.Rooms.Count, _roomLayoutGroup.transform);

        int i = 0;
        foreach (var roomData in _settings.Rooms)
        {
            var view = views[i];
            
            _objectResolver.Inject(view);
            
            view.Initialize(roomData);
            i++;
        }
    }
}