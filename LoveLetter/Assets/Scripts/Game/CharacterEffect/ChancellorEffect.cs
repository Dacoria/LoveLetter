using System.Collections.Generic;
using System.Linq;

public class ChancellorEffect : CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Chancellor;

    private PlayerScript currentPlayer;
    private int currentCardId;

    public override bool DoEffect(PlayerScript player, int cardId)
    {
        currentPlayer = player;
        currentCardId = cardId;

        var modalGo = MonoHelper.Instance.GetModal();

        var cardsInDeck = Deck.instance.Cards.Where(x => x.Status == CardStatus.InDeck).ToList();
        if (cardsInDeck.Count() >= 2)
        {
            Deck.instance.PlayerDrawsCardFromPile(player);
            Deck.instance.PlayerDrawsCardFromPile(player);
        }
        else if (cardsInDeck.Count() == 1)
        {
            Deck.instance.PlayerDrawsCardFromPile(player);
        }
        else
        {
            Text.ActionSync("Chancellor has no cards to get from the deck");
            GameManager.instance.CardEffectPlayed(cardId, player);
            return true;
        }

        cardOptions = Deck.instance.Cards.Where(x => x?.PlayerId.GetPlayer() == player && x.Id != currentCardId).Select(x => x.Character.Type.ToString()).ToList();
        modalGo.SetOptions(ChooseCardAtBottom, "Choose card to put at bottom of pile", cardOptions);
        
        Text.ActionSync("Chancellor played...");
        return true;
    }

    private List<string> cardOptions;

    public void ChooseCardAtBottom(string optionCardAtBottom)
    {
        var cardToPutAtBottom = Deck.instance.Cards.First(x => x?.PlayerId.GetPlayer() == currentPlayer && x.Id != currentCardId && x.Character.Type.ToString() == optionCardAtBottom);

        cardToPutAtBottom.Status = CardStatus.InDeck;
        Deck.instance.Cards.Remove(cardToPutAtBottom);
        Deck.instance.Cards.Add(cardToPutAtBottom);

        cardOptions.Remove(cardOptions.First(x => x == optionCardAtBottom));

        if(cardOptions.Count > 1)
        {
            var modalGo = MonoHelper.Instance.GetModal();
            modalGo.SetOptions(ChooseCardAtBottom, "Again, choose card to put at bottom of pile", cardOptions);
            return;
        }

        Text.ActionSync("Chancellor has placed card(s) at the bottom of the pile");
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

