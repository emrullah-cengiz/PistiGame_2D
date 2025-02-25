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
        RefreshView();
    }

    private void OnDestroy()
    {
        Event.OnPlayerDataChanged -= OnPlayerDataChanged;
        _createTableBtn.onClick.RemoveAllListeners();
        _playBtn.onClick.RemoveAllListeners();
    }

    public void Initialize(RoomData data)
    {
        Data = data;

        _createTableBtn.onClick.AddListener(() => Event.OnShowCreateTablePopup_Click?.Invoke(data));

        _playBtn.onClick.AddListener(
            () => Event.OnCreateTableButton_Click?.Invoke(new TableData()
            {
                RoomData = Data,
                BetAmount = data.BetRange.x,
                Mode = TableMode.TwoPlayers
            }));

        RefreshView();
    }

    private void OnPlayerDataChanged() => RefreshView();

    private void RefreshView()
    {
        if (_playerWallet == null) return;

        var hasEnoughMoney = _playerWallet.HasEnoughBalance(Data.BetRange.x);

        _nameText.text = Data.Name;
        _betRangeText.text = $"{Data.BetRange.x.ToAbbreviated("$")} - " +
                             $"{Data.BetRange.y.ToAbbreviated("$")}";

        _playBtn.interactable = hasEnoughMoney;
        _createTableBtn.interactable = hasEnoughMoney;
    }
}