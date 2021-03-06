
using System.Linq;

public class KingEffect: CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.King;

    private PlayerScript currentPlayer;
    private int currentCardId;

   public override bool DoEffect(PlayerScript player, int cardId)
    {
        if(!CanDoEffect(player, cardId))
        {
            return false;
        }

        currentPlayer = player;
        currentCardId = cardId;

        var otherPlayers = NetworkHelper.Instance.GetOtherPlayersScript(player).Where(x => x.PlayerStatus == PlayerStatus.Normal).Select(x => x.PlayerName).ToList();
        if (otherPlayers.Any())
        {
            Textt.ActionSync("King played...");
            MonoHelper.Instance.DoCharacterChoice(currentPlayer, ChoosePlayer, "Choose who to trade cards with", otherPlayers, CharacterType, currentCardId);
        }
        else
        {
            Textt.ActionSync("King played, no one to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer.PlayerId);
        }

        return true;
    }

    public override bool CanDoEffect(PlayerScript player, int cardId)
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
        return Deck.instance.Cards.First(x => x?.PlayerId.GetPlayer() == player && x.Id != cardId);
    }

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        var currentCardOtherPlayer = Deck.instance.Cards.Single(x => x?.PlayerId.GetPlayer()?.PlayerName == optionSelectedPlayer);
        var yourOtherCard = GetOtherCard(currentPlayer, currentCardId);

        Deck.instance.SetPlayerId(yourOtherCard.Id, currentCardOtherPlayer.PlayerId);
        Deck.instance.SetPlayerId(currentCardOtherPlayer.Id, currentPlayer.PlayerId);

        Textt.ActionSync(currentPlayer.PlayerName + " swapped cards with " + optionSelectedPlayer);
        NetworkActionEvents.instance.CardsSwitched(yourOtherCard.Id, currentCardOtherPlayer.Id);
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer.PlayerId);
    }
}