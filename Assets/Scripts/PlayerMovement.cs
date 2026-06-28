using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Read keyboard each frame. WASD and arrow keys both map to these axes.
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized; // stop diagonal movement being faster
    }

    void FixedUpdate()
    {
        // Physics changes go in FixedUpdate. Unity 6: it's linearVelocity, not velocity.
        rb.linearVelocity = input * moveSpeed;
    }
}