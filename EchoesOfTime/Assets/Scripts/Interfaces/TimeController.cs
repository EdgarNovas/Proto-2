using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float raycastDistance = 100f;

    // Referencias al objeto que estamos controlando actualmente
    private ITimeReversible currentReversible;

    void Update()
    {
        // Lanzar un raycast desde el centro de la cámara
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        // Ponemos el 'if (Physics.Raycast...)' al principio
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            // --- LÓGICA DE CONGELAR ACTUALIZADA ---
            if (Input.GetKeyDown(KeyCode.F))
            {
                // ¿El objeto que golpeamos implementa la interfaz?
                if (hit.collider.TryGetComponent<ITimeStoppable>(out var stoppable))
                {
                    // ¡Sí! Solo dile que cambie su estado.
                    // El objeto se encargará del resto.
                    stoppable.ToggleFreeze();
                }
            }

            // --- LÓGICA DE REBOBINAR (SIN CAMBIOS) ---
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (hit.collider.TryGetComponent<ITimeReversible>(out var reversible))
                {
                    reversible.StartRewind();
                    currentReversible = reversible;
                }
            }
        } // Fin del 'if (Physics.Raycast...)'

        // --- LÓGICA DE REBOBINAR (SIN CAMBIOS) ---
        // Esto debe ir FUERA del Raycast, para que podamos dejar de
        // rebobinar aunque no estemos mirando al objeto.
        if (Input.GetKeyUp(KeyCode.R) && currentReversible != null)
        {
            currentReversible.StopRewind();
            currentReversible = null;
        }

        
    }
}
