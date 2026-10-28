using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public Animator animator;
    public Tweener tweener;
    private Vector3 currentPos;
    public float playerSpeed = 0.25f;
    public AudioPlayer audioplayer;
    public LevelGenerator levelGenerator;

    string lastInput;
    string currentInput;

    Vector3 upVector(Vector3 currentPos) => new Vector3(currentPos.x, currentPos.y + 1, currentPos.z);
    Vector3 leftVector(Vector3 currentPos) => new Vector3(currentPos.x - 1, currentPos.y, currentPos.z);
    Vector3 downVector(Vector3 currentPos) => new Vector3(currentPos.x, currentPos.y - 1, currentPos.z);
    Vector3 rightVector(Vector3 currentPos) => new Vector3(currentPos.x + 1, currentPos.y, currentPos.z);

    void Start()
    {
        
    }

    void Update()
    {
        // animator.speed = 1f;
        currentPos = transform.position;

        if (Input.GetKey(KeyCode.W))
            lastInput = "W";

        if (Input.GetKey(KeyCode.A))
            lastInput = "A";

        if (Input.GetKey(KeyCode.S))
            lastInput = "S";

        if (Input.GetKey(KeyCode.D))
            lastInput = "D";

        // Movement based on Last Input
        if (!tweener.TweenExists(transform))
        {
            Vector3 lastDir = canMove(lastInput);
            Vector3 currentDir = canMove(currentInput);

            if (lastDir != Vector3.zero)
            {
                tweener.AddTween(transform, currentPos, lastDir, playerSpeed);
                setAnimation(lastInput);
                currentInput = lastInput;
            }

            else if (currentDir != Vector3.zero)
            {
                tweener.AddTween(transform, currentPos, currentDir, playerSpeed);
                setAnimation(currentInput);
            }

            // else if (lastInput == "D")
            // {
            //     Vector3 endPos = rightVector(currentPos);
            //     if (levelGenerator.isEmpty(endPos))
            //     {
            //         tweener.AddTween(transform, currentPos, endPos, playerSpeed);
            //         animator.SetInteger("direction", 2);
            //         currentInput = lastInput;
            //     }
            // }
        }

        // No Movement/Lerp
        else 
        {
            // Stop movement sound

            // Stop animation
            // animator.speed = 0f;
        }
    }

    Vector3 canMove(string input)
    {
        Vector3 pos = transform.position;
        Vector3 dir;

        if (input == "W")
            dir = upVector(pos);
        else if (input == "A")
            dir = leftVector(pos);
        else if (input == "S")
            dir = downVector(pos);
        else if (input == "D")
            dir = rightVector(pos);
        else    
            return Vector3.zero;
        
        if (levelGenerator.isEmpty(dir)) return dir;
        return Vector3.zero;
    }

    void setAnimation(string input) 
    {
        if (input == "W") animator.SetInteger("direction", 1);
        if (input == "A") animator.SetInteger("direction", -2);
        if (input == "S") animator.SetInteger("direction", -1);
        if (input == "D") animator.SetInteger("direction", 2);
    }
}
