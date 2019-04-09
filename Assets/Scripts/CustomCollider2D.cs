using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : UnityEvent<CustomCollider2D, Vector2, float> {}

[RequireComponent(typeof(SpriteRenderer))]
public class CustomCollider2D : MonoBehaviour
{
    [SerializeField] float mass;
    [HideInInspector] public CollisionEvent OnCollision = new CollisionEvent();
    
    protected SpriteRenderer spriteRenderer;

    virtual protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        CollisionManager.Instance.RegisterCollider2D(gameObject.layer, this);
    }

    public float Mass
    {
        get { return mass; }
    }
}