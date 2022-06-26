using System;

[Serializable]
public class Card
{
    public int Id;

    // alleen gevuld als een player deze kaart in de hand heeft, anders 0
    public int PlayerId;
    public int IndexOfCardInHand; // player kan meerdere kaarten in hand hebben --> dit bepaalt de index daarvan (1, 2)

    // voor de king switch
    public int PreviousPlayerId;
    public int PreviousIndexOfCardInHand;

    public bool CardIsPlayed;

    public Character Character;
    public DateTime StatusChangeTime { get; private set; }
    private CardStatus _status;
    public CardStatus Status
    {
        get => _status;
        set
        {
            StatusChangeTime = DateTime.Now;
            if (_status != value)
            {
                if (Status == CardStatus.InDiscard && Character.Type == CharacterType.Princess)
                {
                    PlayerId.GetPlayer().PlayerStatus = PlayerStatus.Intercepted;
                }

                if (_status == CardStatus.InPlayerHand)
                {
                    if (Character.Type == CharacterType.Spy)
                    {
                        GameManager.instance.PlayersWhoDiscardedSpies.Add(PlayerId);
                    }

                    PlayerId = -1;
                }
                _status = value;
                
            }
        }
    }
}