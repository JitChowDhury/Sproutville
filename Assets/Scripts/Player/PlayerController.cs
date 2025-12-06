// -----------------------------------------------------------------------------------------
// using classes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

// -----------------------------------------------------------------------------------------
// player movement class
public class PlayerController : MonoBehaviour
{
    // static public members
    public static PlayerController instance;

    // -----------------------------------------------------------------------------------------
    // public members
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference actionInput;

    // -----------------------------------------------------------------------------------------
    // private members
    private Vector2 movement;



    void Awake()
    {
        instance = this;
    }

    void Update()
    {

        movement = moveInput.action.ReadValue<Vector2>().normalized;

        if (actionInput.action.WasPressedThisFrame())
        {
            UseTool();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);


    }

    void UseTool()
    {
        GrowBlock block;

        block = FindFirstObjectByType<GrowBlock>();
        block.PloughSoul();
    }
}
