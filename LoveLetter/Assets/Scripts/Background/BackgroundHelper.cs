using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundHelper : MonoBehaviour
{
    public float speed = 0;
    float pos = 0;
    [ComponentInject] private RawImage image;
    [ComponentInject] private BackgroundSpeed backgroundSpeed;

    void Awake()
    {
        this.ComponentInject();
    }

    void Update()
    {
        pos += (speed * backgroundSpeed.Speed);

        if (pos > 1.0F)

            pos -= 1.0F;

        image.uvRect = new Rect(pos, 0, 1, 1);
    }
}
