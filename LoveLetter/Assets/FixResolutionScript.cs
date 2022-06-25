using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class FixResolutionScript : MonoBehaviour
{
    // https://gamedev.stackexchange.com/questions/79546/how-do-you-handle-aspect-ratio-differences-with-unity-2d/89973#89973

    public int MinimumWidth = 1000;
    public int MinimumHeight = 5000;
    public float pixelsToUnits = 100;

    void Awake()
    {
        FixResolution();
    }

    void Update()
    {
        FixResolution();
    }

    private void FixResolution()
    {
        if (Screen.width > Screen.height)
        {
            var width = Mathf.RoundToInt(MinimumHeight / (float)Screen.width * Screen.height);
            width = math.max(width, MinimumWidth);

            Camera.main.orthographicSize = width / pixelsToUnits / 2;
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            var height = Mathf.RoundToInt(MinimumWidth / (float)Screen.width * Screen.height);
            height = math.max(height, MinimumHeight);

            Camera.main.orthographicSize = height / pixelsToUnits / 2;
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}