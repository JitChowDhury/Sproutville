using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Clamp Bounds (Auto-found per scene)")]
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
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        RecalculateCameraSize();
    }

    private void LateUpdate()
    {
        if (followTarget && target != null)
            Follow();

        if (enableClamping && clampMin && clampMax)
            ClampCamera();
    }

    // ---------------- SCENE LOAD ----------------

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        clampMin = null;
        clampMax = null;

        if (PlayerController.Instance != null)
            SetTarget(PlayerController.Instance.transform, true);

        StartCoroutine(DelayedClampSearch());
    }

    private IEnumerator DelayedClampSearch()
    {
        yield return null; // wait one frame so scene objects exist

        GameObject minObj = GameObject.FindGameObjectWithTag("CameraClampMin");
        GameObject maxObj = GameObject.FindGameObjectWithTag("CameraClampMax");

        if (!minObj || !maxObj)
        {
            Debug.LogWarning(
                $"Camera clamps missing in scene '{SceneManager.GetActiveScene().name}'"
            );
            yield break;
        }

        clampMin = minObj.transform;
        clampMax = maxObj.transform;

        clampMin.SetParent(null);
        clampMax.SetParent(null);

        SnapToTarget(target);
    }

    // ---------------- CAMERA LOGIC ----------------

    private void Follow()
    {
        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.y = target.position.y;
        transform.position = pos;
    }

    private void ClampCamera()
    {
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

    // ---------------- PUBLIC API ----------------

    public void SetTarget(Transform newTarget, bool snap = true)
    {
        target = newTarget;
        followTarget = newTarget != null;

        if (snap && target != null)
            SnapToTarget(target);
    }

    public void SnapToTarget(Transform snapTarget)
    {
        if (snapTarget == null) return;

        Vector3 pos = transform.position;
        pos.x = snapTarget.position.x;
        pos.y = snapTarget.position.y;
        transform.position = pos;

        if (enableClamping && clampMin && clampMax)
            ClampCamera();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        RecalculateCameraSize();
    }
#endif
}
