using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private float interactionDist;
    [SerializeField] private float angleTolerance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && 
            Vector3.Distance(this.transform.position, Camera.main.transform.position) < interactionDist &&
            Vector3.Dot(Camera.main.transform.forward, transform.forward) < -Mathf.Cos(angleTolerance)) 
            SceneManager.LoadScene(targetScene);
    }
}
