using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusColorSetter : MonoBehaviour
{
    [ComponentInject] private TMPro.TMP_Text playerNameText;
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
    }

    private void OnNewGameStarted()
    {
        playerNameText.color = Color.white;
    }

    private void OnGameEnded(List<PlayerScript> playersWon)
    {
        if(playersWon.Any(x => x == player))
        {
            playerNameText.color = Color.green;
        }
    }

    private void OnPlayerStatusChange(PlayerScript playerStatusChanged, PlayerStatus previousValue)
    {
        Debug.Log("OnPlayerStatusChange --> " + player.PlayerName + " " + player.PlayerStatus);
        if(playerStatusChanged != player)
        {
            return;
        }

        switch (player.PlayerStatus)
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
    }
}
