
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public void StartRound()
    {
        if(NetworkHelper.Instance.GetPlayers().Count() <= 1)
        {
            Debug.Log("Minstens 2 spelers om een spel te starten");
            return;
        }

        ResetGameLocal();

        Deck.instance.CreateDeckSync();
        DrawCardsForPlayersSync();

        NetworkActionEvents.instance.NewRoundStarted(AllPlayers.Select(x => x.PlayerId).ToList(), CurrentPlayer().PlayerId);
    }    

    private void OnNewRoundStarted(List<int> playerIds, int currentPlayerId)
    {
        ResetGameLocal();

        var playersMatch = playerIds.All(AllPlayers.Select(x => x.PlayerId).Contains);
        var sameSize = playerIds.Count == AllPlayers.Count;
        
        // fix order....
        var players = new List<PlayerScript>();
        for (int i = 0; i < playerIds.Count; i++)
        {
            var pId = playerIds[i];
            players.Add(AllPlayers.Single(x => x.PlayerId == pId));
        }

        AllPlayers = players;


        if (playersMatch && sameSize)
        {
            CurrentPlayerId = currentPlayerId;
            ActionEvents.NewPlayerTurn?.Invoke(currentPlayerId); // local; want OnNewRoundStarted is al via sync gegaan; status bekend
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
        RoundEnded = false;
        PlayersWhoDiscardedSpies = new List<int>();
        Textt.ActionSync(""); // clear actions
    }

    private void DrawCardsForPlayersSync()
    {
        foreach (var player in AllPlayers)
        {
            Deck.instance.PlayerDrawsCardFromPileSync(player.PlayerId);
        }
    }
}