using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalMovement = horizontalInput * speed * Time.deltaTime;

        transform.Translate(horizontalMovement, 0f, 0f);
    }
}