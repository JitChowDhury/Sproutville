// -----------------------------------------------------------------------------------------
// using classes
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


// -----------------------------------------------------------------------------------------
// player movement class
public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;
    // static public members


    // -----------------------------------------------------------------------------------------
    // public members
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference actionInput;
    [SerializeField] private Transform toolIndicator;
    [SerializeField] private float toolRange = 3f;

    [SerializeField] private float toolWaitTime = .5f;
    private float toolWaitCounter;
    private Vector3 indicatorTargetPos;


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
    void OnEnable()
    {
        moveInput.action.Enable();
        actionInput.action.Enable();
    }

    void OnDisable()
    {
        moveInput.action.Disable();
        actionInput.action.Disable();
    }

    void Start()
    {
        UIController.Instance.SwitchTool((int)currentTool);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        moveInput.action.Enable();
        actionInput.action.Enable();
    }

    void Update()
    {

        if (toolWaitCounter > 0)
        {
            toolWaitCounter -= Time.deltaTime;
            // rb.linearVelocity = Vector2.zero;
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
            UIController.Instance.SwitchTool((int)currentTool);
        }
        if (actionInput.action.WasPressedThisFrame())
        {
            UseTool();
        }

        // --- TOOL INDICATOR LOGIC ONLY ---

        if (!isUsingTool)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorld.z = 0f;

            Vector3 target = mouseWorld;

            // clamp to range
            Vector2 dir = target - transform.position;
            if (dir.magnitude > toolRange)
            {
                dir = dir.normalized * toolRange;
                target = transform.position + (Vector3)dir;
            }

            // snap AFTER intent
            indicatorTargetPos = new Vector3(
                Mathf.FloorToInt(target.x) + 0.5f,
                Mathf.FloorToInt(target.y) + 0.5f,
                0f
            );
        }

        // smooth visual movement (always runs)
        toolIndicator.position = Vector3.Lerp(
            toolIndicator.position,
            indicatorTargetPos,
            Time.deltaTime * 20f
        );

    }

    void FixedUpdate()
    {
        if (toolWaitCounter > 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }


    void UseTool()
    {
        GrowBlock block;

        // block = FindFirstObjectByType<GrowBlock>();
        // block.PloughSoil();

        block = GridController.Instance.GetBlock(toolIndicator.position.x - .5f, toolIndicator.position.y - .5f);
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
                    block.PlantCrop();
                    break;
                case ToolType.bucket:
                    block.HarvestCrop();
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



