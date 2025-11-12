using UnityEngine;
using UnityEngine.Events;

public class Dash : MonoBehaviour
{
    [Header("Dash parameters")]
    [SerializeField] private float timer;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float force;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody rb;

    public bool CanDash => timer <= 0f;

    void Start()
    {
        timer = cooldownTime;
        InputHandler.Instance.DashEvent += Perform;
    }

    void Update()
    {
        if(timer > 0) timer -= Time.deltaTime;
    }

    private void Perform()
    {
        if(!CanDash) return;

        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        timer = cooldownTime;
    }
}
