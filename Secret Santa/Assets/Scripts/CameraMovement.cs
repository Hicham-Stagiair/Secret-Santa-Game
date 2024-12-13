using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed;
    
    private Vector2 moveDirection;
    
    void Update()
    {
        ProcessInputs();
    }
    
    void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        moveDirection = new Vector2(moveX, moveY).normalized;
    }
    
    private void Move()
    {
        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed * Time.deltaTime;
    }
}
