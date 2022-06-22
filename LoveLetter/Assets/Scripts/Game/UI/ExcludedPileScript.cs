using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ExcludedPileScript : MonoBehaviour
{
    public SpriteRenderer Card1Sprite;
    public TMP_Text Text;

    private void Start()
    {
        UpdateCardDisplay();
        ActionEvents.NewGameStarted += UpdateCardDisplay;
        ActionEvents.NewPlayerTurn += UpdateCardDisplay;
        ActionEvents.GameEnded += UpdateCardDisplay;
    }

    private void OnDestroy()
    {
        ActionEvents.NewGameStarted -= UpdateCardDisplay;
        ActionEvents.NewPlayerTurn -= UpdateCardDisplay;
        ActionEvents.GameEnded -= UpdateCardDisplay;
    }

    void UpdateCardDisplay(object o)
    {
        UpdateCardDisplay();
    }

    void UpdateCardDisplay()
    {
        if (Deck.instance.Cards != null)
        {
            var deckExclusions = Deck.instance.Cards.Where(x => x.Status == CardStatus.Excluded);
            var exclusionCount = deckExclusions.Count();

            if (GameManager.instance.GameEnded)
            {
                Card1Sprite.sprite = MonoHelper.Instance.GetCharacterSprite(deckExclusions.First().Character.Type);
            }
            
            Text.text = "Excluded pile (" + exclusionCount + ")";
            Card1Sprite.gameObject.SetActive(exclusionCount >= 1);
        }
        else
        {
            Text.text = "";
            Card1Sprite.gameObject.SetActive(false);
        }
    }
}
