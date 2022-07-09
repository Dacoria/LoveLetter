using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgScriptMoveToRight : MonoBehaviour
{
    public Image Image;
    public GameObject BgPrefab;

    private float speed = 1.5f;
    public bool FirstGo = false;
    private bool spawnedNewGo = false;

    private void Update()
    {
        transform.position += new Vector3(1,0,0) * speed * Time.deltaTime;

        if(!FirstGo && transform.position.x > -75 && !spawnedNewGo)
        {
            SpawnNewBg();
        }
        if (transform.position.x > 100)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (FirstGo)
        {
            SpawnNewBg();
        }

    }

    private void SpawnNewBg()
    {
        if(spawnedNewGo)
        {
            return;
        }

        var newBg = Instantiate(BgPrefab, transform.parent.transform);
        newBg.transform.position = transform.position - new Vector3(transform.GetComponent<RectTransform>().sizeDelta.x, 0, 0);
        newBg.GetComponent<BgScriptMoveToRight>().FirstGo = false;
        spawnedNewGo = true;
    }
}