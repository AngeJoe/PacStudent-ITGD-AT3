using UnityEngine;
using System.Collections.Generic;

public class Tweener : MonoBehaviour
{
    private List<Tween> activeTweens = new List<Tween>();

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < activeTweens.Count; ++i) 
        {
            Tween activeTween = activeTweens[i];
            float distance = Vector3.Distance(activeTween.Target.position, activeTween.EndPos);

            if (distance > 0.02f) 
            {
                float t = (Time.time - activeTween.StartTime) / activeTween.Duration;
                activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, t);
            }

            if (distance <= 0.02f) 
            {
                activeTween.Target.position = activeTween.EndPos;
                activeTweens.RemoveAt(i);
                --i;
            }
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration) 
    {
        if (!TweenExists(targetObject)) 
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
            return true;
        } 
        else 
            return false;
    }

    public bool TweenExists(Transform target) 
    {
        foreach (Tween tween in activeTweens) {
            if (tween.Target == target)
                return true;
        }
        return false;
    }
}
