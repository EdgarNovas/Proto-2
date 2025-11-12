using System.Collections.Generic;
using UnityEngine;

public class ReversibleObject : MonoBehaviour,ITimeReversible
{
    // Un "punto en el tiempo" que guarda posici�n y rotaci�n
    private struct PointInTime
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;

        public PointInTime(Vector3 pos, Quaternion rot, Vector3 vel)
        {
            position = pos;
            rotation = rot;
            velocity = vel;
        }
    }

    // Una lista para guardar el historial
    private List<PointInTime> history = new List<PointInTime>();
    [SerializeField] private float maxHistorySeconds = 10f; // Cu�ntos segundos guardar

    private bool isRewinding = false;
    private Rigidbody rb;

    private Vector3 savedVelocity;

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
        savedVelocity = rb.linearVelocity;
        rb.isKinematic = true; 
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false; 

        if (history.Count > 1)
        {
            // Tomamos la velocidad del punto grabado
            rb.linearVelocity = history[history.Count - 1].velocity;
        }
        else
        {
            // Si no hay historial, usamos la que habia antes del rewind
            rb.linearVelocity = savedVelocity;
        }
    }

    private void RewindFrame()
    {
        if (history.Count > 0)
        {
            // Cargar el ultimo punto del historial
            PointInTime point = history[history.Count - 1];
            transform.position = point.position;
            transform.rotation = point.rotation;

            // Eliminarlo de la lista
            history.RemoveAt(history.Count - 1);
            // Si justo después de este frame ya no quedan puntos, aplicamos su última velocidad.
            if (history.Count == 0)
            {
                rb.isKinematic = false;
                rb.linearVelocity = point.velocity;
                isRewinding = false;
            }
        }
        else
        {
            // Si no hay m�s historial, dejar de rebobinar
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
        history.Add(new PointInTime(transform.position, transform.rotation, rb.linearVelocity));
    }
}
