using System.Collections.Generic;
using UnityEngine;

public abstract class UpdateCardDisplayMonoBehaviourAbstract : MonoBehaviour
{
    public List<int> CardIdsBeingMoved;

    public abstract void UpdateCardDisplay();
    public abstract Transform GetLocationVisibleCardOnTop();
}