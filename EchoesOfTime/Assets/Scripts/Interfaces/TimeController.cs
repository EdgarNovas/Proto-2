using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private LayerMask hitMask;

    
    private ITimeReversible currentReversible;

    public float PenaltyTimer { get; private set; } = 0f;
    void LateUpdate()
    {
        PenaltyTimer += Time.deltaTime;
        if (currentReversible != null && !currentReversible.IsRewinding)
        {
            currentReversible = null;
        }

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryRaycastAndExecute(hit =>
            {
                if (hit.collider.TryGetComponent<ITimeStoppable>(out var stoppable))
                    stoppable.ToggleFreeze();
            });
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            
            if (currentReversible == null)
            {
                
                TryRaycastAndExecute(hit =>
                {
                    if (hit.collider.TryGetComponent<ITimeReversible>(out var reversible))
                    {
                        reversible.StartRewind();
                        currentReversible = reversible; 
                        AddPenalty(2.0f);
                    }
                });
            }
           
            else
            {
                
                currentReversible.StopRewind();
                currentReversible = null; 
            }
        }

       

        
    }

    private void AddPenalty(float seconds)
    {
        PenaltyTimer += seconds;
        
    }

    private void TryRaycastAndExecute(System.Action<RaycastHit> onHit)
    {
        Ray ray = mainCamera.ScreenPointToRay(
     new Vector3(Screen.width / 2, Screen.height / 2)
   );

        // --- AÑADE ESTA LÍNEA ---
        // Dibuja el rayo en la vista "Scene" para ver a dónde apunta
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.yellow, 1.0f);
        // -----------------------

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, hitMask))
        {
            // Si entra aquí, añade un log para estar seguro
            Debug.Log("¡Golpeado! -> " + hit.collider.name, hit.collider.gameObject);
            onHit?.Invoke(hit);
        }
    }
}
