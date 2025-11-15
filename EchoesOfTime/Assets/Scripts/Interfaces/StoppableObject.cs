using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class StoppableObject : MonoBehaviour, ITimeStoppable
{
    private Rigidbody rb;
    private Vector3 storedVelocity;
    private Vector3 storedAngularVelocity;

    
    private bool isCurrentlyFrozen = false;
    [SerializeField] float penaltyTime = 10f;
    [SerializeField] float speed = 1;
    public TravelDirection direction;

    public enum TravelDirection
    {
        Forward,
        Backward,
        Right,
        Left,
        Up,
        Down
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 directionToGo;
        switch (direction)
        {
            case TravelDirection.Forward:
                directionToGo = transform.forward;
                break;

            case TravelDirection.Backward:
                directionToGo = -transform.forward;
                break;

            case TravelDirection.Right:
                directionToGo = transform.right;
                break;

            case TravelDirection.Left:
                directionToGo = -transform.right;
                break;
            case TravelDirection.Up:
                directionToGo = transform.up;
                break;
            case TravelDirection.Down:
                directionToGo = -transform.up;
                break;
            default:
                directionToGo = transform.forward;
                break;
        }
        rb.linearVelocity = directionToGo * speed ;
    }



    public void ToggleFreeze()
    {
        
        isCurrentlyFrozen = !isCurrentlyFrozen;

        if (isCurrentlyFrozen)
        {
            
            storedVelocity = rb.linearVelocity;
            storedAngularVelocity = rb.angularVelocity;
            rb.isKinematic = true;
            TimeManager.Instance.AddTime(penaltyTime);
        }
        else
        {
            
            rb.isKinematic = false;
            rb.linearVelocity = storedVelocity;
            rb.angularVelocity = storedAngularVelocity;
        }
    }
}



