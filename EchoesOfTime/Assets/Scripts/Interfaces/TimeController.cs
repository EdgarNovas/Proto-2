using System.Threading;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private float sphereCastRadius = 3f;
    [SerializeField] private LayerMask hitMask;

    
    private ITimeReversible currentReversible;

    void LateUpdate()
    {
    
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
                        TimeManager.Instance.AddTime(10);
                    }
                });
            }
           
            else
            {
                
                currentReversible.StopRewind();
                currentReversible = null; 
            }
        }

       if(Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Check");
            TryRaycastAndExecute(hit =>
            {
                if (!hit.collider.TryGetComponent(out ITimeExplodable explodable)) return;

                Debug.Log("boom");

                explodable.Explode(this.GetComponent<Player>());
            }
            );
        }

        
    }



    private void TryRaycastAndExecute(System.Action<RaycastHit> onHit)
    {
        Ray ray = mainCamera.ScreenPointToRay(
     new Vector3(Screen.width / 2, Screen.height / 2)
   );

       
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.yellow, 1.0f);
        

        if (Physics.SphereCast(ray,2, out RaycastHit hit, raycastDistance, hitMask))
        {

            onHit?.Invoke(hit);
        }
    }
}
