using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;
    public Deck Deck;


    public void Awake()
    {
        instance = this;
    }

    public void CreateDeck()
    {
        Deck = DeckSettings.CreateNewDeck();
    }
}

public class Deck
{
    public List<Character> Characters;
    public Character RemovedCharacter;
}

public static class DeckSettings
{
    public static Deck CreateNewDeck()
    {
        var result = new List<Character>();
        foreach(CharacterType characterType in Enum.GetValues(typeof(CharacterType)))
        {
            var charSettings = GetCharacterSettings(characterType);
            for(int i = 0; i < charSettings.CountInDeck; i++)
            {
                var newChar = CreateNewCharacter(characterType);
                result.Add(newChar);
            }
        }

        result.Shuffle();
        var removedChar = result[0];
        result.Remove(removedChar);

        return new Deck
        {
            Characters = result,
            RemovedCharacter = removedChar
        };
    }

    public static Character CreateNewCharacter(CharacterType characterType)    
    {
        var result = new Character
        {
            CharacterType = characterType,
            Sprite = MonoHelper.Instance.GetCharacterSprite(characterType),
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
        { CharacterType.Spy,        new CharacterSettings { CountInDeck = 5, Points = 0 } },
        { CharacterType.Guard,      new CharacterSettings { CountInDeck = 5, Points = 1 } },
        { CharacterType.Priest,     new CharacterSettings { CountInDeck = 2, Points = 2 } },
        { CharacterType.Baron,      new CharacterSettings { CountInDeck = 2, Points = 3 } },
        { CharacterType.Handmaid,   new CharacterSettings { CountInDeck = 2, Points = 4 } },
        { CharacterType.Prince,     new CharacterSettings { CountInDeck = 2, Points = 5 } },
        { CharacterType.Chancellor, new CharacterSettings { CountInDeck = 2, Points = 6 } },
        { CharacterType.King,       new CharacterSettings { CountInDeck = 1, Points = 7 } },
        { CharacterType.Countess,   new CharacterSettings { CountInDeck = 1, Points = 8 } },
        { CharacterType.Princess,   new CharacterSettings { CountInDeck = 1, Points = 9 } }
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

public class Character
{
    public CharacterType CharacterType;
    public string Description;
    public Sprite Sprite;
}

public class CharacterSettings
{
    public int Points;
    public int CountInDeck;
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