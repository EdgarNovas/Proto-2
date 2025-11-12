using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private LayerMask hitMask;

    // Referencias al objeto que estamos controlando actualmente
    private ITimeReversible currentReversible;

    void LateUpdate()
    {
        

        // --- Congelar objeto (E) ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryRaycastAndExecute(hit =>
            {
                if (hit.collider.TryGetComponent<ITimeStoppable>(out var stoppable))
                    stoppable.ToggleFreeze();
            });
        }

        // --- Empezar rebobinado (R) ---
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryRaycastAndExecute(hit =>
            {
                if (hit.collider.TryGetComponent<ITimeReversible>(out var reversible))
                {
                    reversible.StartRewind();
                    currentReversible = reversible;
                }
            });
        }

        // --- LÓGICA DE REBOBINAR (SIN CAMBIOS) ---
        // Esto debe ir FUERA del Raycast, para que podamos dejar de
        // rebobinar aunque no estemos mirando al objeto.
        if (Input.GetKeyUp(KeyCode.R) && currentReversible != null)
        {
            currentReversible.StopRewind();
            currentReversible = null;
        }

        
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
