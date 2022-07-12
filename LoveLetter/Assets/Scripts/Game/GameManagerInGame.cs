using System.Collections;
using System.Collections.Generic;
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
        if(!RoundEnded && playerId == CurrentPlayer().PlayerId && !MonoHelper.Instance.GetModal().IsActive)
        {            
            DoCardEffect(cardId, playerId);            
        }       
        else if(RoundEnded)
        {
            Debug.Log(playerId.GetPlayer().PlayerName + " wants to do a move, but the round has already ended");
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
            Textt.ActionLocal("Not allowed to play " + cardId.GetCard().Character.Type);
        }
    }

    public void CardEffectPlayed(int cardId, int playerId)
    {
        StartCoroutine(ProcessCardEffectPlayedAfterXSeconds(cardId, playerId, 1.5f));
    }

    public IEnumerator ProcessCardEffectPlayedAfterXSeconds(int cardId, int playerId, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Deck.instance.DiscardCardSync(cardId, cardIsPlayed: true);
        NetworkActionEvents.instance.EndCharacterEffect(playerId, cardId.GetCard().Character.Type, cardId);

        StartCoroutine(NextTurnInXSec(2.5f));
    }

    private IEnumerator NextTurnInXSec(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        TryNextTurn();
    }

    private void TryNextTurn()
    {
        if (!IsEndOfRound())
        {
            NextTurn();
        }
        else
        {
            DoEndOfRound();
        }
    }

    private void DoEndOfRound()
    {
        RoundEnded = true;
        var winners = CheckWinners();

        foreach (var winnerPId in winners)
        {
            winnerPId.Key.GetPlayer().Score += winnerPId.Value; // wordt gesynct automagisch na een change                
        }

        var roundEnded = new RoundEnded { PlayerScores = new List<PlayerScore>() };
        foreach (var player in NetworkHelper.Instance.GetPlayers())
        {
            roundEnded.PlayerScores.Add(
                    new PlayerScore
                    {
                        PlayerId = player.PlayerId,
                        PlayerScorePoints = player.Score,
                        WonRound = winners.Keys.Contains(player.PlayerId)
                    }
                );
        }

        NetworkActionEvents.instance.RoundEnded(roundEnded);
    }

    private void NextTurn()
    {
        do
        {
            CurrentPlayerId = NextPlayerId();
        }
        while (CurrentPlayer().PlayerStatus == PlayerStatus.Intercepted);

        CurrentPlayer().PlayerStatus = PlayerStatus.Normal;

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