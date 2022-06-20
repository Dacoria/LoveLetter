using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;

public class PlayerScript : MonoBehaviour, IPunInstantiateMagicCallback
{
    [ComponentInject] 
    private TMP_Text PlayerText;

    public string PlayerName;
    public PlayerStatus PlayerStatus;

    public Card CurrentCard1() => DeckManager.instance.Deck.Cards.FirstOrDefault(x => x.Player == this && x.IndexOfCardInHand == 1);
    public Card CurrentCard2() => DeckManager.instance.Deck.Cards.FirstOrDefault(x => x.Player == this && x.IndexOfCardInHand == 2);
    public Card CurrentCard3() => DeckManager.instance.Deck.Cards.FirstOrDefault(x => x.Player == this && x.IndexOfCardInHand == 3);
    public Card CurrentCard4() => DeckManager.instance.Deck.Cards.FirstOrDefault(x => x.Player == this && x.IndexOfCardInHand == 4);

    private void Awake()
    {
        this.ComponentInject();
        PlayerStatus = PlayerStatus.Normal;
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
}

public enum PlayerStatus
{
    Normal,
    Protected,
    Intercepted
}