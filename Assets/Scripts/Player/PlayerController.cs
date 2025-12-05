using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    [SerializeField] private InputActionReference moveInput;

    void Update()
    {
        Vector2 input = moveInput.action.ReadValue<Vector2>();
        rb.linearVelocity = input.normalized * moveSpeed;

        // FIX: stable diagonal → choose dominant axis
        float animX = 0;
        float animY = 0;

        if (input.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                animX = Mathf.Sign(input.x);
            else
                animY = Mathf.Sign(input.y);
        }

        animator.SetFloat("moveX", animX);
        animator.SetFloat("moveY", animY);

        animator.SetFloat("speed", rb.linearVelocity.magnitude);
    }
}
