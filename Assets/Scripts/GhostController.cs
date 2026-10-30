using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public int ghostNumber = 1;
    public string state = "";
    public LevelGenerator levelGenerator;
    public GameManager gameManager;
    public Transform player;
    public Animator animator;

    Vector3 startPosition;
    Vector3 endPosition;
    string currentDirection = "";
    bool moving = false;
    bool inSpawn = true;
    int cornerNumber = 0;

    void Start()
    {
        startPosition = transform.position;
        endPosition = transform.position;
        UpdateAnimation();
    }

    public void SetStartPosition(Vector3 pos)
    {
        startPosition = pos;
        transform.position = pos;
        ResetGhost();
    }

    void Update()
    {
        if (!gameManager.isPlaying)
        {
            animator.speed = 0;
            return;
        }

        animator.speed = 1;

        float speed = (1f / gameManager.player.playerSpeed) * 0.9f;
        if (state == "Scared" || state == "Recovering")
            speed *= 0.5f;

        if (state == "Dead")
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                ResetGhost();
                ChangeState(gameManager.GetGhostState());
                gameManager.UpdateMusic();
            }
            return;
        }

        if (!moving)
        {
            string direction = ChooseDirection();
            if (direction == "")
                return;

            currentDirection = direction;
            endPosition = transform.position + DirectionVector(direction);
            moving = true;
            UpdateAnimation();
        }

        transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPosition) < 0.01f)
        {
            transform.position = endPosition;
            moving = false;
        }
    }

    Vector3 DirectionVector(string direction)
    {
        if (direction == "W") return Vector3.up;
        if (direction == "A") return Vector3.left;
        if (direction == "S") return Vector3.down;
        if (direction == "D") return Vector3.right;
        return Vector3.zero;
    }

    string Backwards(string direction)
    {
        if (direction == "W") return "S";
        if (direction == "S") return "W";
        if (direction == "A") return "D";
        if (direction == "D") return "A";
        return "";
    }

    string ChooseDirection()
    {
        if (inSpawn)
        {
            int exitX = (ghostNumber == 1 || ghostNumber == 2) ? 0 : 1;
            bool goUp = ghostNumber == 1 || ghostNumber == 3;
            int x = Mathf.RoundToInt(transform.position.x);
            int y = Mathf.RoundToInt(transform.position.y);

            if (x < exitX) return "D";
            if (x > exitX) return "A";
            if (goUp && y < 3) return "W";
            if (!goUp && y > -3) return "S";
            inSpawn = false;
        }

        List<string> possible = new List<string>();
        string[] directions = { "W", "A", "S", "D" };

        foreach (string direction in directions)
        {
            Vector3 next = transform.position + DirectionVector(direction);
            if (levelGenerator.isGhostWalkable(next) && !levelGenerator.isTeleport(next))
                possible.Add(direction);
        }

        if (possible.Count == 0)
            return "";

        if (possible.Count > 1)
            possible.Remove(Backwards(currentDirection));

        if (possible.Count == 0)
            return Backwards(currentDirection);

        if (state == "Scared" || state == "Recovering" || ghostNumber == 1)
            return ChooseByDistance(possible, true);
        if (ghostNumber == 2)
            return ChooseByDistance(possible, false);
        if (ghostNumber == 3)
            return possible[Random.Range(0, possible.Count)];

        return ChooseClockwise(possible);
    }

    string ChooseByDistance(List<string> possible, bool away)
    {
        float currentDistance = (transform.position - player.position).sqrMagnitude;
        List<string> best = new List<string>();

        foreach (string direction in possible)
        {
            Vector3 next = transform.position + DirectionVector(direction);
            float distance = (next - player.position).sqrMagnitude;

            if ((away && distance >= currentDistance - 0.001f) ||
                (!away && distance <= currentDistance + 0.001f))
                best.Add(direction);
        }

        if (best.Count == 0)
            best = possible;

        return best[Random.Range(0, best.Count)];
    }

    string ChooseClockwise(List<string> possible)
    {
        Vector3[] corners = {
            new Vector3(13, 13, 0),
            new Vector3(13, -13, 0),
            new Vector3(-12, -13, 0),
            new Vector3(-12, 13, 0)
        };

        if (Vector3.Distance(transform.position, corners[cornerNumber]) < 0.1f)
            cornerNumber = (cornerNumber + 1) % corners.Length;

        string best = possible[0];
        int bestDistance = 100000;

        foreach (string direction in possible)
        {
            Vector3 next = transform.position + DirectionVector(direction);
            int distance = PathDistance(next, corners[cornerNumber]);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = direction;
            }
        }
        return best;
    }

    int PathDistance(Vector3 from, Vector3 target)
    {
        int width = levelGenerator.realXLength;
        int height = levelGenerator.realYLength;
        bool[,] visited = new bool[width, height];
        Queue<Vector3> places = new Queue<Vector3>();
        Queue<int> distances = new Queue<int>();
        places.Enqueue(from);
        distances.Enqueue(0);
        string[] directions = { "W", "A", "S", "D" };

        while (places.Count > 0)
        {
            Vector3 pos = places.Dequeue();
            int distance = distances.Dequeue();
            int[] cell = levelGenerator.WorldToMapPos(pos);

            if (cell[0] < 0 || cell[0] >= width || cell[1] < 0 || cell[1] >= height)
                continue;

            if (visited[cell[0], cell[1]])
                continue;

            visited[cell[0], cell[1]] = true;

            if (Vector3.Distance(pos, target) < 0.1f)
                return distance;

            foreach (string dir in directions)
            {
                Vector3 next = pos + DirectionVector(dir);
                if (levelGenerator.isGhostWalkable(next) && !levelGenerator.isTeleport(next))
                {
                    places.Enqueue(next);
                    distances.Enqueue(distance + 1);
                }
            }
        }
        return 100000;
    }

    public void ChangeState(string newState)
    {
        if (state == newState)
            return;

        state = newState;
        if (state == "Dead")
            moving = false;

        UpdateAnimation();
    }

    public void ResetGhost()
    {
        transform.position = startPosition;
        endPosition = startPosition;
        currentDirection = "";
        state = "";
        inSpawn = true;
        moving = false;
        cornerNumber = 0;
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        string dir = "Right";
        if (currentDirection == "W") dir = "Up";
        if (currentDirection == "A") dir = "Left";
        if (currentDirection == "S") dir = "Down";

        string animation = "Ghost" + state + dir + "_Clip";
        Debug.Log(animation);

        animator.Play(animation);
    }
}
