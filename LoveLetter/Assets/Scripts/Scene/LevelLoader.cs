using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private Animator Transition;

    private float TransitionTime = 0.5f;
    private Canvas canvas;

    public static LevelLoader instance;

    private void Awake()
    {
        instance = this;
        Transition = GetComponentInChildren<Animator>();
        canvas = FindObjectOfType<Canvas>();
        canvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        var animStateInfo = Transition.GetCurrentAnimatorStateInfo(0);
        var NTime = animStateInfo.normalizedTime;

        if (NTime > 1.0f)
        {
            canvas.gameObject.SetActive(true);
        }
    }



    private string sceneName;
    public void LoadScene(string sceneName)
    {
        this.sceneName = sceneName;
        StartCoroutine(CR_LoadAnimation(LoadScene));
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }


    public void LoadSceneAnimation(Action callback)
    {
        StartCoroutine(CR_LoadAnimation(callback));
    }

    //public void LoadNextScene(bool punScene)
    //{
    //    StartCoroutine(CR_LoadScene(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name, punScene));
    //}  

    private IEnumerator CR_LoadAnimation(Action Callback)
    {
        Transition.SetTrigger(Statics.ANIMATION_TRIGGER_START_ANIMATION_SCENE);
        yield return new WaitForSeconds(TransitionTime);
        Callback();
    }
}
