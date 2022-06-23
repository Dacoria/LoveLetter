using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DiscardPileScript : UpdateCardDisplayMonoBehaviourAbstract
{
    public SpriteRenderer Card1Sprite;
    public SpriteRenderer Card2Sprite;
    public SpriteRenderer Card3Sprite;

    public Sprite BackgroundCard;

    public TMP_Text Text;

    private void Start()
    {
        UpdateCardDisplay();       
    }

    public override void UpdateCardDisplay()
    {
       if(Deck.instance.Cards != null)
        {
            var deckDiscarded = Deck.instance.Cards.Where(x => x.Status == CardStatus.InDiscard);
            var deckDiscardedCount = deckDiscarded.Count();

            
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
                cardOnTop.sprite = MonoHelper.Instance.GetCharacterSprite(deckDiscarded.OrderByDescending(x => x.StatusChangeTime).First().Character.Type);
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
