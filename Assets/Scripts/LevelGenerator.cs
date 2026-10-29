using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject level01;
    public GameObject outCorner;
    public GameObject outWall;
    public GameObject inCorner;
    public GameObject inWall;
    public GameObject pellet;
    public GameObject powerPellet;
    public GameObject tJunction;
    public GameObject ghostExit;

    int[,] levelMap = 
        {
            {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
            {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
            {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
            {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
            {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
            {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
            {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
            {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,4,0,3,4,4,8},
            {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
            {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
        };
    // LEGEND:
    // 0 - Empty (Can either be a background sprite or no sprite).
    // 1 - Outside corner (double lined corner piece in original game).
    // 2 - Outside wall (double line in original game)
    // 3 - Inside corner (single lined corner piece in original game).
    // 4 - Inside wall (single line in original game).
    // 5 - An empty space with a Standard pellet (see Visual Assets above).
    // 6 - An empty space with a Power pellet (see Visual Assets above).
    // 7 - A t junction piece for connecting with adjoining regions.
    // 8 - Ghost exit wall (pink line in original game).

    int xLength;
    int yLength;
    public int realXLength => levelMap.GetLength(1) * 2;
    public int realYLength => levelMap.GetLength(0) * 2 - 1;
    
    public static LevelGenerator Inst { get; private set; }

    void Awake()
    {
        if (Inst == null) Inst = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        xLength = levelMap.GetLength(1);
        yLength = levelMap.GetLength(0);
        
        Destroy(level01);
        createLevel();
        MirrorHori();
        MirrorVert();
    }

    void Update()
    {

    }

    public Vector3 MapToWorldPos(int x, int y)
    {
        x -= xLength - 1;
        y = yLength - y - 1;
        return new Vector3(x, y);
    }

    public int[] WorldToMapPos(Vector3 vec)
    {
        int x = (int)Mathf.Round(vec.x);
        int y = (int)Mathf.Round(vec.y);

        x += xLength - 1;
        y = yLength - y - 1;

        return new int[] {x, y};
    }

    public int getTile(int x, int y)
    {
        // Debug.Log(x + ", " + y + " before");
        int[] mapPos = WorldToMapPos(new Vector3(x, y));
        x = mapPos[0];
        y = mapPos[1];
        // Debug.Log(x + ", " + y + " after");

        if (x >= xLength) x = 2 * xLength - x - 1;
        if (y >= yLength) y = 2 * yLength - y - 2;
        return levelMap[y, x];
    }

    public int getTile(float x, float y)
    {
        return getTile((int)Mathf.Round(x), (int)Mathf.Round(y));
    }

    public bool isEmpty(Vector3 endPos)
    {
        int tile = getTile(endPos.x, endPos.y);
        return tile == 0 || tile == 5 || tile == 6;
    }

    public bool isNormalPellet(Vector3 endPos)
    {
        int tile = getTile(endPos.x, endPos.y);
        return tile == 5;
    }

    public bool isPowerPellet(Vector3 endPos)
    {
        int tile = getTile(endPos.x, endPos.y);
        return tile == 6;
    }

    public void createLevel() 
    {
        int leftCell = -1;
        int rightCell = -1;
        int topCell = -1;
        int bottomCell = -1;
        int rotation = -1;

        for (int y = 0; y < yLength; ++y) 
        {
            for(int x = 0; x < xLength; ++x)
            {
                int cell = levelMap[y, x];
                Vector3 worldPos = new Vector3(x - xLength + 1, -y + yLength - 1, 0);

                if (x > 0)
                    leftCell = levelMap[y, x-1];
                if (x < xLength - 1)
                    rightCell = levelMap[y, x+1];
                if (y > 0)
                    topCell = levelMap[y-1, x];
                if (y < yLength - 1) 
                    bottomCell = levelMap[y+1, x];
                
                switch(cell)
                {
                    case 0: // Empty
                        break;
                    case 1: // Outside corner
                        if ((rightCell == 1 || rightCell == 2) && (bottomCell == 1 || bottomCell == 2)) 
                            rotation = 0;
                        if ((leftCell == 1 || leftCell == 2) && (bottomCell == 1 || bottomCell == 2))
                            rotation = 270;
                        if ((leftCell == 1 || leftCell == 2) && (topCell == 1 || topCell == 2))
                            rotation = 180;
                        if ((topCell == 1 || topCell == 2) && (rightCell == 1 || rightCell == 2)) 
                            rotation = 90;
                        GameObject spawnOC = SpawnPiece(outCorner, worldPos, rotation);
                        break;

                    case 2: // Outside wall
                        if (topCell == 1 || topCell == 2 && (bottomCell == 1 || bottomCell == 2))
                            rotation = 90;
                        // if ((rightCell == 1 || rightCell == 2 || rightCell == 7) && (leftCell == 1 || leftCell == 2 || leftCell == 7))
                        else
                            rotation = 0;
                        GameObject spawnOW = SpawnPiece(outWall, worldPos, rotation);
                        break;

                    case 3: // Inside corner
                        if (Connectible(rightCell) && Connectible(bottomCell)) 
                            rotation = 0;
                        if (Connectible(leftCell) && Connectible(bottomCell))
                            rotation = 270;
                        if (Connectible(leftCell) && Connectible(topCell))
                            rotation = 180;
                        if (Connectible(rightCell) && Connectible(topCell)) 
                            rotation = 90;
                        GameObject spawnIC = SpawnPiece(inCorner, worldPos, rotation);
                        break;

                    case 4: // Inside wall
                       if ((rightCell == 3 || rightCell == 4 || rightCell == 8) && (leftCell == 3 || leftCell == 4 || leftCell == 8))
                            rotation = 0;
                        else
                            rotation = 90;
                        GameObject spawnIW = SpawnPiece(inWall, worldPos, rotation);
                        break;

                    case 5: // Standard pellet
                        rotation = 0;
                        GameObject spawnSP = SpawnPiece(pellet, worldPos, rotation);
                        break;

                    case 6: // Power pellet
                        rotation = 0;
                        GameObject spawnPP = SpawnPiece(powerPellet, worldPos, rotation);
                        break;

                    case 7: // T Junction
                        rotation = 0;
                        GameObject spawnTJ = SpawnPiece(tJunction, worldPos, rotation);
                        break;

                    case 8: // Ghost exit wall
                        rotation = 0;
                        GameObject spawnGE = SpawnPiece(ghostExit, worldPos, rotation);
                        break;
                }
            }
        }
    }

    public bool Connectible(int cell) 
    {
        return (cell >= 1 && cell <= 4) || cell == 7 || cell == 8;
    }

    public GameObject SpawnPiece(GameObject prefab, Vector3 position, float rotation)
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = position;
        obj.transform.rotation = Quaternion.Euler(0, 0, rotation);
        return obj;
    }

    public void MirrorHori()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child == transform)
                continue;

            float rotation = child.rotation.eulerAngles.z;
            if (rotation == 90)
                rotation = 180;
            else if (rotation == 180)
                rotation = 90;
            else if (rotation == 270)
                rotation = 0;
            else if (rotation == 0)
                rotation = 270;

            Transform obj = SpawnPiece(child.gameObject, new Vector3(-child.localPosition.x + 1, child.localPosition.y, child.localPosition.z), child.rotation.eulerAngles.z).transform;

            if (obj.name.Contains("Corner"))
            {
                obj.rotation = Quaternion.Euler(0, 0, rotation);
            }
            else if (obj.name.Contains("Junction"))
            {
                obj.rotation = Quaternion.Euler(0, 0, -child.rotation.eulerAngles.z);
                obj.localScale = new Vector3(-obj.localScale.x, obj.localScale.y, obj.localScale.z);
            }
        }
    }

    void MirrorVert()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            Vector3 pos = new Vector3(child.localPosition.x, -child.localPosition.y, child.localPosition.z);
            if (child == transform || pos == child.localPosition)
                continue;

            float rotation = child.rotation.eulerAngles.z;
            if (rotation == 0)
                rotation = 90;
            else if (rotation == 90)
                rotation = 0;
            else if (rotation == 180)
                rotation = 270;
            else if (rotation == 270)
                rotation = 180;

            Transform obj = SpawnPiece(child.gameObject, pos, child.rotation.eulerAngles.z).transform;

            if (obj.name.Contains("Corner"))
            {
                obj.rotation = Quaternion.Euler(0, 0, rotation);
            }
            else if (child.name.Contains("Junction"))
            {
                obj.rotation = Quaternion.Euler(0, 0, -child.rotation.eulerAngles.z);
                obj.localScale = new Vector3(obj.localScale.x, -obj.localScale.y, obj.localScale.z);
            }
        }
    }
}
