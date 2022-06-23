using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class GameTexts : MonoBehaviour
{
    public TMP_Text GameText;
    public TMP_Text ActionText;

    private void Start()
    {
        ActionEvents.NewGameStarted += OnNewGameStarted;
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
    }

    private void OnDestroy()
    {
        ActionEvents.NewGameStarted -= OnNewGameStarted;
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
    }

    private void OnNewPlayerTurn(int playerId)
    {
        UpdateTurnCurrentPlayerGameText(playerId);
    }

    private void OnNewGameStarted(List<int> listPlayers, int playerId)
    {
        UpdateTurnCurrentPlayerGameText(playerId);
    }

    private void UpdateTurnCurrentPlayerGameText(int playerId)
    {
        GameText.text = NetworkHelper.Instance.GetMyPlayerScript().PlayerId == playerId ? "Your turn" : "Turn: " + NetworkHelper.Instance.GetPlayerScriptById(playerId).PlayerName;

    }
}

