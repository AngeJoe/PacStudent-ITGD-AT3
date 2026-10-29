using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    public CherryController cherryController;
    public Vector3 midMap = new Vector3();
    public float speed = 5f;

    int xLength;
    int yLength;

    IEnumerator Start()
    {
        xLength = levelGenerator.realXLength;
        yLength = levelGenerator.realYLength;
        int y;
        int x;

        int spawnSide = (Random.Range(0, 4));

        if (spawnSide == 0) // Left
        {
            y = Random.Range(0, yLength - 1);
            transform.position = LevelGenerator.Inst.MapToWorldPos(0, y);
        }

        else if (spawnSide == 1) // Up
        {
            x = Random.Range(0, xLength - 1);
            transform.position = LevelGenerator.Inst.MapToWorldPos(x, 0);
        }

        else if (spawnSide == 2) // Right
        {
            y = Random.Range(0, yLength - 1);
            transform.position = LevelGenerator.Inst.MapToWorldPos(xLength * 2, y);
        }

        else if (spawnSide == 3) // Down
        {
            x = Random.Range(0, xLength - 1);
            transform.position = LevelGenerator.Inst.MapToWorldPos(x, yLength * 2);
        }

        Vector3 dir = (-transform.position).normalized;

        while (!offScreen())
        {
            transform.position += speed * dir * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (cherryController != null)
            cherryController.destroyCherry();
    }

    bool offScreen()
    {
        if (levelGenerator == null) return true;
        int[] pos = levelGenerator.WorldToMapPos(transform.position);
        if (pos == null || pos.Length < 2) return true;
        int x = pos[0];
        int y = pos[1];
        return x < -20 || y < -5 || x > xLength + 20 || y > yLength + 5;
    }
}