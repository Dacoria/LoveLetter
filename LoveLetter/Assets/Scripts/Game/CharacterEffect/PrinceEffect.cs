using System.Linq;

public class PrinceEffect : ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Prince;

    private PlayerScript currentPlayer;
    private int currentCardId;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        if (!CanDoEffect(player, cardId))
        {
            return false;
        }

        currentPlayer = player;
        currentCardId = cardId;
        var modalGo = MonoHelper.Instance.GetModal();

        var players = NetworkHelper.Instance.GetPlayers().Where(x => x.PlayerStatus == PlayerStatus.Normal).Select(x => x.PlayerName).ToList();

        if (players.Any())
        {
            modalGo.SetOptions(ChoosePlayer, "Choose who should discard his/her card", players);
        }
        else
        {
            MonoHelper.Instance.SetActionText("Noone to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer);
        }

        return true;
    }

    private bool CanDoEffect(PlayerScript player, int cardId)
    {
        var otherCardOfCurrentPlayer = GetOtherCard(player, cardId);

        if (otherCardOfCurrentPlayer.Character.CharacterType == CharacterType.Countess)
        {
            return false;
        }        

        return true;
    }

    private Card GetOtherCard(PlayerScript player, int cardId)
    {
        return DeckManager.instance.Deck.Cards.First(x => x?.Player == player && x.Id != cardId);
    }

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        var currentCardPlayer = DeckManager.instance.Deck.Cards.Single(x => x?.Player?.PlayerName == optionSelectedPlayer && x.Id != currentCardId);
        var playerOfCard = currentCardPlayer.Player;

        currentCardPlayer.Status = CardStatus.InDiscard;

        if(DeckManager.instance.Deck.Cards.Any(x => x.Status == CardStatus.InDeck))
        {
            DeckManager.instance.PlayerDrawsCardFromPile(playerOfCard);
            MonoHelper.Instance.SetActionText(optionSelectedPlayer + " discarded his/her hand & got a new card");
        }
        else
        {
            MonoHelper.Instance.SetActionText(optionSelectedPlayer + " discarded his/her hand. No other cards left");
        }
        
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

