
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
            modalGo.SetOptions(ChoosePlayer, "Choose who to compare rank with", otherPlayers);
        }
        else
        {
            MonoHelper.Instance.SetActionText("Noone to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer);
        }

        return true;
    }

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        var currentCardOtherPlayer = DeckManager.instance.Deck.Cards.Single(x => x?.Player?.PlayerName == optionSelectedPlayer);
        var yourPoints = DeckSettings.GetCharacterSettings(CharacterType).Points;
        var otherPoints = DeckSettings.GetCharacterSettings(currentCardOtherPlayer.Character.CharacterType).Points;


        if (yourPoints == otherPoints)
        {
            MonoHelper.Instance.SetActionText("Baron is of the same rank! Nothing happens");
        }
        else if (yourPoints > otherPoints)
        {
            currentCardOtherPlayer.Player.PlayerStatus = PlayerStatus.Intercepted;
            MonoHelper.Instance.SetActionText("Baron is ranked higher! " + currentCardOtherPlayer.Player.PlayerName + " is now intercepted");
        }
        else if (yourPoints < otherPoints)
        {
            currentPlayer.PlayerStatus = PlayerStatus.Intercepted;
            MonoHelper.Instance.SetActionText("Baron is ranked lower! " + currentPlayer.PlayerName + " is now intercepted");
        }

        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

