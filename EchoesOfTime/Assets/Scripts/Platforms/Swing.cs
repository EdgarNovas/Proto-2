using UnityEngine;

public class Swing : MonoBehaviour, ITimeStoppable
{
    private bool frozen = false;
    [SerializeField] private float amplitude;
    [SerializeField] private float speed;
    [SerializeField] private float time = 0;

    public void ToggleFreeze() => frozen = !frozen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen) return;

        time += Time.deltaTime;

        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, amplitude * Mathf.Sin(time * speed)));
    }
}
