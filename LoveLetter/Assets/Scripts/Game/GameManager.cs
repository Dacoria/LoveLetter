
using Photon.Pun;
using System;
using System.Collections;
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
        Textt.GameLocal("Waiting for game to start");
        this.ComponentInject();
    }

    private void Start()
    {
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
        ActionEvents.NewRoundStarted += OnNewRoundStarted;
        ActionEvents.RoundEnded += OnRoundEnded;
        ActionEvents.GameEnded += OnGameEnded;
        StartCoroutine(ShowMenuLocationDialogInXSeconds(0.9f));
    }

    private IEnumerator ShowMenuLocationDialogInXSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        MonoHelper.Instance.ShowOkDiaglogMessage("Menu", "Menu can be found in the left upper corner (Rose)");
    }

    private void OnDestroy()
    {
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
        ActionEvents.NewRoundStarted -= OnNewRoundStarted;
        ActionEvents.RoundEnded -= OnRoundEnded;
        ActionEvents.GameEnded -= OnGameEnded;
    }
}