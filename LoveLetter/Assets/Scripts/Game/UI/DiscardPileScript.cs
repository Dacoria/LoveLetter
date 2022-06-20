using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardPileScript : MonoBehaviour
{
    public SpriteRenderer Card1Sprite;
    public SpriteRenderer Card2Sprite;
    public SpriteRenderer Card3Sprite;

    public Sprite BackgroundCard;

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
            var deckDiscarded = DeckManager.instance.Deck.Cards.Where(x => x.Status == CardStatus.InDiscard);
            var deckDiscardedCount = deckDiscarded.Count();

            if (deckDiscardedCount != previousDeckCount)
            {
                Text.text = "Discard pile (" + deckDiscardedCount + ")";

                Card1Sprite.gameObject.SetActive(deckDiscardedCount >= 1);
                Card2Sprite.gameObject.SetActive(deckDiscardedCount >= 2);
                Card3Sprite.gameObject.SetActive(deckDiscardedCount >= 3);

                Card1Sprite.sprite = BackgroundCard;
                Card2Sprite.sprite = BackgroundCard;
                Card3Sprite.sprite = BackgroundCard;

                if (deckDiscardedCount > 0)
                {
                    var cardOnTop = deckDiscardedCount == 1 ? Card1Sprite : deckDiscardedCount == 2 ? Card2Sprite : Card3Sprite;
                    cardOnTop.sprite = deckDiscarded.OrderByDescending(x => x.StatusChangeTime).First().Character.Sprite;
                }


                previousDeckCount = deckDiscardedCount;
            }
        }
        else
        {
            Text.text = "";

            Card1Sprite.gameObject.SetActive(false);
            Card2Sprite.gameObject.SetActive(false);
            Card3Sprite.gameObject.SetActive(false);
        }
    }
}
