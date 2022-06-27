using Photon.Pun;
using UnityEngine;

public class DeckArrowScript : MonoBehaviour
{
    [ComponentInject] private BiggerSmallerScript biggerSmallerScript;
    [ComponentInject] private SpriteRenderer spriteRenderer;    

    private void Awake()
    {
        this.ComponentInject();
        biggerSmallerScript.enabled = false;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
    }

    void Start()
    {
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
        ActionEvents.DeckCardDrawn += OnDeckCardDrawn;
    }

    private void OnDeckCardDrawn(int pId)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == pId || PhotonNetwork.OfflineMode)
        {
            biggerSmallerScript.enabled = false;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        }
    }

    private void OnNewPlayerTurn(int pId)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == pId || PhotonNetwork.OfflineMode)
        {
            biggerSmallerScript.enabled = true;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        }
    }

    private void OnDestroy()
    {
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
        ActionEvents.DeckCardDrawn -= OnDeckCardDrawn;
    }
}
