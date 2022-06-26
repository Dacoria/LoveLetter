using System.Linq;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    private int CurrentPlayerId;
    public PlayerScript CurrentPlayer() => AllPlayers.Single(x => x.PlayerId == CurrentPlayerId);

    private void OnNewPlayerTurn(int playerId)
    {
        CurrentPlayerId = playerId;
    }

    public void PlayCard(int cardId, int playerId)
    {
        if(!GameEnded && playerId == CurrentPlayer().PlayerId && !MonoHelper.Instance.GetModal().IsActive)
        {            
            DoCardEffect(cardId, playerId);            
        }       
        else if(GameEnded)
        {
            Debug.Log(playerId.GetPlayer().PlayerName + " wants to do a move, but the game has already ended");
        }
        else
        {
            Debug.Log(playerId.GetPlayer().PlayerName + " wants to do a move, but it is not his turn");
        }
    }

    private void DoCardEffect(int cardId, int playerId)
    {
        var card = cardId.GetCard();
        var charSettings = DeckSettings.GetCharacterSettings(card.Character.Type);

        Debug.Log(playerId.GetPlayer().PlayerName + " - DoCardEffect -> " + card.Character.Type);

        var isEffectAllowed = charSettings.CharacterEffect.DoEffect(playerId.GetPlayer() , cardId);
        if (isEffectAllowed)
        {
            NetworkActionEvents.instance.StartCharacterEffect(playerId, card.Character.Type, cardId);
        }
        else 
        { 
            Text.ActionLocal("Not allowed to play " + cardId.GetCard().Character.Type);
        }
    }

    public void CardEffectPlayed(int cardId, int playerId)
    {
        Deck.instance.DiscardCardSync(cardId, cardIsPlayed: true);
        NetworkActionEvents.instance.EndCharacterEffect(playerId, cardId.GetCard().Character.Type, cardId);

        if (!EndOfGame())
        {
            NextPlayer();
        }
        else
        {
            GameEnded = true;
            var winners = CheckWinners();
            NetworkActionEvents.instance.GameEnded(winners);
        }
    }

    private void NextPlayer()
    {
        do
        {
            CurrentPlayerId = NextPlayerId();
        }
        while (CurrentPlayer().PlayerStatus == PlayerStatus.Intercepted);

        CurrentPlayer().PlayerStatus = PlayerStatus.Normal;
        Deck.instance.PlayerDrawsCardFromPileSync(CurrentPlayer().PlayerId);

        NetworkActionEvents.instance.NewPlayerTurn(CurrentPlayer().PlayerId);
    }

    private int NextPlayerId()
    {
        var foundCurrPlayer = false;

        for(int i = 0; i < AllPlayers.Count(); i++)
        {
            var player = AllPlayers[i];
            if (foundCurrPlayer)
            {
                return player.PlayerId;
            }
            
            if(player.PlayerId == CurrentPlayerId)
            {
                foundCurrPlayer = true;
            }
        }

        return AllPlayers[0].PlayerId;
    }
}