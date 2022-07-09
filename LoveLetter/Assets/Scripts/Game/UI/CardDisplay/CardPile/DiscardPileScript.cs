using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DiscardPileScript : UpdateCardDisplayMonoBehaviourAbstract, IOnCardMouseDownEvent
{
    public SpriteRenderer Card1Sprite;
    public SpriteRenderer Card2Sprite;
    public SpriteRenderer Card3Sprite;

    public TMP_Text Text;


    private void Awake()
    {
        CardIdsBeingMoved = new List<int>();
    }

    private void Start()
    {
        UpdateCardDisplay();       
    }

    public override Transform GetLocationVisibleCardOnTop()
    {
        var cardOnTop = Deck.instance.Cards.Count(card => card.Status == CardStatus.InDiscard) >= 3 ? Card3Sprite :
                        Deck.instance.Cards.Count(card => card.Status == CardStatus.InDiscard) == 2 ? Card2Sprite :
                        Card1Sprite; // dan maar zo

        return cardOnTop.transform;
    }

    public override void UpdateCardDisplay()
    {
       if(Deck.instance.Cards != null)
        {
            var deckDiscarded = Deck.instance.Cards.Where(card => card.Status == CardStatus.InDiscard && !CardIdsBeingMoved.Any(cardIdsMoved => card.Id == cardIdsMoved)).OrderByDescending(x => x.StatusChangeTime).ToList();
            var deckDiscardedCount = deckDiscarded.Count();
            
            Text.text = "Discarded (" + deckDiscardedCount + ")";

            UpdateCardDisplay(Card1Sprite, deckDiscarded, 1);
            UpdateCardDisplay(Card2Sprite, deckDiscarded, 2);
            UpdateCardDisplay(Card3Sprite, deckDiscarded, 3);


            if (deckDiscardedCount > 0)
            {
                var cardOnTop = deckDiscardedCount == 1 ? Card1Sprite : deckDiscardedCount == 2 ? Card2Sprite : Card3Sprite;
                cardOnTop.sprite = MonoHelper.Instance.GetCharacterSprite(GetLastDiscardedCharacter());
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

    private void UpdateCardDisplay(SpriteRenderer spriteRenderer, List<Card> deckDiscarded, int number)
    {
        spriteRenderer.gameObject.SetActive(deckDiscarded.Count >= number);
        if(deckDiscarded.Count >= number)
        {
            spriteRenderer.sprite = MonoHelper.Instance.GetCharacterSprite(deckDiscarded[number - 1].Character.Type);
        }
    }

    private CharacterType GetLastDiscardedCharacter() => Deck.instance.Cards
        .Where(card =>
            card.Status == CardStatus.InDiscard &&
            !CardIdsBeingMoved.Any(cardIdsMoved => card.Id == cardIdsMoved))
        .OrderByDescending(x => x.StatusChangeTime)
        .First().Character.Type;

    public void OnCardMouseDownEvent()
    {
        var cardOnTop = Card3Sprite.gameObject.activeSelf ? Card3Sprite :
                        Card2Sprite.gameObject.activeSelf ? Card2Sprite :
                        Card1Sprite.gameObject.activeSelf ? Card1Sprite :
                        null;
        if(cardOnTop != null)
        {
            BigCardHandler.instance.ShowBigCardNoButtons(GetLastDiscardedCharacter());
        }
    }
}