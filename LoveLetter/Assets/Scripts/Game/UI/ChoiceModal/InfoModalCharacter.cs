using System;
using UnityEngine;

public class InfoModalCharacter : MonoBehaviour
{

    [ComponentInject] private ModalOptionScript modalOptionScript;
    private CharacterType characterTypeOfModalOption;

    void Awake()
    {
        this.ComponentInject();        
    }

    private void Start()
    {
        var valueOfModal = modalOptionScript.Text.text;

        foreach (CharacterType type in Enum.GetValues(typeof(CharacterType)))
        {
            if (valueOfModal == type.ToString())
            {
                characterTypeOfModalOption = type;
                modalOptionScript.SetCountInDeck(DeckSettings.GetCharacterSettings(type).CountInDeck);
                return;
            }
        }

        // Geen char gevonden -> destroy info button
        Destroy(gameObject);
    }

    public void ClickOnButton()
    {
        BigCardHandler.instance.ShowBigCardNoButtons(characterTypeOfModalOption);
    }
}
