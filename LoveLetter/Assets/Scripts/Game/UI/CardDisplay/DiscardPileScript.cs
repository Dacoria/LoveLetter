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

            Card1Sprite.sprite = MonoHelper.Instance.BackgroundCardSprite;
            Card2Sprite.sprite = MonoHelper.Instance.BackgroundCardSprite;
            Card3Sprite.sprite = MonoHelper.Instance.BackgroundCardSprite;

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

    private CharacterType GetLastDiscardedCharacter() => Deck.instance.Cards.Where(x => x.Status == CardStatus.InDiscard).OrderByDescending(x => x.StatusChangeTime).First().Character.Type;

    public void OnCardMouseDownEvent()
    {
        var cardOnTop = Card3Sprite.gameObject.activeSelf ? Card3Sprite :
                        Card2Sprite.gameObject.activeSelf ? Card2Sprite :
                        Card1Sprite.gameObject.activeSelf ? Card1Sprite :
                        null;
        if(cardOnTop != null)
        {
            MonoHelper.Instance.ShowBigCard(GetLastDiscardedCharacter());
        }
    }
}
