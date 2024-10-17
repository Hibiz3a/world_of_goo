using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float minX = 0f;
    public float maxX = 15f;

    private void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            moveDirection += Vector3.left;
        }

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}