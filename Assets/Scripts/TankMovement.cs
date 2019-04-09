using UnityEngine;

[RequireComponent(typeof(CustomBoxCollider2D))]
public class TankMovement : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;

    CustomBoxCollider2D boxCollider2D;

    void Awake()
    {
        boxCollider2D = GetComponent<CustomBoxCollider2D>();
        boxCollider2D.OnCollision.AddListener(OnCollisionDetected);
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalMovement = horizontalInput * speed * Time.deltaTime;

        transform.Translate(horizontalMovement, 0f, 0f);
    }

    void OnCollisionDetected(CustomCollider2D collider, Vector2 normal, float penetration)
    {
        float massRatio = boxCollider2D.Mass / collider.Mass;
        float penetrationMult = 1f / (1f + massRatio);
        
        transform.Translate(normal * penetration * penetrationMult);
    }
}