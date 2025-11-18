using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class StoppableObject : MonoBehaviour, ITimeStoppable
{
    private Rigidbody rb;
    private Vector3 storedVelocity;
    private Vector3 storedAngularVelocity;
    
    private bool isCurrentlyFrozen = false;
    [SerializeField] float penaltyTime = 10f;

    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward ;
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



