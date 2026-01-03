using UnityEngine;

public class GuideNPC : MonoBehaviour
{
    public CropController.CropType crop;
    [Header("Interaction")]
    public float interactionDistance = 1.5f;
    public KeyCode interactKey = KeyCode.E;


    [Header("Dialogue")]
    [TextArea]
    public string[] firstDialogueLines;

    [TextArea]
    public string[] repeatDialogueLines;

    [Header("Starter Items")]
    public int chilliSeedAmount = 5;

    private bool hasGivenItems = false;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);

        if (distance <= interactionDistance && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    void Interact()
    {
        if (DialogueUI.Instance.IsPlaying)
            return;

        switch (TutorialManager.Instance.CurrentState)
        {
            case TutorialManager.TutorialState.NotStarted:
                DialogueUI.Instance.PlayLines(new string[]
                {
                "Welcome to Sprout Ville!",
                "Make your own farm here.",
                "I'll give you 5 chilli seeds.",
                "Use your hotbar tools to work the farm.",
                "Hoe to till soil, watering can to water.",
                "Use seeds to plant and bucket to harvest.",
                "Plant them inside the fenced area."
                });

                GiveStarterItems();
                TutorialManager.Instance.SetState(TutorialManager.TutorialState.SeedsGiven);
                break;


            case TutorialManager.TutorialState.SeedsGiven:
                DialogueUI.Instance.PlayLines(new string[]
                {
                "Plant the seeds inside the fenced farm area.",
                "Water them every day to help them grow."
                });
                break;

            case TutorialManager.TutorialState.SeedPlanted:
            case TutorialManager.TutorialState.CropGrowing:
                DialogueUI.Instance.PlayLines(new string[]
                {
                "Good job!",
                "Now wait for the crops to grow.",
                "Water them daily. Each day they will grow."
                });
                break;

            case TutorialManager.TutorialState.CropFullyGrown:
                DialogueUI.Instance.PlayLines(new string[]
                {
                "Your crops are fully grown!",
                "You can now visit the shop behind the fence.",
                "Sell your harvest there."
                });
                TutorialManager.Instance.SetState(TutorialManager.TutorialState.ShopUnlocked);
                break;

            case TutorialManager.TutorialState.ShopUnlocked:
                DialogueUI.Instance.PlayLines(new string[]
                {
                "Keep farming and selling crops.",
                "Build your own Sprout Ville!"
                });
                break;
        }
    }



    void GiveStarterItems()
    {
        CropController.Instance.AddSeed(crop, 5);
        Debug.Log("Gave player " + chilliSeedAmount + " chilli seeds");
    }
}

