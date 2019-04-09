using UnityEngine;

[RequireComponent(typeof(CustomBoxCollider2D))]
public class TankMovement : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;

    void Awake()
    {
        CustomBoxCollider2D boxCollider2D = GetComponent<CustomBoxCollider2D>();

        boxCollider2D.OnCollision.AddListener(OnCollisionDetected);
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalMovement = horizontalInput * speed * Time.deltaTime;

        transform.Translate(horizontalMovement, 0f, 0f);
    }

    void OnCollisionDetected()
    {
        Debug.Log("Collision!");
    }
}