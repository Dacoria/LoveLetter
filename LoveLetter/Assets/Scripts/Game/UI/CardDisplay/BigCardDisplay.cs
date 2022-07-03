using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BigCardDisplay : MonoBehaviour
{
    public bool bigCardIsActive;
    private SpriteRenderer spriteRenderer;
    private Canvas canvas;

    public void Awake()
    {
        ResizeSpriteToScreen();
        spriteRenderer = GetComponent<SpriteRenderer>();
        canvas = FindObjectOfType<Canvas>();
    }
    private void ResizeSpriteToScreen()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        var width = sr.sprite.bounds.size.x;
        var height = sr.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        var lowestMultiplier = Mathf.Min((float)(worldScreenWidth / width), (float)(worldScreenHeight / height));

        transform.localScale = new Vector3(lowestMultiplier, lowestMultiplier, 0.1f);
    }

    public void OnMouseDown()
    {
        if(!MonoHelper.Instance.GuiAllowed(checkOptionsModal: false))
        {
            return;
        }

        if (bigCardIsActive && spriteRenderer.color.a > 0.95f)
        {
            HideBigCard();
            canvas.gameObject.SetActive(true);
        }        
    }

    public void ShowBigCard(CharacterType characterType)
    {
        if(MonoHelper.Instance.GetModal().IsActive)
        { 
            bigCardIsActive = false;
            gameObject.SetActive(false);
            return;
        }

        spriteRenderer.sprite = MonoHelper.Instance.GetCharacterSprite(characterType);

        StartCoroutine(MonoHelper.Instance.FadeSprites(false, 0.5f, GetComponentsInChildren<SpriteRenderer>().ToList()));
        bigCardIsActive = true;
        canvas.gameObject.SetActive(false);
    }

    public void HideBigCard()
    {
        StartCoroutine(MonoHelper.Instance.FadeSprites(true, 0.5f, GetComponentsInChildren<SpriteRenderer>().ToList(), HidingFinished));
        bigCardIsActive = false;
    }

    private void HidingFinished()
    {
        gameObject.SetActive(false);
    }
}