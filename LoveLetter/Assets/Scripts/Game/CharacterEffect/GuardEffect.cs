
using System;
using System.Linq;

public class GuardEffect: CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Guard;

    public override bool CanDoEffect(PlayerScript player, int cardId) => true;

    private PlayerScript currentPlayer;
    private int currentCardId;

    public override bool DoEffect(PlayerScript player, int cardId)
    {
        currentPlayer = player;
        currentCardId = cardId;

        var otherPlayers = NetworkHelper.Instance.GetOtherPlayersScript(player).Where(x => x.PlayerStatus == PlayerStatus.Normal).Select(x => x.PlayerName).ToList();
        if(otherPlayers.Any())
        {
            Textt.ActionSync("Guard played...");
            MonoHelper.Instance.DoCharacterChoice(currentPlayer, ChoosePlayer, "Choose player...", otherPlayers, CharacterType, currentCardId);
        }
        else
        {
            Textt.ActionSync("Guard played, no one to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer.PlayerId);
        }


        return true;
    }

    private string selectedPlayerName;

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        selectedPlayerName = optionSelectedPlayer;
        Textt.ActionSync("Guard selects " + selectedPlayerName + " ...");

        var options = MonoHelper.Instance.GetCharacterTypes().Where(x => x != CharacterType).Select(x => x.ToString()).ToList();
        MonoHelper.Instance.DoCharacterChoice(currentPlayer, ChooseCharacterType, "Choose Character to inspect", options, CharacterType, currentCardId);
    }

    public void ChooseCharacterType(string optionSelectedCharacterType)
    {
        var selectedCharacterType = Enum.Parse<CharacterType>(optionSelectedCharacterType);
        var currentCardPlayer = Deck.instance.Cards.Single(x => x?.PlayerId.GetPlayer()?.PlayerName == selectedPlayerName);

        if(currentCardPlayer.Character.Type == selectedCharacterType)
        {
            Textt.ActionSync("Guard chose right! " + currentCardPlayer.PlayerId.GetPlayer().PlayerName + " has a " + optionSelectedCharacterType + " and is now out of the round");
            currentCardPlayer.PlayerId.GetPlayer().PlayerStatus = PlayerStatus.Intercepted;            
        }
        else
        {
            Textt.ActionSync("Guard chose wrong! " + currentCardPlayer.PlayerId.GetPlayer().PlayerName + " does not have a " + optionSelectedCharacterType);
        }

        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer.PlayerId);
    }
}