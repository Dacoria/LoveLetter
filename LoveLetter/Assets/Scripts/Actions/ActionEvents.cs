using System;
using System.Collections.Generic;

public static class ActionEvents
{
    public static Action NewGameStarted;
    public static Action NewPlayerTurn;
    public static Action<PlayerScript, CharacterType, int> StartCharacterEffect;
    public static Action<PlayerScript, CharacterType, int> EndCharacterEffect;
    public static Action<PlayerScript, PlayerStatus> PlayerStatusChange;
    public static Action<List<PlayerScript>> GameEnded;
}