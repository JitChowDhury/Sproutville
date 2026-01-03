using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public enum TutorialState
    {
        NotStarted,
        SeedsGiven,
        SeedPlanted,
        CropGrowing,
        CropFullyGrown,
        ShopUnlocked,
        Completed
    }

    public TutorialState CurrentState { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(TutorialState newState)
    {
        if (newState > CurrentState)
        {
            CurrentState = newState;
            Debug.Log("Tutorial State = " + newState);
        }
    }
}
