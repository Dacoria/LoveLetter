
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{

    public bool GameEnded;
  
    private List<int> CheckWinners()
    {
        var highestScore = -1;
        List<int> playersWithHighestScore = new List<int>();

        foreach (PlayerScript player in AllPlayers.Where(x => x.PlayerStatus != PlayerStatus.Intercepted))
        {
            var cardPlayer = player.CurrentCard1() ?? player.CurrentCard2();
            var pointsOfCard = DeckSettings.GetCharacterSettings(cardPlayer.Character.Type).Points;

            if (pointsOfCard > highestScore)
            {
                highestScore = pointsOfCard;
                playersWithHighestScore = new List<int> { player.PlayerId };
            }
            else if (pointsOfCard == highestScore)
            {
                playersWithHighestScore.Add(player.PlayerId);
            }
        }


        var playersLeft = AllPlayers.Where(x => x.PlayerStatus != PlayerStatus.Intercepted).ToList();
        var extraSpyText = "";
        if (playersLeft.Count() == 1 && PlayersWhoDiscardedSpies.Any(x => x == playersLeft[0].PlayerId))
        {
            extraSpyText = " + Spy bonus";
        }       

        Text.GameSync("Game Ended - " + string.Join(", ", playersWithHighestScore.Select(x => x.GetPlayer().PlayerName).ToList()) + " Wins!" + extraSpyText);


        return playersWithHighestScore;
    }

    private void OnGameEnded(List<int> obj)
    {
        GameEnded = true;
    }


    private bool EndOfGame()
    {
        if (AllPlayers.Count(x => x.PlayerStatus != PlayerStatus.Intercepted) <= 1)
        {
            return true;
        }

        if (Deck.instance.Cards.Count(x => x.Status == CardStatus.InDeck) == 0)
        {
            return true;
        }

        return false;
    }
}