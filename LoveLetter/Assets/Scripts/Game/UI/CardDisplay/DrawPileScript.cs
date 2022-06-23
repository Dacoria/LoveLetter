using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DrawPileScript : UpdateCardDisplayMonoBehaviourAbstract
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;

    public TMP_Text Text;

    private void Start()
    {
        UpdateCardDisplay();
    }

    public override void UpdateCardDisplay()
    {
        if (Deck.instance.Cards != null)
        {
            var deckCount = Deck.instance.Cards.Count(x => x.Status == CardStatus.InDeck);
            Text.text = "Draw pile (" + deckCount + ")";

            Card1.SetActive(deckCount >= 1);
            Card2.SetActive(deckCount >= 2);
            Card3.SetActive(deckCount >= 3);
        }
        else
        {
            Text.text = "";

            Card1.SetActive(false);
            Card2.SetActive(false);
            Card3.SetActive(false);
        }
    }
}