using UnityEngine;
using UnityEngine.Events;

public class CustomBoxCollider2D : CustomCollider2D
{
    float boundingBoxWidth;
    float boundingBoxHeight;

    override protected void Awake()
    {
        base.Awake();

        boundingBoxWidth = spriteRenderer.bounds.size.x;
        boundingBoxHeight = spriteRenderer.bounds.size.y;
    }

    public float BoundingBoxWidth
    {
        get { return boundingBoxWidth; }
    }

    public float BoundingBoxHeight
    {
        get { return boundingBoxHeight; }
    }
}