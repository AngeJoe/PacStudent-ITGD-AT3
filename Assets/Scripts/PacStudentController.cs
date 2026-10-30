
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
    public ParticleSystem wallParticles;
    public ParticleSystem deathParticles;
    public CherryController cherryController;

    bool wallBumped = false;
    string lastInput = "";
    string currentInput = "";

    Vector3 upVector(Vector3 currentPos) => new Vector3(currentPos.x, currentPos.y + 1, currentPos.z);
    Vector3 leftVector(Vector3 currentPos) => new Vector3(currentPos.x - 1, currentPos.y, currentPos.z);
    Vector3 downVector(Vector3 currentPos) => new Vector3(currentPos.x, currentPos.y - 1, currentPos.z);
    Vector3 rightVector(Vector3 currentPos) => new Vector3(currentPos.x + 1, currentPos.y, currentPos.z);

    void Start()
    {
        animator.SetInteger("direction", 2);
        animator.speed = 0;
    }

    void Update()
    {
        if (!GameManager.inst.isPlaying)
        {
            if (!GameManager.inst.isDying)
                setMoving(false);
            else
            {
                var emission = particles.emission;
                emission.enabled = false;
            }
            return;
        }

        currentPos = transform.position;
        setMoving(false);

        audioPlayer.isMoving = false;
        audioPlayer.isPellet = false;
        audioPlayer.isPower = false;

        if (Input.GetKey(KeyCode.W)) lastInput = "W";
        if (Input.GetKey(KeyCode.A)) lastInput = "A";
        if (Input.GetKey(KeyCode.S)) lastInput = "S";
        if (Input.GetKey(KeyCode.D)) lastInput = "D";

        // Movement based on Last Input
        if (!tweener.TweenExists(transform))
        {
            if (tryToTeleport()) return;

            Vector3 lastNext = canMove(lastInput);
            Vector3 currentNext = canMove(currentInput);

            if (lastNext != Vector3.zero)
            {
                tweener.AddTween(transform, currentPos, lastNext, playerSpeed);
                setAnimation(lastInput);
                currentInput = lastInput;
                setMoving(true);
                wallBumped = false;
                Instantiate(particles);
                playMoveSound(lastNext);
            }
            else if (currentNext != Vector3.zero)
            {
                tweener.AddTween(transform, currentPos, currentNext, playerSpeed);
                setAnimation(currentInput);
                setMoving(true);
                wallBumped = false;
                Instantiate(particles);
                playMoveSound(currentNext);
            }
            else if (lastInput != "" && !wallBumped)
            {
                wallBumped = true;
                Instantiate(wallParticles, currentPos + 0.5f * getMoveDir(lastInput), Quaternion.identity);
                audioPlayer.audioSource.PlayOneShot(audioPlayer.wallSound);
            }
        }
        else
        {
            setMoving(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.inst.isPlaying)
            return;

        if (other.CompareTag("Pellet") || other.CompareTag("PowerPellet"))
        {
            other.enabled = false;

            if (other.CompareTag("PowerPellet"))
            {
                GameManager.inst.AddScore(50);
                GameManager.inst.StartScaredTimer();
            }
            else
            {
                GameManager.inst.AddScore(10);
            }

            GameManager.inst.PelletCollected();
            levelGenerator.pelletGone(currentPos);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Cherry"))
        {
            if (other.gameObject == cherryController.curCherry)
            {
                other.enabled = false;
                GameManager.inst.AddScore(100);
                audioPlayer.audioSource.PlayOneShot(audioPlayer.cherrySound);
                cherryController.destroyCherry();
            }
        }
        else if (other.CompareTag("Ghost"))
        {
            GhostController ghost = other.GetComponent<GhostController>();

            if (ghost.state == "")
            {
                GameManager.inst.PlayerHit();
            }
            else if (ghost.state != "Dead")
            {
                GameManager.inst.GhostEaten(ghost);
            }
        }
    }

    void setMoving(bool moving)
    {
        animator.speed = moving ? 1f : 0f;

        var emission = particles.emission;
        emission.enabled = moving;
    }

    void playMoveSound(Vector3 nextPos)
    {
        audioPlayer.isMoving = true;
        audioPlayer.isPellet = levelGenerator.isNormalPellet(nextPos);
        audioPlayer.isPower = levelGenerator.isPowerPellet(nextPos);
        audioPlayer.PlayMove();
    }

    bool tryToTeleport()
    {
        if (Mathf.Abs(transform.position.y) > 0.1f)
            return false;

        if (transform.position.x <= -13 && (lastInput == "A" || currentInput == "A"))
        {
            transform.position = new Vector3(14, 0, transform.position.z);
            currentInput = "A";
            lastInput = "A";
            return true;
        }

        if (transform.position.x >= 14 && (lastInput == "D" || currentInput == "D"))
        {
            transform.position = new Vector3(-13, 0, transform.position.z);
            currentInput = "D";
            lastInput = "D";
            return true;
        }

        return false;
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

    Vector3 getMoveDir(string input)
    {
        Vector3 pos = Vector3.zero;
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
        
        return dir;
    }

    void setAnimation(string input)
    {
        if (input == "W") animator.SetInteger("direction", 1);
        if (input == "A") animator.SetInteger("direction", -2);
        if (input == "S") animator.SetInteger("direction", -1);
        if (input == "D") animator.SetInteger("direction", 2);
    }
}