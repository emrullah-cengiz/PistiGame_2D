using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VContainer;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class CreateTablePopup : InfoPopup
{
    [Inject] private PlayerWallet _playerWallet;

    [SerializeField] private Slider _betAmountSlider;
    [SerializeField] private ToggleGroup _gameModeToggleGroup;
    [SerializeField] private TMP_Text _minBetAmountText, _maxBetAmountText, _sliderValueText;

    private new Action<TableData> _callback;
    private RoomData _roomData;

    private Toggle[] _toggles;

    private void Start()
    {
        _toggles = _gameModeToggleGroup.GetComponentsInChildren<Toggle>();
    }

    public void Show(RoomData roomData, Action<TableData> callback = null, string message = null)
    {
        base.Show();

        _roomData = roomData;
        _callback = callback;

        _minBetAmountText.text = _roomData.BetRange.x.ToAbbreviated(" $");
        _maxBetAmountText.text = _roomData.BetRange.y.ToAbbreviated(" $");

        _betAmountSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        okBtn.interactable = _playerWallet.HasEnoughBalance((int)value);

        _sliderValueText.text = ((int)value).ToAbbreviated(" $");
    }

    protected override void OkBtn_Click()
    {
        base.OkBtn_Click();

        _callback?.Invoke(new TableData()
        {
            RoomData = _roomData,
            BetAmount = (int)_betAmountSlider.value,
            //_gameModeToggleGroup.GetFirstActiveToggle() not working
            Mode = (TableMode)_toggles.FirstOrDefault(x => x.isOn).transform.GetSiblingIndex()
        });
    }
}