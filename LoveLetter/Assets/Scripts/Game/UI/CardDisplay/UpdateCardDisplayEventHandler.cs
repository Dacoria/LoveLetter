using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdateCardDisplayEventHandler : MonoBehaviour
{
    private void Start()
    {
        ActionEvents.NewRoundStarted += OnNewRoundStarted;
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
        ActionEvents.RoundEnded += OnRoundEnded;
        ActionEvents.CardSynced += OnCardSynced;
    }

    private void OnCardSynced()
    {
        UpdateCardDisplay();
    }

    private void OnRoundEnded(RoundEnded roundEnded)
    {
        UpdateCardDisplay();
    }

    private void OnNewPlayerTurn(int obj)
    {
        UpdateCardDisplay();
    }

    private void OnNewRoundStarted(List<int> a, int b)
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
        ActionEvents.NewRoundStarted -= OnNewRoundStarted;
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
        ActionEvents.RoundEnded -= OnRoundEnded;
        ActionEvents.CardSynced -= OnCardSynced;
    }
}