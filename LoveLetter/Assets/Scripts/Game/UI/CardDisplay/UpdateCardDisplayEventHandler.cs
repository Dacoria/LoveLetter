using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdateCardDisplayEventHandler : MonoBehaviour
{
    private void Start()
    {
        ActionEvents.NewGameStarted += OnNewGameStarted;
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
        ActionEvents.GameEnded += OnGameEnded;
        ActionEvents.CardSynced += OnCardSynced;
    }

    private void OnCardSynced()
    {
        UpdateCardDisplay();
    }

    private void OnGameEnded(List<int> obj)
    {
        UpdateCardDisplay();
    }

    private void OnNewPlayerTurn(int obj)
    {
        UpdateCardDisplay();
    }

    private void OnNewGameStarted(List<int> a, int b)
    {
        UpdateCardDisplay();
    }

    private void UpdateCardDisplay()
    {
        var cardsToUpdate = FindObjectsOfType<UpdateCardDisplayMonoBehaviourAbstract>().ToList(); // niet in var --> nieuwe players hebben ook een deck

        foreach (var cardToUpdate in cardsToUpdate)
        {
            cardToUpdate.UpdateCardDisplay();
        }
    }

    private void OnDestroy()
    {
        ActionEvents.NewGameStarted -= OnNewGameStarted;
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
        ActionEvents.GameEnded -= OnGameEnded;
        ActionEvents.CardSynced -= OnCardSynced;
    }
}