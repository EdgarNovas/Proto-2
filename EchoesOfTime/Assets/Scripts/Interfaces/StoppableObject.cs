using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class StoppableObject : MonoBehaviour, ITimeStoppable
{
    private Rigidbody rb;
    private Vector3 storedVelocity;
    private Vector3 storedAngularVelocity;
    
    private bool isCurrentlyFrozen = false;
    public float penaltyTimer = 0f;

    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward ;
        
        
    }

    private void Update()
    {
        if (isCurrentlyFrozen)
        {
            penaltyTimer += Time.deltaTime;
        }
        
    }

    public void ToggleFreeze()
    {
        
        isCurrentlyFrozen = !isCurrentlyFrozen;

        if (isCurrentlyFrozen)
        {
            
            storedVelocity = rb.linearVelocity;
            storedAngularVelocity = rb.angularVelocity;
            rb.isKinematic = true;
        }
        else
        {
            TimeManager.Instance.AddTime(penaltyTimer);
            penaltyTimer = 0f;
            rb.isKinematic = false;
            rb.linearVelocity = storedVelocity;
            rb.angularVelocity = storedAngularVelocity;
        }
    }
}



