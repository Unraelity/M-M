using UnityEngine;

public class MovementController : MonoBehaviour, IMovable
{

    [Header("Move Settings")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private OrientationController orientation;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private float maxSpeed = 3f;                 // max speed of character
    private Vector2 velocity;
    private Vector2 acceleration;
    private Vector2 previousPosition;
    private Vector2 previousVelocity;
    private Vector2 dir;

    public Vector2 Velocity
    {
        get { return velocity; }
        private set { velocity = value; }
    }

    public Vector2 Acceleration
    {
        get { return acceleration; }
        private set { acceleration = value; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
        private set { maxSpeed = value; }
    }

    public void SetDirection(Vector2 dir)
    {
        this.dir = dir;
    }

    private void Awake()
    {

        if (rb == null)
        {

            rb = GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                Debug.LogError("No Rigidbody2D Component Attached");
            }
        }

        if (orientation == null)
        {

            orientation = GetComponent<OrientationController>();

            if (orientation == null)
            {
                Debug.LogError("No Rigidbody2D Component Attached");
            }
        }
    }


    protected virtual void FixedUpdate()
    {

        // calculate current position and velocity
        Vector2 currentPosition = (Vector2)transform.position;
        velocity = (currentPosition - previousPosition) / Time.fixedDeltaTime; // Instantaneous velocity

        // calculate acceleration
        acceleration = (velocity - previousVelocity) / Time.fixedDeltaTime; // Instantaneous acceleration

        // update previous position and velocity for next frame
        previousPosition = currentPosition;
        previousVelocity = velocity;

        if (dir != Vector2.zero)
        {

            Vector3 movePosition = (Vector2)transform.position + dir * maxSpeed * Time.fixedDeltaTime;
            rb.MovePosition(movePosition);
            orientation.UpdateOrientation(dir.x);
        }
    }

    public void MovePosition(Vector2 targetPos)
    {
        rb.MovePosition(targetPos);
    }

    public bool ReachedDestination(Vector2 target)
    {

        if (Vector2.Distance(transform.position, target) > stopDistance)
        {
            return true;
        }

        return false;
    }

    public bool ReachedDestination(Vector2 target, float stopDistance)
    {

        if (Vector2.Distance(transform.position, target) > stopDistance)
        {
            return true;
        }

        return false;
    }
}
