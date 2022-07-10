using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour, IPunInstantiateMagicCallback
{
    public int PlayerId; // in offline mode een oplopend getal (voor dummies). voor normale games het actorNr vd player (natuurlijk... )
    public int CounterId;
    public bool IsAi;
    public int Score;

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

                if (_playerStatus == PlayerStatus.Intercepted && !PhotonNetwork.OfflineMode && PlayerId == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    MonoHelper.Instance.ShowOkDiaglogMessage("Rose Intercepted", "You are out of the round!");
                }
            }
        }
    }

    public Card CurrentCard1() => Deck.instance.Cards.FirstOrDefault(x => x.PlayerId == PlayerId && x.IndexOfCardInHand == 1);
    public Card CurrentCard2() => Deck.instance.Cards.FirstOrDefault(x => x.PlayerId == PlayerId && x.IndexOfCardInHand == 2);

    private void Awake()
    {
        this.ComponentInject();
        PlayerStatus = PlayerStatus.Normal;
        Debug.Log("Awake: " + CounterId);
    }

    private void Start()
    {
        ActionEvents.PlayerStatusChange += OnPlayerStatusChange;
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
        ActionEvents.DeckCardDrawn += OnDeckCardDrawn;
        ActionEvents.RoundEnded += OnRoundEnded;
    }

    private void OnRoundEnded(RoundEnded roundEnded)
    {
        var thisPlayerScore = roundEnded.PlayerScores.FirstOrDefault(x => x.PlayerId == PlayerId);
        if(thisPlayerScore != null)
        {
            Score = thisPlayerScore.PlayerScorePoints;
        }
    }

    private void OnPlayerStatusChange(int playerId, PlayerStatus newPlayerStatus)
    {
        if(playerId == PlayerId)
        {
            // dit ping pongt wel terug met events -> maar op een gegeven moment verandert de status niet meer (en stopt de netwerk update loop)
            PlayerStatus = newPlayerStatus;
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        var name = instantiationData[0].ToString();
        CounterId = NetworkHelper.Instance.GetPlayers().Count(); // aantal spawned Playerobject (inclusief jijzelf)
        IsAi = bool.Parse(instantiationData[1].ToString());   

        Debug.Log("OnPhotonInstantiate: " + CounterId);

        if (PhotonNetwork.OfflineMode || IsAi)
        {
            PlayerId = CounterId;
        }
        else
        {
            PlayerId = info.photonView.OwnerActorNr;
        }

        transform.position = GetPos(CounterId);

        PlayerText.text = name;        
        PlayerName = name;


        if(StaticHelper.IsWideScreen)
        {
            transform.localScale *= 1.8f;
            PlayerText.transform.localScale *= 1.2f;
            PlayerText.transform.localPosition += new Vector3(0, 0.3f, 0);
        }

        if(IsAi && PhotonNetwork.IsMasterClient)
        {
            gameObject.AddComponent<AiPlayerScript>();
        }
    }
    

    private bool hasDrawnCard;

    public bool HasPickedCardFromDeckIfPossible()
    {
        if (hasDrawnCard)
        {
            return true;
        }
        return Deck.instance.Cards.Count(x => x.Status == CardStatus.InDeck) == 0;
    }

    private void OnNewPlayerTurn(int obj)
    {
        hasDrawnCard = false;
    }

    private void OnDestroy()
    {
        ActionEvents.PlayerStatusChange -= OnPlayerStatusChange;
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
        ActionEvents.DeckCardDrawn -= OnDeckCardDrawn;
        ActionEvents.RoundEnded -= OnRoundEnded;
    }

    private void OnDeckCardDrawn(int playedId)
    {
        hasDrawnCard = true;
    }

    private Vector2 GetPos(int playerIndex)
    {
        Vector2 topRight = MonoHelper.Instance.GetTopRightOfMainCam();
        if (StaticHelper.IsWideScreen)
        {

            if (playerIndex == 1)
            {
                return new Vector2(topRight.x / 0.9f * 1.5f * -1, topRight.y / 2.5f);
            }
            if (playerIndex == 2)
            {
                return new Vector2(topRight.x / 2.7f * 1.5f * -1, topRight.y / 2.5f);
            }
            if (playerIndex == 3)
            {
                return new Vector2(topRight.x / 2.7f * 1.5f * 1, topRight.y / 2.5f);
            }
            if (playerIndex == 4)
            {
                return new Vector2(topRight.x / 0.9f * 1.5f * 1, topRight.y / 2.5f);
            }
        }
        else
        {

            if (playerIndex == 1)
            {
                return new Vector2(topRight.x / 4 * 1.5f * -1, topRight.y / 2);
            }
            if (playerIndex == 2)
            {
                return new Vector2(topRight.x / 4 * 1.5f, topRight.y / 2);
            }
            if (playerIndex == 3)
            {
                return new Vector2(topRight.x / 4 * 1.5f * -1, 0);
            }
            if (playerIndex == 4)
            {
                return new Vector2(topRight.x / 4 * 1.5f, 0);
            }
        }

        throw new System.Exception();
    }
}