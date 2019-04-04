using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;

    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");

        transform.Translate(horizontalMovement * Time.deltaTime, 0f, 0f);
    }
}