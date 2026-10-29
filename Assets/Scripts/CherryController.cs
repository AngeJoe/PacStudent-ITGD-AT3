using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    public GameObject cherry;

    public GameObject curCherry = null;
    float lastDestroyed = 0;

    void Start()
    {
        lastDestroyed = Time.time;
    }

    void Update()
    {
        if (Time.time - lastDestroyed > 5 && curCherry == null)
        {
            curCherry = Instantiate(cherry);
            curCherry.SetActive(false);

            Cherry cherryScript = curCherry.GetComponent<Cherry>();
            cherryScript.levelGenerator = levelGenerator;
            cherryScript.cherryController = this;

            curCherry.SetActive(true);
        }
    }

    public void destroyCherry()
    {
        Destroy(curCherry);
        curCherry = null;
        lastDestroyed = Time.time;
    }
}