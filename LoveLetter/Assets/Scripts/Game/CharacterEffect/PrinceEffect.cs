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
            Text.ActionSync("Prince played...");
            modalGo.SetOptions(ChoosePlayer, "Choose who should discard his/her card", players);
        }
        else
        {
            Text.ActionSync("Priest played, noone to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer);
        }

        return true;
    }

    private bool CanDoEffect(PlayerScript player, int cardId)
    {
        var otherCardOfCurrentPlayer = GetOtherCard(player, cardId);

        if (otherCardOfCurrentPlayer.Character.Type == CharacterType.Countess)
        {
            return false;
        }        

        return true;
    }

    private Card GetOtherCard(PlayerScript player, int cardId)
    {
        return Deck.instance.Cards.First(x => x?.Player == player && x.Id != cardId);
    }

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        var currentCardPlayer = Deck.instance.Cards.Single(x => x?.Player?.PlayerName == optionSelectedPlayer && x.Id != currentCardId);
        var playerOfCard = currentCardPlayer.Player;

        currentCardPlayer.Status = CardStatus.InDiscard;

        if(Deck.instance.Cards.Any(x => x.Status == CardStatus.InDeck))
        {
            Deck.instance.PlayerDrawsCardFromPile(playerOfCard);
            Text.ActionSync(optionSelectedPlayer + " discarded card " + currentCardPlayer.Character.Type + " & Got a new card");
        }
        else
        {
            Text.ActionSync(optionSelectedPlayer + " discarded card " + currentCardPlayer.Character.Type + ". No other cards left");
        }
        
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

