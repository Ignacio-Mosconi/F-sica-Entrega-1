using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomCollider2D : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnCollision = new UnityEvent();
    protected SpriteRenderer spriteRenderer;

    virtual protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        CollisionManager.Instance.RegisterCollider2D(gameObject.layer, this);
    }
}