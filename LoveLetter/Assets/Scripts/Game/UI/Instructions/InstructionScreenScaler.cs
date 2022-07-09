using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InstructionScreenScaler : MonoBehaviour
{
    private Vector3 InitLocalScale = new Vector3(0.5f, 0.5f, 1);
    public RectTransform rect;

    private void Start()
    {
        rect = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        var diffWidthHeight = (float)Screen.width / Screen.height;
        if (diffWidthHeight >= 1.32)
        {
            ScreenTooWideSoScaleDown(diffWidthHeight);
        }
        else if (diffWidthHeight < 0.65f)
        {
            ScreenTooBigSoScaleDown(diffWidthHeight);
        }
        else
        {
            transform.localScale = InitLocalScale;
        }
    }    

    private void ScreenTooWideSoScaleDown(float scaleNow)
    {
        // https://docs.google.com/spreadsheets/d/1e6uaZtFYQGyo96YvATnNIVJ9ggrm4d_b6n47n2g2BBE/edit#gid=0
        var scale1 = 1.32f;
        var idealMulti1 = 1;

        var scale2 = 2.6666f;
        var idealMulti2 = 2;

        var scaleExtraNow = scaleNow - scale1;

        var extraMultiNow = scaleExtraNow / scale1;
        var idealMultiNow = 1 + extraMultiNow;

        transform.localScale = InitLocalScale / idealMultiNow;
    }

    private void ScreenTooBigSoScaleDown(float scaleNow)
    {
        var diff = 0.65f / scaleNow;
        transform.localScale = InitLocalScale / diff;
    }
}
