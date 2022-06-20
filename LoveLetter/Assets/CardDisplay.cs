using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour
{    
    public Card Card;
    [ComponentInject] private PlayerScript playerScript;

    private void Awake()
    {
        this.ComponentInject();
    }

    void OnMouseDown()
    {
        PlayCard();
    }

    public void PlayCard()
    {
        GameManager.instance.PlayCard(Card.Character.CharacterType, playerScript);
    }
}
