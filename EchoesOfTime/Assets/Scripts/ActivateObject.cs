using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateObject : MonoBehaviour
{
    public GameObject objetoActivar;
    
    [SerializeField] string nameSceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objetoActivar.SetActive(true);
            SceneManager.LoadScene(nameSceneToLoad);

        }
    }
}
