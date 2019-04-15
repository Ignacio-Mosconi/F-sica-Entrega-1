using UnityEngine;

[RequireComponent(typeof(CustomBoxCollider2D))]
public class TankMovement : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;

    Animator animator;
    CustomBoxCollider2D boxCollider;

    void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<CustomBoxCollider2D>();
        boxCollider.OnCollision.AddListener(OnCollisionDetected);
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalMovement = horizontalInput * speed * Time.deltaTime;

        bool isMoving = (horizontalMovement != 0f) ? true : false;
        animator.SetBool("Moving", isMoving);

        transform.Translate(horizontalMovement, 0f, 0f);
        ClampHorizontalPosition();
    }

    void ClampHorizontalPosition()
    {
        float leftBound = GameManager.Instance.GetViewBound(ViewBound.Left) + boxCollider.Width * 0.5f;
        float rightBound = GameManager.Instance.GetViewBound(ViewBound.Right) - boxCollider.Width * 0.5f;
        
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftBound, rightBound),
                                        transform.position.y,
                                        transform.position.z);
    }

    void OnCollisionDetected(CustomCollider2D collider, Vector2 normal, float penetration)
    {
        float massRatio = boxCollider.Mass / collider.Mass;
        float penetrationMult = 1f / (1f + massRatio);
        
        transform.Translate(normal * penetration * penetrationMult);

        ClampHorizontalPosition();
    }

    public void DisableMovementAnimation()
    {
        animator.SetBool("Moving", false);
    }
}