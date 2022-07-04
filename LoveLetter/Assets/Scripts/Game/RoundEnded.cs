using System;
using System.Collections.Generic;

[Serializable]
public class RoundEnded
{
    // result of all players
    public List<PlayerScore> PlayerScores;
}

[Serializable]
public class PlayerScore
{
    public int PlayerId;
    public int PlayerScorePoints;
    public bool WonRound;
}