using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckPickCardDisplayTriggerScript : MonoBehaviour
{    
    public SpriteRenderer SpriteRendererArrow;
    public List<GameObject> DeckCards;

    public Material Outline;
    public Material NoOutline;

    private void Awake()
    {
        this.ComponentInject();
        SpriteRendererArrow.color = new Color(SpriteRendererArrow.color.r, SpriteRendererArrow.color.g, SpriteRendererArrow.color.b, 0);
        
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
            StopDrawCardDisplayTrigger();
            
        }
    }

    private void OnNewPlayerTurn(int pId)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == pId || PhotonNetwork.OfflineMode)
        {
            ShowDrawCardDisplayTrigger();
            MonoHelper.Instance.ShowOkDiaglogMessage("Your Turn", "Please draw a card.");
        }
    }

    private void OnDestroy()
    {
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
        ActionEvents.DeckCardDrawn -= OnDeckCardDrawn;
    }

    private void ShowDrawCardDisplayTrigger()
    {
        SetArrow(true);
        SetOutlineCards(true);

    }    

    private void StopDrawCardDisplayTrigger()
    {
        SetArrow(false);
        SetOutlineCards(false);
    }

    private void SetArrow(bool enabled)
    {
        SpriteRendererArrow.color = new Color(SpriteRendererArrow.color.r, SpriteRendererArrow.color.g, SpriteRendererArrow.color.b, enabled ? 1 : 0);
    }

    private void SetOutlineCards(bool enabled)
    {
        foreach(var card in DeckCards)
        {
            SetOutlineCard(card, enabled);
        }
    }

    private Vector3 localScaleStart = new Vector3(0.7f, 0.7f, 1);

    private void SetOutlineCard(GameObject card, bool enabled)
    {
        if (enabled)
        {
            card.transform.localScale = card.transform.localScale * 1.07f;
            card.GetComponent<Renderer>().material = Outline;
        }
        else
        {
            card.transform.localScale = localScaleStart;
            card.GetComponent<Renderer>().material = NoOutline;
        }
    }
}
