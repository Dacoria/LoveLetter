using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerScript : MonoBehaviour, IPunInstantiateMagicCallback
{
    [ComponentInject] private TMP_Text PlayerText;
    public string PlayerName;

    public Card CurrentCard1;
    public Card CurrentCard2;

    private void Awake()
    {
        this.ComponentInject();
        //PlayerText.text = PhotonNetwork.NickName;
        // PlayerName = PhotonNetwork.NickName;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        var name = instantiationData[0].ToString();
        PlayerText.text = name;
        PlayerName = name;
    }

    public void DrawCard()
    {
        var card = DeckManager.instance.PlayerDrawsCardFromPile(this);
        Debug.Log(PlayerName + " DrawCard: " + card.Character.CharacterType);

        if(CurrentCard1 == null)
        {
            CurrentCard1 = card;
        }
        else if (CurrentCard2 == null)
        {
            CurrentCard2 = card;
        }
        else
        {
            throw new System.Exception(PlayerName + " ==> DrawCard");
        }
    }

    public void CardPlayed(CharacterType characterType)
    {
        Debug.Log("Card played for " + PlayerName + ": " + characterType);
        if (CurrentCard2?.Character?.CharacterType == characterType)
        {
            CurrentCard2 = null;
        }
        else if (CurrentCard1?.Character?.CharacterType == characterType)
        {
            CurrentCard1 = null;

            if(CurrentCard2 != null)
            {
                CurrentCard1 = CurrentCard2;
                CurrentCard2 = null;
            }
        }
        else
        {
            throw new System.Exception(PlayerName = " ==> CardPlayed");
        }
    }    
}
