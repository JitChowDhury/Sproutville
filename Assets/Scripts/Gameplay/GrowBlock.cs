using UnityEngine;
using UnityEngine.InputSystem;

public class GrowBlock : MonoBehaviour
{
    [SerializeField] private SpriteRenderer theSR;
    [SerializeField] private Sprite soilTilled;
    public enum GrowthStage
    {
        barren,
        ploughed,
        planted,
        growing1,
        growing2,
        ripe
    }

    private GrowthStage currentStage;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Keyboard.current.eKey.wasPressedThisFrame)
         {
             AdvanceStage();

             SetSoilSprite();
         }
         */

    }

    void AdvanceStage()
    {
        currentStage = currentStage + 1;
        if ((int)currentStage >= 6)
        {
            currentStage = GrowthStage.barren;
        }
    }

    public void SetSoilSprite()
    {
        if (currentStage == GrowthStage.barren)
        {
            theSR.sprite = null;
        }
        else
            theSR.sprite = soilTilled;
    }

    public void PloughSoil()
    {
        if (currentStage == GrowthStage.barren)
        {
            currentStage = GrowthStage.ploughed;
            SetSoilSprite();
        }
    }
}
