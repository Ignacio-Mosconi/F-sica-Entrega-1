using UnityEngine;

[RequireComponent(typeof(CustomBoxCollider2D))]
public class TankMovement : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;

    CustomBoxCollider2D boxCollider;
    bool isMoving = false;

    void Awake()
    {
        boxCollider = GetComponent<CustomBoxCollider2D>();
        boxCollider.OnCollision.AddListener(OnCollisionDetected);
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalMovement = horizontalInput * speed * Time.deltaTime;

        isMoving = (horizontalMovement != 0f) ? true : false;

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

    public bool IsMoving
    {
        get { return isMoving; }
    }
}