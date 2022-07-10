
using System.Collections.Generic;
using System.Linq;

public class PriestEffect : CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Priest;

    private PlayerScript currentPlayer;
    private int currentCardId;

    public override bool CanDoEffect(PlayerScript player, int cardId) => true;
    public override bool DoEffect(PlayerScript player, int cardId)
    {
        currentPlayer = player;
        currentCardId = cardId;



        var otherPlayers = NetworkHelper.Instance.GetOtherPlayersScript(player).Where(x => x.PlayerStatus == PlayerStatus.Normal).Select(x => x.PlayerName).ToList();
        if (otherPlayers.Any())
        {
            Textt.ActionSync("Priest played...");
            MonoHelper.Instance.DoCharacterChoice(currentPlayer, ChoosePlayer, "Choose who's card to look at", otherPlayers, CharacterType, currentCardId);
        }
        else
        {
            Textt.ActionSync("Priest played, no one to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer.PlayerId);
        }

        return true;
    }

    private int watchedCardId;

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        var currentCardOtherPlayer = Deck.instance.Cards.Single(x => x?.PlayerId.GetPlayer()?.PlayerName == optionSelectedPlayer);
        watchedCardId = currentCardOtherPlayer.Id;

        ActionEvents.StartShowCardEffect(currentPlayer.PlayerId, CharacterType, currentCardId, watchedCardId);
        Textt.ActionSync("Priest watches the card of " + optionSelectedPlayer);
        Textt.ActionLocal("Card in hand of " + optionSelectedPlayer + " is " + currentCardOtherPlayer.Character.Type);

        MonoHelper.Instance.DoCharacterChoice(currentPlayer, CardWatched, "Finished watching?", new List<string> { "Yes" }, CharacterType, currentCardId);
    }

    public void CardWatched(string res)
    {
        ActionEvents.EndShowCardEffect(currentPlayer.PlayerId, CharacterType, currentCardId, watchedCardId);
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer.PlayerId);
    }
}

