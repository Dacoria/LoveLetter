using UnityEngine;

public class DrawCardDisplayScript : MonoBehaviour
{
    [ComponentInject] private DrawPileScript drawPileScript;
    private void Awake()
    {
        this.ComponentInject();
    }

    void OnMouseDown()
    {
        if (MonoHelper.Instance.GuiAllowed())
        {
            drawPileScript.OnDeckCardClick();
        }
    }
}