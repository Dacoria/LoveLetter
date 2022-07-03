using System;
using System.Collections;
using System.Collections.Generic;
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
                return;
            }
        }

        // Geen char gevonden -> destroy info button
        Destroy(gameObject);
    }

    public void ClickOnButton()
    {
        MonoHelper.Instance.ShowBigCard(characterTypeOfModalOption);
    }
}
