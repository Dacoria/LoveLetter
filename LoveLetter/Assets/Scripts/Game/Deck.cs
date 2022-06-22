using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    public List<Card> Cards;

    [ComponentInject] private PhotonView photonView;

    public void Awake()
    {
        instance = this;
        this.ComponentInject();
    }
      
    public Card PlayerDrawsCardFromPile(PlayerScript player)
    {
        var cardToDeal = instance.Cards.First(x => x.Status == CardStatus.InDeck);

        cardToDeal.Status = CardStatus.InPlayerHand;
        cardToDeal.PlayerId = player.PlayerId;
        cardToDeal.IndexOfCardInHand = instance.Cards.Count(x => x.PlayerId.GetPlayer() == player);

        return cardToDeal;
    }

    public void CreateDeck()
    {
        Cards = DeckSettings.CreateNewDeck();
        SyncDeck();
    }

    public void SyncDeck()
    {
        var cardsToSend = JsonUtility.ToJson(new CardWrapper { Cards = Cards});
        photonView.RPC("RPC_SyncDeck", RpcTarget.Others, cardsToSend);
    }

    [PunRPC]
    public void RPC_SyncDeck(string cardsJson)
    {
        var cardWrapper = JsonUtility.FromJson<CardWrapper>(cardsJson);
        Cards = cardWrapper.Cards;
    }
}

[Serializable]
public class CardWrapper
{
    public List<Card> Cards;
}

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

                if(_status == CardStatus.InPlayerHand)
                {
                    if(Character.Type == CharacterType.Spy)
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

public enum CardStatus
{
    InDeck,
    InPlayerHand,
    InDiscard,
    Excluded
}

public static class DeckSettings
{
    public static List<Card> CreateNewDeck()
    {
        var characters = new List<Character>();
        foreach(CharacterType characterType in Enum.GetValues(typeof(CharacterType)))
        {
            var charSettings = GetCharacterSettings(characterType);
            for(int i = 0; i < charSettings.CountInDeck; i++)
            {
                var newChar = CreateNewCharacter(characterType);
                characters.Add(newChar);
            }
        }

        characters.Shuffle();
        var cards = characters.Select(character => new Card { Character = character, Status = CardStatus.InDeck }).ToList();
        
        for(int i = 0; i < cards.Count;i++)
        {
            cards[i].Id = i;
        }

        cards[0].Status = CardStatus.Excluded;
        return cards;
    }

    public static Character CreateNewCharacter(CharacterType characterType)    
    {
        var result = new Character
        {
            Type = characterType,
            Description = GetCharacterDescription(characterType)
        };

        return result;
    }

    private static string GetCharacterDescription(CharacterType characterType)
    {
        switch (characterType)
        {
            case CharacterType.Spy:
                return "A Spy has no effect when played or discarded. At the end of the round, if you are the only player still in the round who played or discarded a Spy, you gain one favor token.";
            case CharacterType.Guard:
                return "Choose another player and name a character other than Guard. If the chosen player has that card in their hand, they are out of the round.";
            case CharacterType.Priest:
                return "Choose another player and secretly look at their hand (without revealing it to anyone else).";
            case CharacterType.Baron:
                return "Choose another player. You and that player secretly compare your hands. Whoever has the lowervalue card is out of the round. If there is a tie, neither player is out of the round.";
            case CharacterType.Handmaid:
                return "Until the start of your next turn, other players cannot choose you for their card effects.";
            case CharacterType.Prince:
                return "Choose any player (including yourself). That player discards their hand (without resolving its effect) and draws a new hand. If the deck is empty, the chosen player draws the facedown set-aside card.";
            case CharacterType.Chancellor:
                return "Draw two cards from the deck into your hand. Choose and keep one of the three cards now in your hand, and place the other two facedown on the bottom of the deck in any order. If there is only one card in the deck, draw it and return one card. If there are no cards left, this card is played with no effect.";
            case CharacterType.King:
                return "Choose another player and trade hands with that player.";
            case CharacterType.Countess:
                return "The Countess has no effect when played or discarded. You must play the Countess as the card for your turn if either the King or a Prince is the other card in your hand.";
            case CharacterType.Princess:
                return "If you either play or discard the Princess for any reason, you are immediately out of the round.";
            default:
                throw new Exception();
        }

    }

    

    public static Dictionary<CharacterType, CharacterSettings> CharacterCountInDeck = new Dictionary<CharacterType, CharacterSettings>()
    {
        { CharacterType.Spy,        new CharacterSettings { CountInDeck = 2, Points = 0, CharacterEffect = new SpyEffect()          } },
        { CharacterType.Guard,      new CharacterSettings { CountInDeck = 6, Points = 1, CharacterEffect = new GuardEffect()        } },
        { CharacterType.Priest,     new CharacterSettings { CountInDeck = 2, Points = 2, CharacterEffect = new PriestEffect()       } },
        { CharacterType.Baron,      new CharacterSettings { CountInDeck = 2, Points = 3, CharacterEffect = new BaronEffect()        } },
        { CharacterType.Handmaid,   new CharacterSettings { CountInDeck = 2, Points = 4, CharacterEffect = new HandmaidEffect()     } },
        { CharacterType.Prince,     new CharacterSettings { CountInDeck = 2, Points = 5, CharacterEffect = new PrinceEffect()       } },
        { CharacterType.Chancellor, new CharacterSettings { CountInDeck = 2, Points = 6, CharacterEffect = new ChancellorEffect()   } },
        { CharacterType.King,       new CharacterSettings { CountInDeck = 1, Points = 7, CharacterEffect = new KingEffect()         } },
        { CharacterType.Countess,   new CharacterSettings { CountInDeck = 1, Points = 8, CharacterEffect = new CountessEffect()     } },
        { CharacterType.Princess,   new CharacterSettings { CountInDeck = 1, Points = 9, CharacterEffect = new PrincessEffect()     } }
    };

    public static CharacterSettings GetCharacterSettings(CharacterType characterType)
    {
        if (CharacterCountInDeck.ContainsKey(characterType))
        {
            return CharacterCountInDeck[characterType];
        }

        return null;
    }
}

[Serializable]
public class Character
{
    public CharacterType Type;
    public string Description;
}

public class CharacterSettings
{
    public int Points;
    public int CountInDeck;
    public ICharacterEffect CharacterEffect;
}

public enum CharacterType
{
    Spy,
    Guard,
    Priest,
    Baron,
    Handmaid,
    Prince,
    Chancellor,
    King,
    Countess,
    Princess
}