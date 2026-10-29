using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public Animator animator;
    public Tweener tweener;
    private Vector3 currentPos;
    public float playerSpeed = 0.25f;
    public AudioPlayer audioPlayer;
    public LevelGenerator levelGenerator;
    public ParticleSystem particles;
    public CherryController cherryController;

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
        currentPos = transform.position;

        var emission = particles.emission;
        emission.enabled = false;
        audioPlayer.isMoving = false;
        audioPlayer.isPellet = false;
        audioPlayer.isPower = false;
        animator.speed = 0f;

        if (Input.GetKey(KeyCode.W)) lastInput = "W";
        if (Input.GetKey(KeyCode.A)) lastInput = "A";
        if (Input.GetKey(KeyCode.S)) lastInput = "S";
        if (Input.GetKey(KeyCode.D)) lastInput = "D";

        if (!tweener.TweenExists(transform))
        {
            Vector3 lastNext = canMove(lastInput);
            Vector3 currentNext = canMove(currentInput);

            if (lastNext != Vector3.zero)
            {
                tweener.AddTween(transform, currentPos, lastNext, playerSpeed);
                setAnimation(lastInput);
                currentInput = lastInput;
                emission.enabled = true;

                playMoveSound(lastNext);
            }
            else if (currentNext != Vector3.zero)
            {
                tweener.AddTween(transform, currentPos, currentNext, playerSpeed);
                setAnimation(currentInput);
                emission.enabled = true;

                playMoveSound(currentNext);
            }
        }
        else
        {
            emission.enabled = true;
            animator.speed = 1f;
        }
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log(other.tag);
    //     if (other.tag == "Cherry")
    //     {
    //         // GameManager.instance.AddScore(100);

    //         // audioPlayer.PlaySound(audioPlayer.cherrySound);

    //         cherryController.destroyCherry();
    //     }
    // }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected: " + other.name);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Inside trigger: " + other.name);
    }

    void playMoveSound(Vector3 nextPos)
    {
        if (levelGenerator.isNormalPellet(nextPos))
        {
            audioPlayer.isMoving = true;
            audioPlayer.isPellet = true;
            audioPlayer.isPower = false;
        }
        else if (levelGenerator.isPowerPellet(nextPos))
        {
            audioPlayer.isMoving = true;
            audioPlayer.isPellet = false;
            audioPlayer.isPower = true;
        }
        else if (levelGenerator.isEmpty(nextPos))
        {
            audioPlayer.isMoving = true;
            audioPlayer.isPellet = false;
            audioPlayer.isPower = false;
        }

        audioPlayer.PlayMove();
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