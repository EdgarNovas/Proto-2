using UnityEngine;

public class ExplodableObject : MonoBehaviour, ITimeExplodable
{
    [SerializeField] private float explosionForce;
    public float ExplosionForce { get => explosionForce; }

    [SerializeField] private float explosionRadius;
    public float ExplosionRadius { get => explosionRadius; }
    public Transform Transform { get => transform; }
}
