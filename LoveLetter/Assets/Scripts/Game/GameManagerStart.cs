
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        if(NetworkHelper.Instance.GetPlayers().Count() <= 1)
        {
            Debug.Log("Minstens 2 spelers om een spel te starten");
            return;
        }

        ResetGame();

        Deck.instance.CreateDeck();

        StartNewGameForPlayers();
        GiveCardToCurrentPlayer();

        ActionEvents.NewGameStarted?.Invoke();
    }

    private void ResetGame()
    {
        // bepaling wanneer alle spelers compleet zijn;
        AllPlayers = NetworkHelper.Instance.GetPlayers();
        foreach (var player in AllPlayers)
        {
            player.PlayerStatus = PlayerStatus.Normal;
        }

        CurrentPlayerIndex = 0;
        GameEnded = false;
        PlayersWhoDiscardedSpies = new List<PlayerScript>();
        Text.ActionSync(""); // clear actions
    }

    private void StartNewGameForPlayers()
    {
        foreach (var player in AllPlayers)
        {
            DealCardToPlayer(player);
        }
    }
}