
using System.Linq;

public class KingEffect: ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.King;

    private PlayerScript currentPlayer;
    private int currentCardId;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        if(!CanDoEffect(player, cardId))
        {
            return false;
        }

        currentPlayer = player;
        currentCardId = cardId;
        var modalGo = MonoHelper.Instance.GetModal();


        var otherPlayers = NetworkHelper.Instance.GetOtherPlayers(player).Where(x => x.PlayerStatus == PlayerStatus.Normal).Select(x => x.PlayerName).ToList();
        if (otherPlayers.Any())
        {
            modalGo.SetOptions(ChoosePlayer, "Choose who to trade cards with", otherPlayers);
        }
        else
        {
            MonoHelper.Instance.SetActionText("No player to select");
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
        var currentCardOtherPlayer = DeckManager.instance.Deck.Cards.Single(x => x?.Player?.PlayerName == optionSelectedPlayer);
        var yourOtherCard = GetOtherCard(currentPlayer, currentCardId);

        yourOtherCard.Player = currentCardOtherPlayer.Player;
        currentCardOtherPlayer.Player = currentPlayer;

        MonoHelper.Instance.SetActionText(currentPlayer.PlayerName + " swapped cards with " + optionSelectedPlayer);
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

