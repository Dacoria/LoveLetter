using System.Collections.Generic;
using UnityEngine;

public class DestroyDummyPlayerScript : MonoBehaviour
{
    void Start()
    {
        ActionEvents.NewRoundStarted += OnNewRoundStarted;
    }

    private void OnNewRoundStarted(List<int> p, int q)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ActionEvents.NewRoundStarted -= OnNewRoundStarted;
    }
}