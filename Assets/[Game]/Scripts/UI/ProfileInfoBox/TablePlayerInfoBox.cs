using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class TablePlayerInfoBox : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerName, _score;

    public void RefreshView(TablePlayerData playerTableData)
    {
        _playerName.text = playerTableData.Name;
        _score.text = playerTableData.Score.ToString();
        // _score.text = _score.text.Replace("0", "O");
    }

}