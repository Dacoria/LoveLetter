
using System.Linq;

public class BaronEffect : ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Baron;

    private PlayerScript currentPlayer;
    private int currentCardId;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        currentPlayer = player;
        currentCardId = cardId;

        var modalGo = MonoHelper.Instance.GetModal();


        var otherPlayers = NetworkHelper.Instance.GetOtherPlayers(player).Where(x => x.PlayerStatus == PlayerStatus.Normal).Select(x => x.PlayerName).ToList();
        if (otherPlayers.Any())
        {
            Text.ActionSync("Baron played...");
            modalGo.SetOptions(ChoosePlayer, "Choose who to compare rank with", otherPlayers);
        }
        else
        {
            Text.ActionSync("Baron played, but noone to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer);
        }

        return true;
    }

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        var currentCardOtherPlayer = Deck.instance.Cards.Single(x => x?.Player?.PlayerName == optionSelectedPlayer);
        var yourPoints = DeckSettings.GetCharacterSettings(CharacterType).Points;
        var otherPoints = DeckSettings.GetCharacterSettings(currentCardOtherPlayer.Character.Type).Points;


        if (yourPoints == otherPoints)
        {
            Text.ActionSync("Baron is of the same rank! Nothing happens");
        }
        else if (yourPoints > otherPoints)
        {
            Text.ActionSync("Baron is ranked higher! " + currentCardOtherPlayer.Player.PlayerName + " is now intercepted");
            currentCardOtherPlayer.Player.PlayerStatus = PlayerStatus.Intercepted;
            
        }
        else if (yourPoints < otherPoints)
        {
            Text.ActionSync("Baron is ranked lower! " + currentPlayer.PlayerName + " is now intercepted");
            currentPlayer.PlayerStatus = PlayerStatus.Intercepted;            
        }

        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

