using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class StoppableObject : MonoBehaviour, ITimeStoppable
{
    private Rigidbody rb;
    private Vector3 storedVelocity;
    private Vector3 storedAngularVelocity;

    private bool isCurrentlyFrozen = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward ;
    }

    public void ToggleFreeze()
    {
        // Invierte el estado actual
        isCurrentlyFrozen = !isCurrentlyFrozen;

        if (isCurrentlyFrozen)
        {
            // Guardar el estado actual de la física
            storedVelocity = rb.linearVelocity;
            storedAngularVelocity = rb.angularVelocity;

            // Congelar el objeto
            rb.isKinematic = true;
        }
        else
        {
            // Devolver el objeto a la física
            rb.isKinematic = false;
            rb.linearVelocity = storedVelocity;
            rb.angularVelocity = storedAngularVelocity;
        }
    }
}



