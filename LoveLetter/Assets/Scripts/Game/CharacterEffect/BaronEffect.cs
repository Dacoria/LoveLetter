
using System.Collections.Generic;
using System.Linq;

public class BaronEffect : CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Baron;

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
            Textt.ActionSync("Baron played...");
            MonoHelper.Instance.DoCharacterChoice(currentPlayer, ChoosePlayer, "Choose who to compare rank with", otherPlayers, CharacterType, currentCardId);
        }
        else
        {
            Textt.ActionSync("Baron played, but no one to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer.PlayerId);
        }

        return true;
    }

    private Card GetOtherCard(PlayerScript player, int cardId)
    {
        return Deck.instance.Cards.First(x => x?.PlayerId.GetPlayer() == player && x.Id != cardId);
    }

    private string optionSelectedPlayer;

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        this.optionSelectedPlayer = optionSelectedPlayer;
        Textt.ActionSync("Baron picks " + optionSelectedPlayer + ". Comparing cards with each other...");

        var currentCardOtherPlayer = Deck.instance.Cards.Single(x => x?.PlayerId.GetPlayer()?.PlayerName == optionSelectedPlayer);
        NetworkActionEvents.instance.StartComparingCards(currentPlayer.PlayerId, currentCardId, currentCardOtherPlayer.PlayerId, currentCardOtherPlayer.Id);

        MonoHelper.Instance.DoCharacterChoice(currentPlayer, CardsCompared, "Finished?", new List<string> { "Yes"}, CharacterType, currentCardId);
    }

    public void CardsCompared(string res)
    {
        var currentCardOtherPlayer = Deck.instance.Cards.Single(x => x?.PlayerId.GetPlayer()?.PlayerName == optionSelectedPlayer);
        var otherPoints = DeckSettings.GetCharacterSettings(currentCardOtherPlayer.Character.Type).Points;

        var yourOtherCard = GetOtherCard(currentPlayer, currentCardId);
        var yourPoints = DeckSettings.GetCharacterSettings(yourOtherCard.Character.Type).Points;

        if (yourPoints == otherPoints)
        {
            Textt.ActionSync("The ranks are the same! Nothing happens");
        }
        else if (yourPoints > otherPoints)
        {
            Textt.ActionSync(currentPlayer.PlayerName + " has a higher rank. " + currentCardOtherPlayer.PlayerId.GetPlayer().PlayerName + " is out of the round");
            currentCardOtherPlayer.PlayerId.GetPlayer().PlayerStatus = PlayerStatus.Intercepted;

        }
        else if (yourPoints < otherPoints)
        {
            Textt.ActionSync(currentCardOtherPlayer.PlayerId.GetPlayer().PlayerName + " has a higher rank. " + currentPlayer.PlayerName + " is out of the round");
            currentPlayer.PlayerStatus = PlayerStatus.Intercepted;
        }

        NetworkActionEvents.instance.FinishedComparingCards(currentPlayer.PlayerId, yourOtherCard.Id, currentCardOtherPlayer.PlayerId, currentCardOtherPlayer.Id);
        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer.PlayerId);
    }
}