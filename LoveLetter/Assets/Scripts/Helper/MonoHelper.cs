using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MonoHelper : MonoBehaviour
{
    private Camera mainCam;
    public static MonoHelper Instance;

    public List<CharacterSprite> CharacterSprites;

    void Awake()
    {
        Instance = this;
        mainCam = Camera.main;
    }

    public Vector2 GetTopRightOfMainCam()
    {
        Vector2 topRight = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth, mainCam.pixelHeight, mainCam.nearClipPlane));
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

    public Sprite GetCharacterSprite(CharacterType characterType) => CharacterSprites.Single(x => x.CharacterType == characterType).Sprite;

    public bool IsPartOfPlayerGo(GameObject go) => GetPlayerGo(go) != null;
}

[Serializable]
public class CharacterSprite
{
    public CharacterType CharacterType;
    public Sprite Sprite;
}