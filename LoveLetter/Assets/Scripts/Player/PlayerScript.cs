using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour, IPunInstantiateMagicCallback
{
    public int PlayerId; // in offline mode een oplopend getal (voor dummies). voor normale games het actorNr vd player (natuurlijk... )

    [ComponentInject] 
    private TMP_Text PlayerText;

    public string PlayerName;
    private PlayerStatus _playerStatus;
    public PlayerStatus PlayerStatus
    {
        get => _playerStatus;
        set
        {
            if (_playerStatus != value)
            {
                var oldValue = _playerStatus;
                _playerStatus = value;

                NetworkActionEvents.instance.PlayerStatusChange(PlayerId, _playerStatus);

                //if(_playerStatus == PlayerStatus.Intercepted)
                //{
                //    var cardsOfPlayer = Deck.instance.Cards.Where(x => x.PlayerId == PlayerId).ToList();
                //    for(int i = 0; i < cardsOfPlayer.Count; i++)
                //    {
                //        cardsOfPlayer[i].Status = CardStatus.InDiscard;
                //    }    
                //}
            }
        }
    }

    public Card CurrentCard1() => Deck.instance.Cards.FirstOrDefault(x => x.PlayerId == PlayerId && x.IndexOfCardInHand == 1);
    public Card CurrentCard2() => Deck.instance.Cards.FirstOrDefault(x => x.PlayerId == PlayerId && x.IndexOfCardInHand == 2);

    private void Awake()
    {
        this.ComponentInject();
        PlayerStatus = PlayerStatus.Normal;
        //PlayerText.text = PhotonNetwork.NickName;
        // PlayerName = PhotonNetwork.NickName;
    }

    private void Start()
    {
        ActionEvents.PlayerStatusChange += OnPlayerStatusChange;
    }

    private void OnPlayerStatusChange(int playerId, PlayerStatus newPlayerStatus)
    {
        if(playerId == PlayerId)
        {
            // dit ping pongt wel terug met events -> maar op een gegeven moment verandert de status niet meer (en stopt de netwerk update loop)
            PlayerStatus = newPlayerStatus;
        }
    }

    private void OnDestroy()
    {
        ActionEvents.PlayerStatusChange -= OnPlayerStatusChange;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        var name = instantiationData[0].ToString();

        if(PhotonNetwork.OfflineMode)
        {
            PlayerId = int.Parse(instantiationData[1].ToString());
        }
        else
        {
            PlayerId = info.photonView.OwnerActorNr;
        }

        PlayerText.text = name;
        PlayerName = name;
    }
}