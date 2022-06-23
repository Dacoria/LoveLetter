using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatusColorSetter : MonoBehaviour
{
    [ComponentInject] private TMP_Text playerNameText;
    [ComponentInject] private PlayerScript player;

    private void Awake()
    {
        this.ComponentInject();
    }

    void Start()
    {
        ActionEvents.PlayerStatusChange += OnPlayerStatusChange;
        ActionEvents.NewGameStarted += OnNewGameStarted;
        ActionEvents.GameEnded += OnGameEnded;
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
    }
    private void OnNewPlayerTurn(int currentPlayerId)
    {
        playerNameText.fontStyle = player.PlayerId == currentPlayerId ? FontStyles.Underline : FontStyles.Normal;
    }

    private void OnNewGameStarted(List<int> a, int b)
    {
        playerNameText.color = Color.white;
    }

    private void OnGameEnded(List<int> playersWon)
    {
        playerNameText.fontStyle = FontStyles.Normal;
        if (playersWon.Any(x => x == player.PlayerId))
        {
            playerNameText.color = Color.green;
        }
        else if(playerNameText.color == Color.blue)
        {
            playerNameText.color = Color.white;
        }
    }

    private void OnPlayerStatusChange(int pIdStatusChanged, PlayerStatus playerStatus)
    {
        Debug.Log("OnPlayerStatusChange --> " + player.PlayerName + " " + player.PlayerStatus);
        if(pIdStatusChanged != player.PlayerId)
        {
            return;
        }

        switch (playerStatus)
        {
            case PlayerStatus.Normal:
                playerNameText.color = Color.white;
                break;
            case PlayerStatus.Protected:
                playerNameText.color = Color.blue;
                break;
            case PlayerStatus.Intercepted:
                playerNameText.color = Color.red;
                break;
            default:
                throw new Exception(player.PlayerStatus + " onbekend");
        }
    }

    private void OnDestroy()
    {
        ActionEvents.PlayerStatusChange -= OnPlayerStatusChange;
        ActionEvents.NewGameStarted -= OnNewGameStarted;
        ActionEvents.GameEnded -= OnGameEnded;
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
    }
}
