using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private Animator Transition;

    private float TransitionTime = 0.5f;

    public static LevelLoader instance;

    private void Awake()
    {
        instance = this;
        Transition = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //if(Input.GetKeyDown("p"))
        //{
        //    LoadNextScene();
        //}
    }

    public void LoadScene(string scene, bool punScene)
    {
        StartCoroutine(CR_LoadScene(scene, punScene));
    }

    public void LoadNextScene(bool punScene)
    {
        StartCoroutine(CR_LoadScene(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name, punScene));
    }

    private IEnumerator CR_LoadScene(string sceneName, bool punScene)
    {
        Transition.SetTrigger(Statics.ANIMATION_TRIGGER_START_ANIMATION_SCENE);
        yield return new WaitForSeconds(TransitionTime);

        if(punScene)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
