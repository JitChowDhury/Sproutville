using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
public class NPCScheduleController : MonoBehaviour
{
    public Transform[] dayRoamPoints;
    public Transform[] nightPathPoints;
    public Transform nightPoint;

    public float nightTime = 20f;
    public float morningTime = 7f;

    public SpriteRenderer spriteRenderer;
    public Collider2D npcCollider;

    private NPCMovement movement;
    private Transform currentTarget;
    private int roamIndex;
    private int nightPathIndex;
    private bool isPausedForDialogue;

    private enum NPCState
    {
        Roaming,
        GoingHome,
        StayingHome
    }

    private NPCState state;

    void Awake()
    {
        movement = GetComponent<NPCMovement>();
    }

    void Start()
    {
        EvaluateInitialState();
    }

    void Update()
    {
        if (DialogueUI.Instance != null && DialogueUI.Instance.IsPlaying)
        {
            PauseForDialogue();
            return;
        }
        else if (isPausedForDialogue)
        {
            ResumeAfterDialogue();
        }

        if (TimeController.Instance == null)
            return;

        float absoluteTime = Mathf.Lerp(
            7f,
            26f,
            TimeController.Instance.GetDayNormalizedTime()
        );

        bool isNight = absoluteTime >= nightTime;
        UpdateVisibility(isNight);

        HandleSchedule(absoluteTime);
        HandleMovement();
    }

    void HandleSchedule(float time)
    {
        if (time >= nightTime)
        {
            if (state != NPCState.GoingHome && state != NPCState.StayingHome)
            {
                state = NPCState.GoingHome;
                nightPathIndex = 0;
                currentTarget = nightPathPoints.Length > 0
                    ? nightPathPoints[nightPathIndex]
                    : nightPoint;
            }
        }
        else
        {
            if (state != NPCState.Roaming)
            {
                state = NPCState.Roaming;
                roamIndex = 0;
                currentTarget = dayRoamPoints[roamIndex];
            }
        }
    }

    void HandleMovement()
    {
        if (currentTarget == null)
        {
            movement.Stop();
            return;
        }

        Vector2 dir = currentTarget.position - transform.position;

        if (dir.magnitude < 0.1f)
        {
            OnTargetReached();
            return;
        }

        movement.SetMovement(dir);
    }

    void OnTargetReached()
    {
        movement.Stop();

        if (state == NPCState.Roaming)
        {
            roamIndex = (roamIndex + 1) % dayRoamPoints.Length;
            currentTarget = dayRoamPoints[roamIndex];
        }
        else if (state == NPCState.GoingHome)
        {
            nightPathIndex++;

            if (nightPathIndex >= nightPathPoints.Length)
            {
                state = NPCState.StayingHome;
                currentTarget = null;
            }
            else
            {
                currentTarget = nightPathPoints[nightPathIndex];
            }
        }
    }

    void PauseForDialogue()
    {
        if (isPausedForDialogue)
            return;

        isPausedForDialogue = true;
        movement.Stop();
    }

    void ResumeAfterDialogue()
    {
        isPausedForDialogue = false;
    }

    void UpdateVisibility(bool isNight)
    {
        spriteRenderer.enabled = !isNight;
        npcCollider.enabled = !isNight;
    }

    void EvaluateInitialState()
    {
        float absoluteTime = TimeController.Instance != null
            ? Mathf.Lerp(7f, 26f, TimeController.Instance.GetDayNormalizedTime())
            : morningTime;

        if (absoluteTime >= nightTime)
        {
            state = NPCState.StayingHome;
            transform.position = nightPoint.position;
            movement.Stop();
            UpdateVisibility(true);
        }
        else
        {
            state = NPCState.Roaming;
            roamIndex = 0;
            currentTarget = dayRoamPoints[roamIndex];
            UpdateVisibility(false);
        }
    }
}
