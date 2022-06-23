
using Photon.Pun;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private List<PlayerScript> AllPlayers;
    public List<int> PlayersWhoDiscardedSpies;

    private void Awake()
    {
        instance = this;
        Text.GameLocal("Waiting for game to start");
        this.ComponentInject();
    }

    private void Start()
    {
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
        ActionEvents.NewGameStarted += OnNewGameStarted;
    }

    private void OnDestroy()
    {
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
        ActionEvents.NewGameStarted -= OnNewGameStarted;
    }    
}