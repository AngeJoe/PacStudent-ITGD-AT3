using UnityEngine;
using UnityEngine.UI;

public class BorderAnimation : MonoBehaviour
{
    public Image image;
    public Sprite[] frames;
    int curFrame = 0;

    void Update()
    {
        if (Time.frameCount % 30 == 0)
        {
            curFrame++;
            curFrame = curFrame % frames.Length;
            image.sprite = frames[curFrame];
        }
    }
}