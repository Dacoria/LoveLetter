
using System;
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

        ResetGameLocal();

        Deck.instance.CreateDeckSync();
        DrawCardsForPlayersSync();
        Deck.instance.PlayerDrawsCardFromPileSync(CurrentPlayer().PlayerId);

        NetworkActionEvents.instance.NewGameStarted(AllPlayers.Select(x => x.PlayerId).ToList(), CurrentPlayer().PlayerId);
    }

    private void OnNewGameStarted(List<int> playerIds, int currentPlayerId)
    {
        ResetGameLocal();

        var playersMatch = playerIds.All(AllPlayers.Select(x => x.PlayerId).Contains);
        var sameSize = playerIds.Count == AllPlayers.Count;

        if(playersMatch && sameSize)
        {
            CurrentPlayerId = currentPlayerId;
            return;
        }
        else
        {
            throw new Exception();
        }
    }

    private void ResetGameLocal()
    {
        // bepaling wanneer alle spelers compleet zijn;
        AllPlayers = NetworkHelper.Instance.GetPlayers();
        foreach (var player in AllPlayers)
        {
            player.PlayerStatus = PlayerStatus.Normal;
        }

        CurrentPlayerId = AllPlayers[0].PlayerId;
        GameEnded = false;
        PlayersWhoDiscardedSpies = new List<int>();
        Text.ActionSync(""); // clear actions
    }

    private void DrawCardsForPlayersSync()
    {
        foreach (var player in AllPlayers)
        {
            Deck.instance.PlayerDrawsCardFromPileSync(player.PlayerId);
        }
    }
}