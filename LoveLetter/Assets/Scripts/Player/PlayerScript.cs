using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour, IPunInstantiateMagicCallback
{
    [ComponentInject] 
    private TMP_Text PlayerText;

    public string PlayerName;
    public PlayerStatus _playerStatus;
    public PlayerStatus PlayerStatus
    {
        get => _playerStatus;
        set
        {
            if (_playerStatus != value)
            {
                var oldValue = _playerStatus;
                _playerStatus = value;
                ActionEvents.PlayerStatusChange?.Invoke(this, oldValue);

                if(_playerStatus == PlayerStatus.Intercepted)
                {
                    var cardsOfPlayer = Deck.instance.Cards.Where(x => x.Player == this).ToList();
                    for(int i = 0; i < cardsOfPlayer.Count; i++)
                    {
                        cardsOfPlayer[i].Status = CardStatus.InDiscard;
                    }    
                }
            }
        }
    }

    public Card CurrentCard1() => Deck.instance.Cards.FirstOrDefault(x => x.Player == this && x.IndexOfCardInHand == 1);
    public Card CurrentCard2() => Deck.instance.Cards.FirstOrDefault(x => x.Player == this && x.IndexOfCardInHand == 2);

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

    private void Start()
    {
        ActionEvents.NewGameStarted += OnNewGameStarted;
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
        ActionEvents.GameEnded += OnGameEnded;
    }

    private void OnNewGameStarted()
    {
        //throw new System.NotImplementedException();
    }

    private void OnNewPlayerTurn()
    {
        //throw new System.NotImplementedException();
    }

    private void OnGameEnded(List<PlayerScript> playersWon)
    {
        //throw new System.NotImplementedException();
    }   

    private void OnDestroy()
    {
        ActionEvents.NewGameStarted -= OnNewGameStarted;
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
        ActionEvents.GameEnded -= OnGameEnded;
    }
}