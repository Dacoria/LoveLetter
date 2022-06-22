
using System.Linq;

public class PriestEffect : CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Priest;

    private PlayerScript currentPlayer;
    private int currentCardId;

    public override bool DoEffect(PlayerScript player, int cardId)
    {
        currentPlayer = player;
        currentCardId = cardId;

        var modalGo = MonoHelper.Instance.GetModal();


        var otherPlayers = NetworkHelper.Instance.GetOtherPlayers(player).Where(x => x.PlayerStatus == PlayerStatus.Normal).Select(x => x.PlayerName).ToList();
        if (otherPlayers.Any())
        {
            Text.ActionSync("Priest played...");
            modalGo.SetOptions(ChoosePlayer, "Choose who's card to look at", otherPlayers);
        }
        else
        {
            Text.ActionSync("Priest played, noone to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer);
        }

        return true;
    }

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        var currentCardOtherPlayer = Deck.instance.Cards.Single(x => x?.PlayerId.GetPlayer()?.PlayerName == optionSelectedPlayer);

        Text.ActionSync("Priest watches the card of " + optionSelectedPlayer);
        Text.ActionLocal("Card in hand of " + optionSelectedPlayer + " is " + currentCardOtherPlayer.Character.Type);
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

