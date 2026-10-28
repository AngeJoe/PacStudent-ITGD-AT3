using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    public Vector3 midMap = new Vector3();
    public float speed = 5f;
    public float waitTime = 5;

    IEnumerator Start()
    {
        int xLength;
        int yLength;

        int x;
        int y;

        int spawnSide = (Random.Range(0, 4));
        // if (spawnSide == 0) // Left
        // else if (spawnSide == 1) // Up
        // else if (spawnSide == 2) // Right
        // else if (spawnSide == 3) // Down
        yield return new WaitForSeconds(waitTime);
    }

    void Update()
    {
        
    }

    void destroy()
    {

    }
}
