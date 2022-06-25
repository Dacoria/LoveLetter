using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ExcludedPileScript : UpdateCardDisplayMonoBehaviourAbstract, IOnCardMouseDownEvent
{
    public SpriteRenderer Card1Sprite;
    public TMP_Text Text;

    private void Start()
    {
        UpdateCardDisplay();
        ActionEvents.GameEnded += OnGameEnded;
        ActionEvents.NewGameStarted += OnNewGameStarted;
    }

    private void OnDestroy()
    {
        ActionEvents.GameEnded -= OnGameEnded;
        ActionEvents.NewGameStarted -= OnNewGameStarted;
    }

    private void OnNewGameStarted(List<int> arg1, int arg2)
    {
        Card1Sprite.sprite = MonoHelper.Instance.BackgroundCardSprite;
    }

    private void OnGameEnded(List<int> obj)
    {
        var deckExclusions = Deck.instance.Cards.Where(x => x.Status == CardStatus.Excluded);
        Card1Sprite.sprite = MonoHelper.Instance.GetCharacterSprite(Deck.instance.Cards.First(x => x.Status == CardStatus.Excluded).Character.Type);        
    }

    public override void UpdateCardDisplay()
    {
        if (Deck.instance.Cards != null)
        {
            var exclusionCount = Deck.instance.Cards.Count(x => x.Status == CardStatus.Excluded);            
            
            Text.text = "Excluded pile (" + exclusionCount + ")";
            Card1Sprite.gameObject.SetActive(exclusionCount >= 1);
        }
        else
        {
            Text.text = "";
            Card1Sprite.gameObject.SetActive(false);
        }
    }

    public void OnCardMouseDownEvent()
    {        
        if (Card1Sprite.sprite != MonoHelper.Instance.BackgroundCardSprite)
        {
            MonoHelper.Instance.ShowBigCard(Deck.instance.Cards.First(x => x.Status == CardStatus.Excluded).Character.Type);
        }
    }
}
