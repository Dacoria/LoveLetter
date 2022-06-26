using System;

[Serializable]
public class Card
{
    public int Id;
    public int PlayerId; // alleen gevuld als een player deze kaart in de hand heeft
    public int IndexOfCardInHand; // player kan meerdere kaarten in hand hebben --> dit bepaalt de index daarvan (0, 1, 2)

    public Character Character;
    public DateTime StatusChangeTime { get; private set; }
    private CardStatus _status;
    public CardStatus Status
    {
        get => _status;
        set
        {
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
                StatusChangeTime = DateTime.Now;

                
            }
        }
    }
}