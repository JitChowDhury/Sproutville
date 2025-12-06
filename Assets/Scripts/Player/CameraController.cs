using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // private Transform target;
    [SerializeField] Transform clampMin, clampMax;
    private Camera cam;
    private float halfWidth, halfHeight;
    void Start()
    {
        // target = FindAnyObjectByType<PlayerController>().transform;

        clampMin.SetParent(null);
        clampMax.SetParent(null);
        cam = GetComponent<Camera>();
        halfHeight = cam.orthographicSize;
        halfWidth = cam.orthographicSize * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LateUpdate()
    {
        // transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        Vector3 clampPosition = transform.position;
        clampPosition.x = Mathf.Clamp(clampPosition.x, clampMin.position.x + halfWidth, clampMax.position.x - halfWidth);
        clampPosition.y = Mathf.Clamp(clampPosition.y, clampMin.position.y + halfHeight, clampMax.position.y - halfHeight);
        transform.position = clampPosition;

    }
}
