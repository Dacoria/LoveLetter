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
        cardIdsPutAtBottom = new List<int>();

        var modalGo = MonoHelper.Instance.GetModal();

        var cardsInDeck = Deck.instance.Cards.Where(x => x.Status == CardStatus.InDeck).ToList();
        if (cardsInDeck.Count() >= 2)
        {
            Deck.instance.PlayerDrawsCardFromPileSync(player.PlayerId);
            Deck.instance.PlayerDrawsCardFromPileSync(player.PlayerId);
        }
        else if (cardsInDeck.Count() == 1)
        {
            Deck.instance.PlayerDrawsCardFromPileSync(player.PlayerId);
        }
        else
        {
            Textt.ActionSync("Chancellor has no cards to get from the deck");
            GameManager.instance.CardEffectPlayed(cardId, player.PlayerId);
            return true;
        }

        cardOptions = Deck.instance.Cards.Where(x => x?.PlayerId.GetPlayer() == player && x.Id != currentCardId).Select(x => x.Character.Type.ToString()).ToList();
        modalGo.SetOptions(ChooseCardAtBottom, "Choose card to keep", cardOptions);
        
        Textt.ActionSync("Chancellor played...");
        return true;
    }

    private List<string> cardOptions;
    private List<int> cardIdsPutAtBottom;

    public void ChooseCardAtBottom(string optionCardAtBottom)
    {
        var remainingCardsOfPlayer = Deck.instance.Cards.Where(x => x?.PlayerId.GetPlayer() == currentPlayer && x.Id != currentCardId).ToList();
        remainingCardsOfPlayer.Remove(remainingCardsOfPlayer.First(x => x.Character.Type.ToString() == optionCardAtBottom));

        foreach(var cardToPutAtBottomOfDeck in remainingCardsOfPlayer)
        {
            Deck.instance.PutCardAtBottom(cardToPutAtBottomOfDeck.Id);
            cardIdsPutAtBottom.Add(cardToPutAtBottomOfDeck.Id);
        }

        Textt.ActionSync("Chancellor has placed card(s) at the bottom of the pile");
        NetworkActionEvents.instance.CardsToDeck(cardIdsPutAtBottom);
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer.PlayerId);
    }
}

