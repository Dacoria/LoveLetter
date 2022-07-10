
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public bool RoundEnded;
    public bool GameEnded;

    public void StopRound()
    {
        RoundEnded = true;
        NetworkActionEvents.instance.RoundEnded(new RoundEnded { PlayerScores = new List<PlayerScore>()});
    }

    private void OnGameEnded()
    {
        GameEnded = true;
    }

    private Dictionary<int, int> CheckWinners()
    {
        var highestScore = -1;
        var playersWithHighestScore = new Dictionary<int, int>();

        foreach (PlayerScript player in AllPlayers.Where(x => x.PlayerStatus != PlayerStatus.Intercepted))
        {
            var cardPlayer = player.CurrentCard1() ?? player.CurrentCard2();
            var pointsOfCard = DeckSettings.GetCharacterSettings(cardPlayer.Character.Type).Points;

            if (pointsOfCard > highestScore)
            {
                highestScore = pointsOfCard;
                playersWithHighestScore = new Dictionary<int, int>();
                playersWithHighestScore.Add(player.PlayerId, 1);
            }
            else if (pointsOfCard == highestScore)
            {
                playersWithHighestScore.Add(player.PlayerId, 1);
            }
        }


        var playersLeft = AllPlayers.Where(x => x.PlayerStatus != PlayerStatus.Intercepted).ToList();
        var extraSpyText = "";
        if (playersLeft.Count() == 1 && PlayersWhoDiscardedSpies.Any(x => x == playersLeft[0].PlayerId))
        {
            playersWithHighestScore = new Dictionary<int, int>();
            playersWithHighestScore.Add(playersLeft.First().PlayerId, 2);
            extraSpyText = " + Spy bonus";
        }       

        Textt.GameSync("Round Ended - " + string.Join(" & ", playersWithHighestScore.Select(x => x.Key.GetPlayer().PlayerName).ToList()) + " Wins!" + extraSpyText);

        return playersWithHighestScore;
    }

    private void OnRoundEnded(RoundEnded roundEnded)
    {
        RoundEnded = true;

        var players = NetworkHelper.Instance.GetPlayers();
        var roseLimitToWin = MonoHelper.Instance.GetRoseCountToWinGame(players.Count());
        var largestScore = roundEnded.PlayerScores.Max(x => x.PlayerScorePoints);

        if(largestScore >= roseLimitToWin)
        {
            MonoHelper.Instance.ShowOkDiaglogMessage("Game Ended", "Winner(s) of the Game:" + string.Join(" & ", roundEnded.PlayerScores.Where(x => x.PlayerScorePoints == largestScore).Select(x => x.PlayerId.GetPlayer().PlayerName).ToList()) + ".The princess has found her partner(s)!", true);
            ActionEvents.GameEnded?.Invoke();
        }
        else
        {
            MonoHelper.Instance.ShowOkDiaglogMessage("Round Ended", "Winner(s) of the Round:" + string.Join(" & ", roundEnded.PlayerScores.Where(x => x.WonRound).Select(x => x.PlayerId.GetPlayer().PlayerName).ToList()) + "!Go to menu to start a new round.", true);
        }
    }


    private bool IsEndOfRound()
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