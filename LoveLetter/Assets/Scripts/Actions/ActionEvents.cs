using System;
using System.Collections.Generic;

public static class ActionEvents
{
    // LET OP - LOCAL --> voor netwerk, zie NetworkActionEvents
    public static Action<List<int>, int> NewGameStarted;
    public static Action CardSynced;
    public static Action<int> NewPlayerTurn;
    public static Action<int, CharacterType, int> StartCharacterEffect;
    public static Action<int, CharacterType, int> EndCharacterEffect;
    public static Action<int, PlayerStatus> PlayerStatusChange;
    public static Action<List<int>> GameEnded;
}