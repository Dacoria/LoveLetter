using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionText : MonoBehaviour
{
    public TMP_Text Text;

    public void SetTextFade(string text)
    {
        Text.alpha = 1;
        Text.text = text;
        //StartCoroutine(WaitThenStartFade());
    }

    public IEnumerator WaitThenStartFade()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(StartFade());
    }

    public IEnumerator StartFade()
    {
        while(Text.alpha > 0)
        {
            Text.alpha -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        Text.text = "";
    }
}

