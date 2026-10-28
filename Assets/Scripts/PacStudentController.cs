using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public Animator animator;
    public Tweener tweener;
    private Vector3 currentPos;
    public float playerSpeed = 0.25f;

    string lastInput;
    string currentInput;

    void Start()
    {
        
    }

    void Update()
    {
        currentPos = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            tweener.AddTween(transform, currentPos, new Vector3(currentPos.x, currentPos.y + 1f, currentPos.z), playerSpeed);
            animator.SetInteger("direction", 1);

            lastInput = currentInput;
            currentInput = "W";
        }

        if (Input.GetKey(KeyCode.A))
        {
            tweener.AddTween(transform, currentPos, new Vector3(currentPos.x - 1f, currentPos.y, currentPos.z), playerSpeed);
            animator.SetInteger("direction", -2);

            lastInput = currentInput;
            currentInput = "A";
        }

        if (Input.GetKey(KeyCode.S))
        {
            tweener.AddTween(transform, currentPos, new Vector3(currentPos.x, currentPos.y - 1f, currentPos.z), playerSpeed);
            animator.SetInteger("direction", -1);

            lastInput = currentInput;
            currentInput = "S";
        }

        if (Input.GetKey(KeyCode.D))
        {
            tweener.AddTween(transform, currentPos, new Vector3(currentPos.x + 1f, currentPos.y, currentPos.z), playerSpeed);
            animator.SetInteger("direction", 2);
            
            lastInput = currentInput;
            currentInput = "D";
        }

        // Movement - Lerping
        if (!tweener.TweenExists(transform))
        {
            animator.speed = 1f;
            
            // Direction
            if (lastInput == "W")
            {
                tweener.AddTween(transform, currentPos, new Vector3(currentPos.x, currentPos.y + 1f, currentPos.z), playerSpeed);
                animator.SetInteger("direction", 1);

                currentInput = lastInput;
            }

            else if (lastInput == "A")
            {
                tweener.AddTween(transform, currentPos, new Vector3(currentPos.x - 1f, currentPos.y, currentPos.z), playerSpeed);
                animator.SetInteger("direction", -2);

                currentInput = lastInput;
            }

            else if (lastInput == "S")
            {
                tweener.AddTween(transform, currentPos, new Vector3(currentPos.x, currentPos.y - 1f, currentPos.z), playerSpeed);
                animator.SetInteger("direction", -1);

                currentInput = lastInput;
            }

            else if (lastInput == "D")
            {
                tweener.AddTween(transform, currentPos, new Vector3(currentPos.x + 1f, currentPos.y, currentPos.z), playerSpeed);
                animator.SetInteger("direction", 2);
                
                currentInput = lastInput;
            }
        }
    
        // Not Lerping
        else 
        {
            // Stop movement sound
            // Stop animation
            animator.speed = 0f;
        }
    }
}
