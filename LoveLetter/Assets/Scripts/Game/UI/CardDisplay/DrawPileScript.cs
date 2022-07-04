using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DrawPileScript : UpdateCardDisplayMonoBehaviourAbstract
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;

    public TMP_Text DeckText;

    private void Start()
    {
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
        UpdateCardDisplay();
    }
        
    private void OnDestroy()
    {
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
    }
    public override Transform GetLocationVisibleCardOnTop()
    {
        var cardOnTop = Card3.activeSelf ? Card3 :
                        Card2.activeSelf ? Card2 :
                        Card1.activeSelf ? Card1 :
                        Card1; // dan maar zo

        return cardOnTop.transform;
    }

    public override void UpdateCardDisplay()
    {
        if (Deck.instance.Cards != null)
        {
            var deckCount = Deck.instance.Cards.Count(x => x.Status == CardStatus.InDeck);
            DeckText.text = "Draw pile (" + deckCount + ")";

            Card1.SetActive(deckCount >= 1);
            Card2.SetActive(deckCount >= 2);
            Card3.SetActive(deckCount >= 3);
        }
        else
        {
            DeckText.text = "";

            Card1.SetActive(false);
            Card2.SetActive(false);
            Card3.SetActive(false);
        }
    }

    private void OnNewPlayerTurn(int pId)
    {
        canClickOnDeck = true;        
    }

    private bool canClickOnDeck;

    public void OnDeckCardClick()
    {
        if(!GameManager.instance.RoundEnded && canClickOnDeck)
        {
            if(PhotonNetwork.OfflineMode)
            {
                Deck.instance.PlayerDrawsCardFromPileSync(GameManager.instance.CurrentPlayer().PlayerId);
            }
            else if(GameManager.instance.CurrentPlayer().PlayerId == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Deck.instance.PlayerDrawsCardFromPileSync(GameManager.instance.CurrentPlayer().PlayerId);
            }

            canClickOnDeck = false;
        }
    }
}