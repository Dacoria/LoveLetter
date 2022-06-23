
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
        var modalGo = MonoHelper.Instance.GetModal();


        var otherPlayers = NetworkHelper.Instance.GetOtherPlayersScript(player).Where(x => x.PlayerStatus == PlayerStatus.Normal).Select(x => x.PlayerName).ToList();
        if (otherPlayers.Any())
        {
            Text.ActionSync("King played...");
            modalGo.SetOptions(ChoosePlayer, "Choose who to trade cards with", otherPlayers);
        }
        else
        {
            Text.ActionSync("King played, noone to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer.PlayerId);
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
        return Deck.instance.Cards.First(x => x?.PlayerId.GetPlayer() == player && x.Id != cardId);
    }

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        var currentCardOtherPlayer = Deck.instance.Cards.Single(x => x?.PlayerId.GetPlayer()?.PlayerName == optionSelectedPlayer);
        var yourOtherCard = GetOtherCard(currentPlayer, currentCardId);

        Deck.instance.SetPlayerId(yourOtherCard.Id, currentCardOtherPlayer.PlayerId);
        Deck.instance.SetPlayerId(currentCardOtherPlayer.Id, currentPlayer.PlayerId);

        Text.ActionSync(currentPlayer.PlayerName + " swapped cards with " + optionSelectedPlayer);
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer.PlayerId);
    }
}

