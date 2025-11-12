using System.Collections.Generic;
using UnityEngine;

public class ReversibleObject : MonoBehaviour,ITimeReversible
{
    // Un "punto en el tiempo" que guarda posición y rotación
    private struct PointInTime
    {
        public Vector3 position;
        public Quaternion rotation;

        public PointInTime(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    // Una lista para guardar el historial
    private List<PointInTime> history = new List<PointInTime>();
    [SerializeField] private float maxHistorySeconds = 10f; // Cuántos segundos guardar

    private bool isRewinding = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isRewinding)
        {
            RewindFrame();
        }
        else
        {
            RecordFrame();
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true; // Quitar de la física mientras se rebobina
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false; // Devolver a la física
    }

    private void RewindFrame()
    {
        if (history.Count > 0)
        {
            // Cargar el último punto del historial
            PointInTime point = history[history.Count - 1];
            transform.position = point.position;
            transform.rotation = point.rotation;

            // Eliminarlo de la lista
            history.RemoveAt(history.Count - 1);
        }
        else
        {
            // Si no hay más historial, dejar de rebobinar
            StopRewind();
        }
    }

    private void RecordFrame()
    {
        // Limpiar el historial si es demasiado largo
        // (Basado en 50 FixedUpdates por segundo por defecto)
        if (history.Count > maxHistorySeconds * 50)
        {
            history.RemoveAt(0);
        }

        // Guardar el estado actual
        history.Add(new PointInTime(transform.position, transform.rotation));
    }
}
