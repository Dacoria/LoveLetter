using System.Collections.Generic;
using System.Linq;

public class ChancellorEffect : ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Chancellor;

    private PlayerScript currentPlayer;
    private int currentCardId;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        currentPlayer = player;
        currentCardId = cardId;

        var modalGo = MonoHelper.Instance.GetModal();

        var cardsInDeck = DeckManager.instance.Deck.Cards.Where(x => x.Status == CardStatus.InDeck).ToList();
        if (cardsInDeck.Count() >= 2)
        {
            DeckManager.instance.PlayerDrawsCardFromPile(player);
            DeckManager.instance.PlayerDrawsCardFromPile(player);
        }
        else if (cardsInDeck.Count() == 1)
        {
            DeckManager.instance.PlayerDrawsCardFromPile(player);
        }
        else
        {
            MonoHelper.Instance.SetActionText("Chancellor has no cards to get from the deck");
            GameManager.instance.CardEffectPlayed(cardId, player);
            return true;
        }

        cardOptions = DeckManager.instance.Deck.Cards.Where(x => x?.Player == player && x.Id != currentCardId).Select(x => x.Character.CharacterType.ToString()).ToList();
        modalGo.SetOptions(ChooseCardAtBottom, "Choose card to put at bottom of pile", cardOptions);

        return true;
    }

    private List<string> cardOptions;

    public void ChooseCardAtBottom(string optionCardAtBottom)
    {
        var cardToPutAtBottom = DeckManager.instance.Deck.Cards.First(x => x?.Player == currentPlayer && x.Id != currentCardId && x.Character.CharacterType.ToString() == optionCardAtBottom);

        cardToPutAtBottom.Status = CardStatus.InDeck;
        DeckManager.instance.Deck.Cards.Remove(cardToPutAtBottom);
        DeckManager.instance.Deck.Cards.Add(cardToPutAtBottom);

        cardOptions.Remove(cardOptions.First(x => x == optionCardAtBottom));

        if(cardOptions.Count > 1)
        {
            var modalGo = MonoHelper.Instance.GetModal();
            modalGo.SetOptions(ChooseCardAtBottom, "Again, choose card to put at bottom of pile", cardOptions);
            return;
        }

        MonoHelper.Instance.SetActionText("Chancellor has placed card(s) at the bottom of the pile");
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

