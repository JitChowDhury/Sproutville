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
    [SerializeField] private InputActionAsset inputActions;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoeSound;
    [SerializeField] private AudioClip waterSound;
    [SerializeField] private AudioClip seedSound;



    [SerializeField] private float toolWaitTime = .5f;
    private float toolWaitCounter;
    private Vector3 indicatorTargetPos;
    private bool isGameplayActive;

    public CropController.CropType seedCropType;


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
        isGameplayActive = false;
        UIController.Instance.SwitchTool((int)currentTool);
        UIController.Instance.SwitchSeed(seedCropType);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this == null || gameObject == null)
            return;
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.ResetTrigger("hoeTrigger");
        animator.ResetTrigger("waterTrigger");

        isUsingTool = false;
        toolWaitCounter = 0f;

        EnableGameplay();
        inputActions.Enable();
    }




    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }


    void Update()
    {
        if (actionInput.action.WasPressedThisFrame())
        {
            Debug.Log("ACTION DETECTED");
        }

        if (UIController.Instance != null)
        {
            if (UIController.Instance.ic != null)
            {
                if (UIController.Instance.ic.gameObject.activeSelf == true)
                {
                    return;
                }
            }
            if (UIController.Instance.theShop != null)
            {
                if (UIController.Instance.theShop.gameObject.activeSelf == true)
                {
                    return;
                }
            }

            if (UIController.Instance.theShop != null)
            {
                if (UIController.Instance.pauseScreen.gameObject.activeSelf == true)
                {
                    return;
                }
            }
        }
        if (DialogueUI.Instance != null && DialogueUI.Instance.IsPlaying)
            return;


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
        if (GridController.Instance != null)
        {
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

        else
        {
            toolIndicator.position = new Vector3(0f, 0f, -20f);
        }


    }

    void FixedUpdate()
    {
        if (UIController.Instance != null)
        {
            if (UIController.Instance.ic != null)
            {
                if (UIController.Instance.ic.gameObject.activeSelf == true)
                {
                    return;
                }
            }
            if (UIController.Instance.theShop != null)
            {
                if (UIController.Instance.theShop.gameObject.activeSelf == true)
                {
                    return;
                }
            }
        }
        if (toolWaitCounter > 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (DialogueUI.Instance != null && DialogueUI.Instance.IsPlaying)
            return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }


    void UseTool()
    {


        GrowBlock block;

        // block = FindFirstObjectByType<GrowBlock>();
        // block.PloughSoil();

        block = GridController.Instance.GetCellFromWorldPosition(toolIndicator.position.x - .5f, toolIndicator.position.y - .5f);
        if (block == null)
        {
            Debug.Log("No GrowBlock found under tool indicator");
            return;
        }
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

                    if (CropController.Instance.GetCropInfo(seedCropType).seedAmount > 0)
                    {
                        if (block.PlantCrop(seedCropType))
                        {
                            CropController.Instance.UseSeed(seedCropType);
                            if (seedSound != null)
                                audioSource.PlayOneShot(seedSound);
                        }

                    }

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
        if (hoeSound != null)
            audioSource.PlayOneShot(hoeSound);

        animator.SetTrigger("hoeTrigger");
        // animator.Play(clipName, 0, 0);

        StartCoroutine(ResetToolAfterAnimation());
    }
    void UseWatering()
    {
        if (isUsingTool) return;

        isUsingTool = true;

        int dir = animator.GetInteger("orientation");
        if (waterSound != null)
            audioSource.PlayOneShot(waterSound);

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

    public void SwitchSeed(CropController.CropType newSeed)
    {
        seedCropType = newSeed;
    }
    public void EnableGameplay()
    {
        isGameplayActive = true;
    }

}



