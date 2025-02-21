using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float movementForce = 10f;
    public float maxSpeed = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if(moveInput * rb.linearVelocityX < 0)
        {
            rb.linearVelocityX /= 2;
        }

        if (rb.linearVelocityX < maxSpeed)
            rb.AddRelativeForceX(moveInput * movementForce);
    }
}
