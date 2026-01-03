using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Clamp Bounds")]
    [SerializeField] private Transform clampMin;
    [SerializeField] private Transform clampMax;

    [Header("Settings")]
    [SerializeField] private bool followTarget = true;
    [SerializeField] private bool enableClamping = true;

    private Camera cam;
    private float halfWidth;
    private float halfHeight;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        // Detach bounds so they don't move with parents
        if (clampMin) clampMin.SetParent(null);
        if (clampMax) clampMax.SetParent(null);

        RecalculateCameraSize();
    }

    private void LateUpdate()
    {
        if (followTarget && target != null)
        {
            Follow();
        }

        if (enableClamping)
        {
            ClampCamera();
        }
    }

    private void Follow()
    {
        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.y = target.position.y;
        transform.position = pos;
    }

    private void ClampCamera()
    {
        if (!clampMin || !clampMax) return;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(
            pos.x,
            clampMin.position.x + halfWidth,
            clampMax.position.x - halfWidth
        );

        pos.y = Mathf.Clamp(
            pos.y,
            clampMin.position.y + halfHeight,
            clampMax.position.y - halfHeight
        );

        transform.position = pos;
    }

    private void RecalculateCameraSize()
    {
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }


    public void SetTarget(Transform newTarget, bool snap = true)
    {
        target = newTarget;
        followTarget = newTarget != null;

        if (snap && target != null)
        {
            SnapToTarget(target);
        }
    }

    public void StopFollowing()
    {
        followTarget = false;
    }

    public void ResumeFollowing()
    {
        followTarget = true;
    }

    public void EnableClamping(bool value)
    {
        enableClamping = value;
    }

    public void SnapToTarget(Transform snapTarget)
    {
        Vector3 pos = transform.position;
        pos.x = snapTarget.position.x;
        pos.y = snapTarget.position.y;
        transform.position = pos;

        if (enableClamping)
        {
            ClampCamera();
        }
    }

#if UNITY_EDITOR
    // Auto-update clamp when aspect ratio changes
    private void OnValidate()
    {
        if (cam == null) cam = GetComponent<Camera>();
        RecalculateCameraSize();
    }
#endif
}
