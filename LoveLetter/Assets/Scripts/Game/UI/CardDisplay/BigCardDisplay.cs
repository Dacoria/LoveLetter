using UnityEngine;
using UnityEngine.UI;

public class BigCardDisplay : MonoBehaviour
{
    public bool BigCardIsActive;

    private int cardId;
    private int playerId;

    public Button PlayButton;
    public Image CharacterImage;

    private void Awake()
    {
        MonoHelper.Instance.Fade(false, 0, gameObject);
        BigCardIsActive = false;
    }

    public void ShowBigCard(CharacterType characterType, int cardId, int playerId, bool ignoreModalActive = false)
    {
        if(!ignoreModalActive && MonoHelper.Instance.GetModal().IsActive)
        { 
            BigCardIsActive = false;
            gameObject.SetActive(false);
            return;
        }

        this.cardId = cardId;
        this.playerId = playerId;
        MonoHelper.Instance.Fade(false, 0.5f, gameObject);
        BigCardIsActive = true;

        PlayButton.gameObject.SetActive(cardId >= 0);

        CharacterImage.sprite = MonoHelper.Instance.GetCharacterSprite(characterType);
    }

    public void HideBigCard()
    {
        MonoHelper.Instance.Fade(true, 0.5f, gameObject, HidingFinished);
        BigCardIsActive = false;
    }

    public void PlayCharacterCard()
    {
        MonoHelper.Instance.Fade(true, 0.5f, gameObject, HidingFinishedStartPlaying);
        BigCardIsActive = false;
    }

    private void HidingFinishedStartPlaying()
    {
        HidingFinished();
        GameManager.instance.PlayCard(cardId, playerId);
    }

    private void HidingFinished()
    {
        gameObject.SetActive(false);
    }
}