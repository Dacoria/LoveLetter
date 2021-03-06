using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonoHelper : MonoBehaviour
{
    private Camera mainCam;
    public ModalScript ModalScript;

    public static MonoHelper Instance;

    public Sprite BackgroundCardSprite;
    public DialogText DialogText;
    public GameObject MenuGo;
    public GameObject InstructionsGo;
    public GameObject ScoreGo;

    public bool GuiAllowed(bool checkDialogPopup = true, bool checkOptionsModal = true, bool checkMainMenu = true, bool checkInstructionsMenu = true, bool checkBigCard = true, bool checkScore = true)
    {
        if(checkDialogPopup && DialogText.gameObject.activeSelf)
        {
            return false;
        }
        if (checkOptionsModal && GetModal().IsActive)
        {
            return false;
        }
        if (checkMainMenu && MenuGo.activeSelf)
        {
            return false;
        }
        if (checkInstructionsMenu && InstructionsGo.activeSelf)
        {
            return false;
        }
        if (checkBigCard && BigCardHandler.instance.BigCardIsActive())
        {
            return false;
        }
        if (checkScore && ScoreGo.activeSelf)
        {
            return false;
        }

        return true;
    }

    public void DoCharacterChoice(PlayerScript playerScript, Action<string> callback, string optionsText, List<string> options, CharacterType characterType, int cardId)
    {
        if(playerScript.IsAi)
        {
            playerScript.GetComponent<AiPlayerScript>().DoCardChoice(callback, options, characterType, cardId);
        }
        else
        {
            ModalScript.SetOptions(callback, optionsText, options);
        }
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
    public List<Sprite> EmotionSprites;


    public Sprite GetEmoticonSprite(int index) => EmotionSprites[index];

    void Awake()
    {
        Instance = this;
        mainCam = Camera.main;
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

    public void Fade(bool fadeAway, float timeInSeconds, GameObject go, Action callback = null)
    {
        var fadeObjects = new FadeObjects
        {
            Images = go.GetComponentsInChildren<Image>().ToList(),
            SpriteRenderers = go.GetComponentsInChildren<SpriteRenderer>().ToList(),
            Texts = go.GetComponentsInChildren<TMP_Text>().ToList()
        };

        StartCoroutine(Fade(fadeAway, timeInSeconds, fadeObjects, callback));
    }

    public IEnumerator Fade(bool fadeAway, float timeInSeconds, FadeObjects fadeObjects, Action callback = null)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= (Time.deltaTime / timeInSeconds))
            {
                UpdateAlphaColor(fadeObjects, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            for (float i = 0; i <= 1; i += (Time.deltaTime / timeInSeconds))
            {
                UpdateAlphaColor(fadeObjects, i);
                yield return null;
            }
        }

        if(callback != null)
        {
            callback();
        }
    }

    private void UpdateAlphaColor(FadeObjects fadeObjects, float alpha)
    {
        foreach (SpriteRenderer spriteRenderer in fadeObjects.SpriteRenderers)
        {
            spriteRenderer.color = new Color(1, 1, 1, alpha);
        }
        foreach (Image image in fadeObjects.Images)
        {
            image.color = new Color(1, 1, 1, alpha);
        }
        foreach (TMP_Text text in fadeObjects.Texts)
        {
            text.color = new Color(1, 1, 1, alpha);
        }
    }

    public Sprite GetCharacterSprite(CharacterType characterType) => CharacterSprites.Single(x => x.CharacterType == characterType).Sprite;

    public bool IsPartOfPlayerGo(GameObject go) => GetPlayerGo(go) != null;

    public void ShowOkDiaglogMessage(string title, string description, bool showScore = false)
    {
        DialogText.gameObject.SetActive(true);
        DialogText.SetText(title, description, showScore);
    }

    public int GetRoseCountToWinGame(int playerCount)
    {
        if (playerCount == 2)
        {
            return 6;
        }
        if (playerCount == 3)
        {
            return 5;
        }
        if (playerCount == 4)
        {
            return 4;
        }

        return 3;
    }
}

[Serializable]
public class CharacterSprite
{
    public CharacterType CharacterType;
    public Sprite Sprite;
}

public class FadeObjects
{
    public List<Image> Images;
    public List<SpriteRenderer> SpriteRenderers;
    public List<TMP_Text> Texts;
}