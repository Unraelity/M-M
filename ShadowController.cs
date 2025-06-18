using UnityEngine;

public class ShadowController : MonoBehaviour
{      
    [Header("Shadow Settings")]
    [SerializeField] private bool fixedShadow = false;             // if shadow should change in length of height
    [SerializeField] private float shadowStartingAngle = 45f;      // angle shadow will start at beginning of day
    [SerializeField] private float shadowEndingAngle = -45f;       // angle shadow will end at ending of day
    [SerializeField] private float maxShadowLength = 1.5f;         // length shadow will start at beginning of day
    [SerializeField] private float minShadowLength = 1f;           // length shadow will end at ending of day
    [Header("Necessary Components")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Transform modelTransform;
    private float currYAngle = 0;
    private float currZAngle = 0;
    private bool subscribed; 

    public bool FixedShadow { get { return fixedShadow; } }
    public float ShadowStartingAngle { get { return shadowStartingAngle; } }
    public float ShadowEndingAngle { get { return shadowEndingAngle; } }
    public float MaxShadowLength { get { return maxShadowLength; } }
    public float MinShadowLength { get { return minShadowLength; } }
    
    private void Awake()
    {
        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();

            if (sprite == null)
            {
                Debug.LogError("Sprite in ShadowController not assigned");
            }
        }

        if (modelTransform == null)
        {

            OrientationController controller = GetComponentInParent<OrientationController>();
            modelTransform = controller.Model.transform;

            if (modelTransform == null)
            {
                Debug.LogError("Transform for Model in ShadowController not assigned");
            }
        }
    }

    private void Start()
    {
        if (!fixedShadow && !subscribed && DayNightCycle.Instance != null)
        {
            subscribed = true;
            DayNightCycle.Instance.OnShadowUpdate += UpdateShadow;
        }
    }

    private void OnEnable()
    {
        // if shadow is not fixed, subscribe to shadow update action
        if (!fixedShadow && !subscribed && DayNightCycle.Instance != null)
        {
            subscribed = true;
            DayNightCycle.Instance.OnShadowUpdate += UpdateShadow;
        }
    }

    private void OnDisable() {

        if (subscribed && DayNightCycle.Instance != null)
        {
            DayNightCycle.Instance.OnShadowUpdate -= UpdateShadow;
            subscribed = false;
        }
    }

    private void Update()
    {
        if (modelTransform.localScale.x == -1)
        {
            FlipShadow();
        }
        else
        {
            ResetShadow();
        }

        transform.localRotation = Quaternion.Euler(0, currYAngle, currZAngle);
    }

    private void UpdateShadow(float timePercent)
    {
        // calculate the rotation angle based on time of day
        currZAngle = Mathf.Lerp(shadowStartingAngle, shadowEndingAngle, timePercent);
        //Debug.Log(objectName + " z-angle: " + currZAngle);

        // calculate the scale based on time of day
        float shadowScaleY;
        if (timePercent <= 0.5f) {
                shadowScaleY = Mathf.Lerp(maxShadowLength, minShadowLength, timePercent * 2);
        } 
        else {
            shadowScaleY = Mathf.Lerp(minShadowLength, maxShadowLength, (timePercent - 0.5f) * 2);
        }

        transform.localScale = new Vector3(transform.localScale.x, shadowScaleY, transform.localScale.z);
    }

    private void FlipShadow()
    {
        currYAngle = 180;
        sprite.flipX = true;
    }

    private void ResetShadow()
    {
        currYAngle = 0;
        sprite.flipX = false;
    }
}