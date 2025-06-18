using UnityEngine;

public class Crop : MonoBehaviour
{
    // sorting layers for when crop is seed and crop is growing
    private const int seedSortingLayer = 0;
    private const int plantSortingLayer = 1;
    private const int firstPhaseTransitionDay = 1;

    [Header("Growth Period")]
    [SerializeField] private int daysToGrow = 7;
    [Header("Object Sprite Renderer")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Header("Main Vegetable Sprite")]
    [SerializeField] private Sprite vegetableSprite;
    // vegetable original sprite will be seedling and well transition to stage 1, 2, and 3 overtime
    [Header("Stages of Crop Growth Sprites")]
    [SerializeField] private Sprite[] spriteStages;
    // number of stages the crop has during it's growth
    private int numStages;
    // current amount of days crop has been growing for
    private int currGrowthDays;
    // days after start vegetable will switch sprites
    private int[] transitionDays;
    // current transition day
    private int currTransitionIndex;
    // is crop subscribed to action
    private bool subscribed;
    private bool watered;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                Debug.LogError("Crop does not have Sprite Renderer attached to object!");
            }
        }
    }

    private void Start()
    {
        // if crop is not fixed, subscribe to day change action
        if (!subscribed && ClockManager.Instance != null)
        {
            subscribed = true;
            ClockManager.Instance.OnDayChanged += UpdateCrop;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = seedSortingLayer;
        }

        int numSpriteStages = spriteStages.Length;
        numStages = numSpriteStages;
        transitionDays = new int[numSpriteStages];

        currGrowthDays = 0;

        // in each iteration add delta days * i
        int deltaDays = (daysToGrow / numStages) + 1;
        int i = 0;
        transitionDays[i] = firstPhaseTransitionDay;
        for (i = 1; i < numSpriteStages; i++)
        {
            //Debug.Log("Transition day " + (i + 1) + ": " + (deltaDays * i));
            transitionDays[i] = deltaDays * i;

        }
        currTransitionIndex = 0;
    }

    private void OnEnable()
    {
        // if crop is not fixed, subscribe to day change action
        if (!subscribed && ClockManager.Instance != null)
        {
            subscribed = true;
            ClockManager.Instance.OnDayChanged += UpdateCrop;
        }
    }

    private void OnDisable()
    {
        if (subscribed && ClockManager.Instance != null)
        {
            ClockManager.Instance.OnDayChanged -= UpdateCrop;
            subscribed = false;
        }
    }

    private void UpdateCrop(GameDayOfWeek dayOfWeek)
    {
        /*
        if (!watered)
        {
            return;
        }
        */

        currGrowthDays++;

        if (spriteRenderer == null)
        {
            return;
        }

        if (currGrowthDays >= transitionDays[currTransitionIndex])
        {
            Sprite thisPhaseSprite = spriteStages[currTransitionIndex];
            if (thisPhaseSprite != null)
            {
                spriteRenderer.sortingOrder = plantSortingLayer;
                spriteRenderer.sprite = thisPhaseSprite;
                currTransitionIndex++;
            }
        }
    }

    public void Water()
    {
        watered = true;
    }
}
