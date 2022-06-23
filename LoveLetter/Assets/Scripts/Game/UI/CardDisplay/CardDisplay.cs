using Photon.Pun;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{    
    public Card Card;
    [ComponentInject] private PlayerScript player;
    [ComponentInject] private PhotonView photonView;

    private void Awake()
    {
        this.ComponentInject();
    }

    void OnMouseDown()
    {
        if(GameManager.instance.CurrentPlayer().PlayerId != player.PlayerId)
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

    public void PlayCard()
    {
        GameManager.instance.PlayCard(Card.Id, player.PlayerId);
    }
}
