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

    public enum ToolType
    {
        plough,
        wateringCan,
        seeds,
        bucket

    }

    public ToolType currentTool;

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

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            currentTool++;
            if ((int)currentTool >= 4)
            {
                currentTool = 0;
            }
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentTool = ToolType.plough;
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentTool = ToolType.wateringCan;
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentTool = ToolType.seeds;
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentTool = ToolType.bucket;
        }

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
        // block.PloughSoil();

        if (block != null)
        {
            switch (currentTool)
            {
                case ToolType.plough:

                    block.PloughSoil();
                    break;
                case ToolType.wateringCan:

                    break;
                case ToolType.seeds:

                    break;
                case ToolType.bucket:

                    break;

            }
        }
    }
}
