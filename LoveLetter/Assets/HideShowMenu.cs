using UnityEngine;
using UnityEngine.EventSystems;

public class HideShowMenu : MonoBehaviour, IPointerClickHandler
{
    public GameObject MenuGo;     

    private bool isGoEnabled;

    private void Start()
    {
        MenuGo.SetActive(false);
        isGoEnabled = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isGoEnabled = !isGoEnabled;
        MenuGo.SetActive(isGoEnabled);
    }

    public void HideStartMenu()
    {
        MenuGo.SetActive(false);
        isGoEnabled = false;
    }
}
