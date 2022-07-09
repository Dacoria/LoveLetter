
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
            if (currentPlayer.IsAi)
            {
                currentPlayer.GetComponent<AiPlayerScript>().DoCardChoice(ChoosePlayer, otherPlayers, CharacterType, currentCardId);
            }
            else
            {
                var modalGo = MonoHelper.Instance.GetModal();
                modalGo.SetOptions(ChoosePlayer, "Choose who to compare rank with", otherPlayers);
            }
        }
        else
        {
            Textt.ActionSync("Baron played, but noone to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer.PlayerId);
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
        var otherPoints = DeckSettings.GetCharacterSettings(currentCardOtherPlayer.Character.Type).Points;

        var yourOtherCard = GetOtherCard(currentPlayer, currentCardId);
        var yourPoints = DeckSettings.GetCharacterSettings(yourOtherCard.Character.Type).Points;

        if (yourPoints == otherPoints)
        {
            Textt.ActionSync("Baron picks " + optionSelectedPlayer + ". The ranks are the same! Nothing happens");
        }
        else if (yourPoints > otherPoints)
        {
            Textt.ActionSync("Baron picks " + optionSelectedPlayer + ". Card of " + currentPlayer.PlayerName + " is ranked higher! " + currentCardOtherPlayer.PlayerId.GetPlayer().PlayerName + " is now intercepted");
            currentCardOtherPlayer.PlayerId.GetPlayer().PlayerStatus = PlayerStatus.Intercepted;
            
        }
        else if (yourPoints < otherPoints)
        {
            Textt.ActionSync("Baron picks " + optionSelectedPlayer + ". Card of " + currentCardOtherPlayer.PlayerId.GetPlayer().PlayerName + " is ranked higher! " + currentPlayer.PlayerName + " is now intercepted");
            currentPlayer.PlayerStatus = PlayerStatus.Intercepted;            
        }

        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer.PlayerId);
    }
}