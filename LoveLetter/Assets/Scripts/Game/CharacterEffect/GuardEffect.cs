
using System;
using System.Linq;

public class GuardEffect: ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Guard;


    private PlayerScript currentPlayer;
    private int currentCardId;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        currentPlayer = player;
        currentCardId = cardId;

        var modalGo = MonoHelper.Instance.GetModal();

        var otherPlayers = NetworkHelper.Instance.GetOtherPlayers(player).Where(x => x.PlayerStatus == PlayerStatus.Normal).Select(x => x.PlayerName).ToList();
        if(otherPlayers.Any())
        {
            modalGo.SetOptions(ChoosePlayer, "Choose who to intercept", otherPlayers);
        }
        else
        {
            MonoHelper.Instance.SetActionText("Noone to select");
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
        var currentCardPlayer = DeckManager.instance.Deck.Cards.Single(x => x?.Player?.PlayerName == selectedPlayerName);

        if(currentCardPlayer.Character.CharacterType == selectedCharacterType)
        {
            currentCardPlayer.Player.PlayerStatus = PlayerStatus.Intercepted;
            MonoHelper.Instance.SetActionText("Guard chose right! " + currentCardPlayer.Player.PlayerName + " has a " + optionSelectedCharacterType + " and is now intercepted");
        }
        else
        {
            MonoHelper.Instance.SetActionText("Guard chose wrong! " + currentCardPlayer.Player.PlayerName + " does not have a " + optionSelectedCharacterType);
        }

        GameManager.instance.CardEffectPlayed(currentCardId, currentPlayer);
    }
}

