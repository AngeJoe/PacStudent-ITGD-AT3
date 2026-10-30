using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteQuickly : MonoBehaviour
{
    public float delay = 1f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
