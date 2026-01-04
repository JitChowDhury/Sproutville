using UnityEngine;

public class NPCNightOnly : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    void Awake()
    {
        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (TimeController.Instance == null)
            return;

        sprite.enabled = TimeController.Instance.IsNight();
    }
}
