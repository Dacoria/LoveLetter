using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenPileOffset : MonoBehaviour
{
    private Vector3 initScale = new Vector3(0.8f, 0.8f, 1);
    
    public Vector3 InitPos;
    public Vector3 WideScreenPosOffset;
    public float WideScreenScaleMultiplier;

    void Update()
    {
        if (StaticHelper.IsWideScreen)
        {
            transform.localScale = initScale * WideScreenScaleMultiplier;
            transform.localPosition = InitPos + WideScreenPosOffset;
        }
        else
        {
            transform.localScale = initScale;
            transform.localPosition = InitPos;
        }
    }
}
