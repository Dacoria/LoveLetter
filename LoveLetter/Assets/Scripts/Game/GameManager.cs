
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int CurrentPlayerIndex;
    private List<PlayerScript> AllPlayers;
    private PlayerScript CurrentPlayer() => AllPlayers[CurrentPlayerIndex];

    public bool GameEnded;

    private void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        // bepaling wanneer alle spelers compleet zijn;
        AllPlayers = GameObject.FindGameObjectsWithTag(Statics.TAG_PLAYER).Select(x => x.GetComponent<PlayerScript>()).ToList();

        DeckManager.instance.CreateDeck();
        DealCardToPlayers();

        GiveCardToCurrentPlayer();
    }

    private void GiveCardToCurrentPlayer()
    {
        DealCardToPlayer(CurrentPlayer());
    }

    public void PlayCard(CharacterType characterType, PlayerScript player)
    {
        if(!GameEnded && player == CurrentPlayer())
        {
            DoCardEffect(characterType, player);
            RemoveCard(characterType, player);
            if (!EndOfGame())
            {
                NextPlayer();
            }
            else
            {
                GameEnded = true;
                CheckWinner();
            }
        }
        else if(GameEnded)
        {
            Debug.Log(player.PlayerName + " wants to do a move, but the game has already ended");
        }
        else
        {
            Debug.Log(player.PlayerName + " wants to do a move, but it is not his turn");
        }
    }

    private void CheckWinner()
    {
        var highestScore = -1;
        List<PlayerScript> playersWithHighestScore = new List<PlayerScript>();

        foreach (PlayerScript player in AllPlayers)
        {
            var cardPlayer = player.CurrentCard1;
            var pointsOfCard = DeckSettings.GetCharacterSettings(cardPlayer.Character.CharacterType).Points;

            if(pointsOfCard > highestScore)
            {
                highestScore = pointsOfCard;
                playersWithHighestScore = new List<PlayerScript> { player };
            }
            else if(pointsOfCard == highestScore)
            {
                playersWithHighestScore.Add(player);
            }
        }

        Debug.Log("GAME ENDED --> " + string.Join(", ", playersWithHighestScore.Select(x => x.PlayerName).ToList()) + " WINS");

    }

    private bool EndOfGame()
    {
        return DeckManager.instance.Deck.Cards.Count(x => x.Status == CardStatus.InDeck) == 0;
    }

    private void RemoveCard(CharacterType characterType, PlayerScript player)
    {
        var card = DeckManager.instance.Deck.Cards.First(x => x.Player == player && x.Character.CharacterType == characterType);

        card.Player = null;
        card.Status = CardStatus.InDiscard;

        player.CardPlayed(characterType);
    }

    private void NextPlayer()
    {
        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % AllPlayers.Count;
        if (DeckManager.instance.Deck.Cards.Count(x => x.Status == CardStatus.InDeck)  > 0)
        {
            GiveCardToCurrentPlayer();
        }
    }

    private void DoCardEffect(CharacterType characterType, PlayerScript player)
    {
        //throw new NotImplementedException();
    }

    private void DealCardToPlayers()
    {
        foreach (var player in AllPlayers)
        {
            DealCardToPlayer(player);
        }
    }

    private void DealCardToPlayer(PlayerScript player)
    {
        player.DrawCard();
    }
}
