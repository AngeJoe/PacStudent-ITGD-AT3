using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public GameObject pac;
    public Animator animator;
    public Tweener tweener;

    private Vector3 topLeft = new Vector3(-12f, 13f, 0f);
    private Vector3 topRight = new Vector3(-7f, 13f, 0f);
    private Vector3 bottomRight = new Vector3(-7f, 9f, 0f);
    private Vector3 bottomLeft = new Vector3(-12f, 9f, 0f);

    private Vector3 currentPos;

    void Start()
    {

    }

    void Update()
    {
        currentPos = pac.transform.position;

        if (currentPos == topLeft) 
        {
            tweener.AddTween(pac.transform, pac.transform.position, topRight, 1.5f);
            animator.SetInteger("direction", 2);
        }

        if (currentPos == topRight) 
        {
            tweener.AddTween(pac.transform, pac.transform.position, bottomRight, 1.5f);
            animator.SetInteger("direction", -1);
        }

        if (currentPos == bottomRight) 
        {
            tweener.AddTween(pac.transform, pac.transform.position, bottomLeft, 1.5f);
            animator.SetInteger("direction", -2);
        }

        if (currentPos == bottomLeft) 
        {
            tweener.AddTween(pac.transform, pac.transform.position, topLeft, 1.5f);
            animator.SetInteger("direction", 1);
        }
    }
}
