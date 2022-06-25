using Photon.Pun;
using System;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{   
    [HideInInspector] public Card Card;
    [ComponentInject] private PlayerScript player;
    [ComponentInject] private PhotonView photonView;

    private void Awake()
    {
        this.ComponentInject();
    }

    void OnMouseDown()
    {
        MouseDownTime = DateTime.Now;
        isMouseClick = true;
    }


    private bool isMouseClick;
    private DateTime? MouseDownTime;

    private void Update()
    {
        if(MouseDownTime.HasValue)
        {
            var msPast = (DateTime.Now - MouseDownTime.Value).TotalMilliseconds;
            if(msPast > 500)
            {
                isMouseClick = false;
                MouseDownTime = null;
                MouseHoldEvent();
            }
        }
    }

    private void OnMouseUp()
    {
        if(isMouseClick)
        {
            if (GameManager.instance.CurrentPlayer().PlayerId != player.PlayerId)
            {
                Debug.Log("Not your turn");
                return;
            }
            if (!photonView.IsMine)
            {
                Debug.Log("Not your card");
                return;
            }
            PlayCard();
        }

        MouseDownTime = null;
    }

    private void MouseHoldEvent()
    {
        MonoHelper.Instance.ShowBigCard(Card.Character.Type);
    }

    public void PlayCard()
    {
        GameManager.instance.PlayCard(Card.Id, player.PlayerId);
    }
}
