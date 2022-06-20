using System.Linq;
using TMPro;
using UnityEngine;

public class ExcludedPileScript : MonoBehaviour
{
    public SpriteRenderer Card1Sprite;
    public TMP_Text Text;



    private void Awake()
    {
        this.ComponentInject();
    }


    private int previousDeckCount;

    void Update()
    {
        if (DeckManager.instance.Deck != null)
        {
            var deckExclusions = DeckManager.instance.Deck.Cards.Where(x => x.Status == CardStatus.Excluded);
            var exclusionCount = deckExclusions.Count();

            if (GameManager.instance.GameEnded)
            {
                Card1Sprite.sprite = deckExclusions.First().Character.Sprite;
            }

            if (exclusionCount != previousDeckCount)
            {
                Text.text = "Excluded pile (" + exclusionCount + ")";

                Card1Sprite.gameObject.SetActive(exclusionCount >= 1);

                previousDeckCount = exclusionCount;
            }
        }
        else
        {
            Text.text = "";

            Card1Sprite.gameObject.SetActive(false);
        }
    }
}
