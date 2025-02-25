using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;

public class LobbyUIPanel : UIPanel
{
    [SerializeField] private HorizontalLayoutGroup _roomLayoutGroup;
    [SerializeField] private CarouselSliderController _roomsCarousel;
    [SerializeField] private RoomUIView _roomViewPrefab;
    
    #region Injects

    [Inject] private LobbySettings _lobbySettings;
    [Inject] private IObjectResolver _objectResolver;

    #endregion

    public async void Start()
    {
        await SetupRoomViews();
        
        _roomsCarousel.Setup();
    }

    private async UniTask SetupRoomViews()
    {
        var views = await InstantiateAsync(_roomViewPrefab, _lobbySettings.Rooms.Count, _roomLayoutGroup.transform);

        int i = 0;
        foreach (var roomData in _lobbySettings.Rooms)
        {
            var view = views[i];
            
            _objectResolver.Inject(view);
            
            view.Initialize(roomData);
            i++;
        }
    }
}