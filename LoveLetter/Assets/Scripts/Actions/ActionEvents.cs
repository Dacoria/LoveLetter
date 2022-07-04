using System;
using System.Collections.Generic;

public static class ActionEvents
{
    // LET OP - LOCAL --> voor netwerk, zie NetworkActionEvents
    public static Action<List<int>, int> NewRoundStarted;
    public static Action CardSynced;
    public static Action<int> NewPlayerTurn;
    public static Action<int, CharacterType, int> StartCharacterEffect;
    public static Action<int, CharacterType, int> EndCharacterEffect;
    public static Action<int, PlayerStatus> PlayerStatusChange;
    public static Action<int, int> PlayerScoreChange;
    public static Action<RoundEnded> RoundEnded;

    // only local
    public static Action<int> DeckCardDrawn;
    public static Action<int, CharacterType, int, int> StartShowCardEffect;
    public static Action<int, CharacterType, int, int> EndShowCardEffect;
    public static Action GameEnded;


    // animatie events
    public static Action<List<int>> CardsToDeck;
    public static Action<int> CardDiscarded;
    public static Action<int, int> CardsSwitched;

}