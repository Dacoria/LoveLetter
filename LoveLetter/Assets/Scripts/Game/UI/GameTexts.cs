using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTexts : MonoBehaviour
{
    public TMP_Text GameText;
    public TMP_Text ActionText;

    //public void SetTextFade(string text)
    //{
    //    
    //    //StartCoroutine(WaitThenStartFade());
    //}
    //
    //public IEnumerator WaitThenStartFade()
    //{
    //    yield return new WaitForSeconds(2f);
    //    StartCoroutine(StartFade());
    //}
    //
    //public IEnumerator StartFade()
    //{
    //    while(ActionText.alpha > 0)
    //    {
    //        ActionText.alpha -= 0.1f;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //
    //    ActionText.text = "";
    //}
}

