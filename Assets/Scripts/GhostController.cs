using UnityEngine;

public class GhostController : MonoBehaviour
{
    public int ghostNumber = 1;
    public string state = "Normal";
    public LevelGenerator levelGenerator;
    public GameManager gameManager;
    public Transform player;
    public Animator animator;

    Vector3 startPosition;
    float deadTimer = 0;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!GameManager.inst.isPlaying)
        {
            animator.speed = 0;
            return;
        }

        animator.speed = 1;

        if (state == "Dead")
        {
            deadTimer -= Time.deltaTime;

            if (deadTimer <= 0)
            {
                ResetGhost();
                ChangeState(GameManager.inst.GetGhostState());
                GameManager.inst.UpdateMusic();
            }
        }
    }

    public void ChangeState(string newState)
    {
        state = newState;

        if (state == "Dead") deadTimer = 3f;

        int animation = -1;

        if (state == "Normal") animation = 0;
        if (state == "Dead") animation = 1;
        if (state == "Recovering") animation = 2;
        if (state == "Scared") animation = 3;

        animator.SetInteger("state", animation);
    }

    public void ResetGhost()
    {
        transform.position = startPosition;
        deadTimer = 0;
        ChangeState("Normal");
    }
}