using UnityEngine;

public interface ITimeExplodable
{
    public float ExplosionForce { get; }
    public float ExplosionRadius { get; }
    public Transform Transform { get; }
    public void Explode(Player player)
    {
        if (!player.TryGetComponent(out Rigidbody rb)) return;

        rb.AddExplosionForce(ExplosionForce, Transform.position, ExplosionRadius);
    }
}
