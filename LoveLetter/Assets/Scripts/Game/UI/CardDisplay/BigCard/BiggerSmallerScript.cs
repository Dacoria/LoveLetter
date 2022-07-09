using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerSmallerScript : MonoBehaviour
{
    private Vector3 localScaleStart;
    private Vector3 localScaleEnd;
    private float scaleMultiplier = 1.4f;
    private bool isIncreasing;

    private float timeInSecondsBeforeSwitching = 1.5f;
    private float elapsedTime;

    void Start()
    {
        localScaleStart = transform.localScale;
        localScaleEnd = transform.localScale * scaleMultiplier;
        isIncreasing = true;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float percComplete = elapsedTime / timeInSecondsBeforeSwitching;

        if(percComplete >= 1)
        {
            percComplete = 0;
            elapsedTime = 0;
            isIncreasing = !isIncreasing;
        }

        if(isIncreasing)
        {
            transform.localScale = Vector2.Lerp(localScaleStart, localScaleEnd, percComplete);
        }
        else
        {
            transform.localScale = Vector2.Lerp(localScaleEnd, localScaleStart, percComplete);
        }        
    }
}
