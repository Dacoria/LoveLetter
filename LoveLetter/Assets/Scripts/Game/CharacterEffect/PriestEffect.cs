
using System.Linq;

public class PriestEffect : ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Priest;

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
            modalGo.SetOptions(ChoosePlayer, "Choose who's card to look at", otherPlayers);
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

        MonoHelper.Instance.SetActionText("Card in hand of " + optionSelectedPlayer + " is " + currentCardOtherPlayer.Character.CharacterType);
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

