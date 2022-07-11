using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    public List<Card> Cards { get; private set; }

    [ComponentInject] private PhotonView photonView;

    public void Awake()
    {
        instance = this;
        this.ComponentInject();
    }
      
    public void PlayerDrawsCardFromPileSync(int playerId)
    {
        var cardToDeal = instance.Cards.First(x => x.Status == CardStatus.InDeck);

        cardToDeal.Status = CardStatus.InPlayerHand;
        cardToDeal.PlayerId = playerId;
        cardToDeal.IndexOfCardInHand = 1;

        var otherCardOfPlayer = Cards.FirstOrDefault(x => x.PlayerId == playerId);
        if(otherCardOfPlayer != null && otherCardOfPlayer.IndexOfCardInHand == 1)
        {
            cardToDeal.IndexOfCardInHand = 2;
        }

        ActionEvents.DeckCardDrawn?.Invoke(playerId);

        SyncCard(cardToDeal);
        ActionEvents.CardSynced?.Invoke();
    }

    public void PlayerDrawsCardFromExcludePileSync(int playerId)
    {
        var cardToDeal = instance.Cards.First(x => x.Status == CardStatus.Excluded);

        cardToDeal.Status = CardStatus.InPlayerHand;
        cardToDeal.PlayerId = playerId;
        cardToDeal.IndexOfCardInHand = 1;

        var otherCardOfPlayer = Cards.FirstOrDefault(x => x.PlayerId == playerId);
        if (otherCardOfPlayer != null && otherCardOfPlayer.IndexOfCardInHand == 1)
        {
            cardToDeal.IndexOfCardInHand = 2;
        }

        SyncCard(cardToDeal);
        ActionEvents.CardSynced?.Invoke();
    }

    public void SetPlayerId(int cardId, int playerId)
    {
        var card = Cards.Single(x => x.Id == cardId);
        if(card.PlayerId == playerId)
        {
            return;
        }

        card.PreviousPlayerId = card.PlayerId;
        card.PreviousIndexOfCardInHand = card.IndexOfCardInHand;

        card.PlayerId = playerId;
        card.IndexOfCardInHand = 1;

        var otherCardOfPlayer = Cards.FirstOrDefault(x => x.PlayerId == playerId);
        if (otherCardOfPlayer != null && otherCardOfPlayer.IndexOfCardInHand == 1)
        {
            card.IndexOfCardInHand = 2;
        }

        SyncCard(card);
    }

    public void CreateDeckSync()
    {
        Cards = DeckSettings.CreateNewDeck(); 
        SyncInitialDeck();        
    }

    public void PutCardAtBottom(int cardId)
    {
        var card = cardId.GetCard();
        card.Status = CardStatus.InDeck;
        Cards = Cards.OrderBy(x => x.Id == cardId ? 1 : 0).ToList();
        SyncCard(card, setToEndOfList: true);
    }

    public void DiscardCardSync(int cardId, bool cardIsPlayed)
    {
        var card = cardId.GetCard();
        card.Status = CardStatus.InDiscard;
        card.CardIsPlayed = cardIsPlayed;
        SyncCard(card);
    }

    public void SetCardIsPlayed(int cardId)
    {
        var card = cardId.GetCard();
        card.CardIsPlayed = true;
    }

    private void SyncInitialDeck()
    {
        var cardsToSend = JsonUtility.ToJson(new CardWrapper { Cards = Cards});
        photonView.RPC("RPC_SyncDeck", RpcTarget.Others, cardsToSend);

        foreach(var card in Cards.Where(x => x.Status != CardStatus.InDeck))
        {
            // enum wil niet lekker syncen (voor o.a. excluded) --> daarom zo
            SyncCard(card);
        }
    }

    [PunRPC]
    public void RPC_SyncDeck(string cardsJson)
    {
        var cardWrapper = JsonUtility.FromJson<CardWrapper>(cardsJson);
        Cards = cardWrapper.Cards;
    }

    private void SyncCard(Card card, bool setToEndOfList = false)
    {
        photonView.RPC("RPC_SyncCard", RpcTarget.Others, card.Id, card.Status, card.PlayerId, card.IndexOfCardInHand, card.CardIsPlayed, card.PreviousPlayerId, card.PreviousIndexOfCardInHand, setToEndOfList);
    }

    [PunRPC]
    public void RPC_SyncCard(int cardId, CardStatus cardStatus, int playerId, int indexOfCardInHand, bool cardIsPlayed, int previousPlayerId, int previousIndexOfCardInHand, bool setToEndOfList)
    {
        StartCoroutine(SyncCardIfDeckAvailable(cardId, cardStatus, playerId, indexOfCardInHand, cardIsPlayed, previousPlayerId, previousIndexOfCardInHand, setToEndOfList));
    }

    private IEnumerator SyncCardIfDeckAvailable(int cardId, CardStatus cardStatus, int playerId, int indexOfCardInHand, bool cardIsPlayed, int previousPlayerId, int previousIndexOfCardInHand, bool setToEndOfList)
    {
        var counter = 0;

        do
        {
            if (Cards != null)
            {
                var card = cardId.GetCard();

                card.Status = cardStatus;
                card.PlayerId = playerId;
                card.IndexOfCardInHand = indexOfCardInHand;
                card.CardIsPlayed = cardIsPlayed;
                card.PreviousPlayerId = previousPlayerId;
                card.PreviousIndexOfCardInHand= previousIndexOfCardInHand;

                if (setToEndOfList)
                {
                    Cards = Cards.OrderBy(x => x.Id == cardId ? 1 : 0).ToList();
                }
            }
            else
            {
                counter++;
                yield return new WaitForSeconds(0.1f);
            }
        } while (Cards == null && counter < 20);

        if(counter >= 20)
        {
            throw new Exception("No deck found!");
        }

        ActionEvents.CardSynced?.Invoke();        
    }   
}

[Serializable]
public class CardWrapper
{
    public List<Card> Cards;
}