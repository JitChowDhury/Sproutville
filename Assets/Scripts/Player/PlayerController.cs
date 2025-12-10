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
    [SerializeField] private Animator animator;
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference actionInput;

    [SerializeField] private float toolWaitTime = .5f;
    private float toolWaitCounter;

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

    void Start()
    {
        UIController.instance.SwitchTool((int)currentTool);
    }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (toolWaitCounter > 0)
        {
            toolWaitCounter -= Time.deltaTime;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        }

        movement = moveInput.action.ReadValue<Vector2>().normalized;

        bool hasSwitchedTool = false;
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            currentTool++;
            if ((int)currentTool >= 4)
            {
                currentTool = 0;

            }
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentTool = ToolType.plough;
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentTool = ToolType.wateringCan;

            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentTool = ToolType.seeds;
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentTool = ToolType.bucket;
            hasSwitchedTool = true;
        }
        if (hasSwitchedTool == true)
        {
            UIController.instance.SwitchTool((int)currentTool);
        }
        if (actionInput.action.WasPressedThisFrame())
        {
            UseTool();
        }
    }

    void FixedUpdate()
    {


    }

    void UseTool()
    {
        GrowBlock block;

        block = FindFirstObjectByType<GrowBlock>();
        // block.PloughSoil();
        toolWaitCounter = toolWaitTime;
        if (block != null)
        {

            switch (currentTool)
            {
                case ToolType.plough:

                    block.PloughSoil();
                    UseHoe();
                    break;
                case ToolType.wateringCan:
                    UseWatering();
                    block.WaterSoil();

                    break;
                case ToolType.seeds:

                    break;
                case ToolType.bucket:

                    break;

            }
        }
    }
    bool isUsingTool = false;
    void UseHoe()
    {
        if (isUsingTool) return;

        isUsingTool = true;

        int dir = animator.GetInteger("orientation");


        animator.SetTrigger("hoeTrigger");
        // animator.Play(clipName, 0, 0);

        StartCoroutine(ResetToolAfterAnimation());
    }
    void UseWatering()
    {
        if (isUsingTool) return;

        isUsingTool = true;

        int dir = animator.GetInteger("orientation");


        animator.SetTrigger("waterTrigger");
        // animator.Play(clipName, 0, 0);

        StartCoroutine(ResetToolAfterAnimation());
    }

    private IEnumerator ResetToolAfterAnimation()
    {
        yield return null;

        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        isUsingTool = false;
        animator.ResetTrigger("waterTrigger"); // optional cleanup
        animator.ResetTrigger("hoeTrigger");
    }
}
