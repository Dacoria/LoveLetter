using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonoHelper : MonoBehaviour
{
    private Camera mainCam;
    public ModalScript ModalScript;
    private BigCardDisplay BigCardDisplay;

    public static MonoHelper Instance;

    public Sprite BackgroundCardSprite;
    public Chibi.Free.Dialog DialogMessageGo;

    public bool GuiAllowed()
    {
        return !DialogMessageGo.isActiveAndEnabled;
    }

    public ModalScript GetModal()
    {
        return ModalScript;
    }

    public List<CharacterType> GetCharacterTypes()
    {
        var result = new List<CharacterType>();

        foreach(CharacterType charType in Enum.GetValues(typeof(CharacterType)))
        {
            result.Add(charType);
        }

        return result;
    }

    public List<CharacterSprite> CharacterSprites;


    void Awake()
    {
        Instance = this;
        mainCam = Camera.main;
        BigCardDisplay = GetComponentInChildren<BigCardDisplay>(true);
    }

    public Vector2 GetTopRightOfMainCam()
    {
        Vector2 topRight = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth, mainCam.pixelHeight, mainCam.nearClipPlane));
        topRight = new Vector2(14f, 25f);
        return topRight;
    }

    public GameObject GetPlayerGo(GameObject goPartOfPlayer)
    {
        if (goPartOfPlayer.tag == Statics.TAG_PLAYER)
        {
            return goPartOfPlayer;
        }

        if (goPartOfPlayer?.transform?.parent?.gameObject != null)
        {
            var goOfParent = goPartOfPlayer.transform.parent.gameObject;
            if (goOfParent.tag == Statics.TAG_PLAYER)
            {
                return goOfParent;
            }

            if (goOfParent?.transform?.parent?.gameObject != null)
            {
                var goOfGrandParent = goOfParent.transform.parent.gameObject;
                if (goOfGrandParent.tag == Statics.TAG_PLAYER)
                {
                    return goOfGrandParent;
                }
            }
        }

        return null;

    }

    public void ShowBigCard(CharacterType type)
    {
        BigCardDisplay.gameObject.SetActive(true);
        BigCardDisplay.ShowBigCard(type);
    }

    public bool BigCardIsActive()
    {
        return BigCardDisplay.gameObject.activeSelf && BigCardDisplay.bigCardIsActive;
    }

    public IEnumerator FadeSprites(bool fadeAway, float timeInSeconds, List<SpriteRenderer> SpriteRenderers, Action Callback = null)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= (Time.deltaTime / timeInSeconds))
            {
                foreach (SpriteRenderer spriteRenderer in SpriteRenderers)
                {
                    spriteRenderer.color = new Color(1, 1, 1, i);
                }

                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += (Time.deltaTime / timeInSeconds))
            {
                foreach (SpriteRenderer spriteRenderer in SpriteRenderers)
                {
                    spriteRenderer.color = new Color(1, 1, 1, i);
                }

                yield return null;
            }
        }

        if(Callback != null)
        {
            Callback();
        }
    }

    public Sprite GetCharacterSprite(CharacterType characterType) => CharacterSprites.Single(x => x.CharacterType == characterType).Sprite;

    public bool IsPartOfPlayerGo(GameObject go) => GetPlayerGo(go) != null;
}

[Serializable]
public class CharacterSprite
{
    public CharacterType CharacterType;
    public Sprite Sprite;
}