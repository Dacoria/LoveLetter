using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawPileScript : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;

    public TMP_Text Text;



    private void Awake()
    {
        this.ComponentInject();
    }


    private int previousDeckCount = -1;

    void Update()
    {
       if(DeckManager.instance.Deck != null)
        {
            var deckCount = DeckManager.instance.Deck.Cards.Count(x => x.Status == CardStatus.InDeck);
            if (deckCount != previousDeckCount)
            {
                Text.text = "Draw pile (" + deckCount + ")";

                Card1.SetActive(deckCount >= 1);
                Card2.SetActive(deckCount >= 2);
                Card3.SetActive(deckCount >= 3);

                previousDeckCount = deckCount;
            }
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
