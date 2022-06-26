using System.Collections;
using System.Collections.Generic;
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
        drawPileScript.OnDeckCardClick();
    }
}
