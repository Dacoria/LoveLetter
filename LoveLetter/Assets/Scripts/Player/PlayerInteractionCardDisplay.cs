using Photon.Pun;
using System.Linq;
using System;
using UnityEngine;

public class PlayerInteractionCardDisplay : MonoBehaviour
{
    [HideInInspector][ComponentInject] public LerpMovement LerpMovement;

    [HideInInspector] public Card Card;
    [ComponentInject] private PlayerScript player;
    [ComponentInject] private PhotonView photonView;
    [ComponentInject] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        this.ComponentInject();
    }    

    void OnMouseDown()
    {
        if (!MonoHelper.Instance.GuiAllowed(checkDialogPopup: false))
        {
            return;
        }

        if (spriteRenderer.sprite == MonoHelper.Instance.BackgroundCardSprite)
        {
            return;
        }
        if (GameManager.instance.CurrentPlayer().PlayerId != player.PlayerId)
        {
            Debug.Log("Not your turn");
            BigCardHandler.instance.ShowBigCardNoButtons(Card.Character.Type);
            return;
        }
        if (!photonView.IsMine)
        {
            Debug.Log("Not your card");
            BigCardHandler.instance.ShowBigCardNoButtons(Card.Character.Type);
            return;
        }
        if (!player.HasPickedCardFromDeckIfPossible())
        {
            BigCardHandler.instance.ShowBigCardNoButtons(Card.Character.Type);
            return;
        }
        else
        {
            BigCardHandler.instance.ShowBigCardWithButtons(Card.Character.Type, Card.Id, player.PlayerId);
        }
    }
}
