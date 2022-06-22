
using System;
using System.Linq;

public class GuardEffect: CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Guard;


    private PlayerScript currentPlayer;
    private int currentCardId;

    public override bool DoEffect(PlayerScript player, int cardId)
    {
        currentPlayer = player;
        currentCardId = cardId;

        var modalGo = MonoHelper.Instance.GetModal();

        var otherPlayers = NetworkHelper.Instance.GetOtherPlayers(player).Where(x => x.PlayerStatus == PlayerStatus.Normal).Select(x => x.PlayerName).ToList();
        if(otherPlayers.Any())
        {
            Text.ActionSync("Guard played...");
            modalGo.SetOptions(ChoosePlayer, "Choose who to intercept", otherPlayers);
        }
        else
        {
            Text.ActionSync("Guard played, noone to select");
            GameManager.instance.CardEffectPlayed(cardId, currentPlayer);
        }


        return true;
    }

    private string selectedPlayerName;

    public void ChoosePlayer(string optionSelectedPlayer)
    {
        selectedPlayerName = optionSelectedPlayer;

        var modalGo = MonoHelper.Instance.GetModal();
        var options = MonoHelper.Instance.GetCharacterTypes().Where(x => x != CharacterType).Select(x => x.ToString()).ToList();
        modalGo.SetOptions(ChooseCharacterType, "Choose Character to intercept", options);
    }


    public void ChooseCharacterType(string optionSelectedCharacterType)
    {
        var selectedCharacterType = Enum.Parse<CharacterType>(optionSelectedCharacterType);
        var currentCardPlayer = Deck.instance.Cards.Single(x => x?.PlayerId.GetPlayer()?.PlayerName == selectedPlayerName);

        if(currentCardPlayer.Character.Type == selectedCharacterType)
        {
            Text.ActionSync("Guard chose right! " + currentCardPlayer.PlayerId.GetPlayer().PlayerName + " has a " + optionSelectedCharacterType + " and is now intercepted");
            currentCardPlayer.PlayerId.GetPlayer().PlayerStatus = PlayerStatus.Intercepted;            
        }
        else
        {
            Text.ActionSync("Guard chose wrong! " + currentCardPlayer.PlayerId.GetPlayer().PlayerName + " does not have a " + optionSelectedCharacterType);
        }

        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

