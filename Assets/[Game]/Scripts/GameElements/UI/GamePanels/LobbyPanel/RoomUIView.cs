using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class RoomUIView : MonoBehaviour
{
    public RoomData Data { get; private set; }

    [SerializeField] private TMP_Text _nameText, _betRangeText;
    [SerializeField] private Button _playBtn, _createTableBtn;

    #region Injects

    [Inject] private PlayerWallet _playerWallet;

    #endregion

    private void OnEnable()
    {
        Event.OnPlayerDataChanged += OnPlayerDataChanged;
    }

    public void Initialize(RoomData data)
    {
        Data = data;

        UpdateView();
    }

    private void OnPlayerDataChanged() => UpdateView();

    private void UpdateView()
    {
        var hasEnoughMoney = _playerWallet.HasEnoughBalance(Data.BetRange.x);

        _nameText.text = Data.Name;
        _betRangeText.text = $"{Data.BetRange.x} - {Data.BetRange.y}";

        _playBtn.interactable = hasEnoughMoney;
        _createTableBtn.interactable = hasEnoughMoney;
    }
}