using UnityEngine;

public class AiImageVisibleScript : MonoBehaviour
{
    [ComponentInject] private PlayerScript playerScript;

    private void Awake()
    {
        this.ComponentInject();
    }

    void Start()
    {
        if(!playerScript.IsAi)
        {
            Destroy(gameObject);
        }
    }
}
