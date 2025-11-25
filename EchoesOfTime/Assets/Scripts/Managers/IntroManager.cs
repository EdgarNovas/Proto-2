using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private float textScrollSpeed;
    [SerializeField, Multiline(lines:10)] private string text;
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private float introDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayText.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Scenes/OutdoorsScene");
        }
        displayText.rectTransform.localPosition += Vector3.up * textScrollSpeed;

        introDuration -= Time.deltaTime;
        if (introDuration <= 0) SceneManager.LoadScene("Scenes/OutdoorsScene");
    }
}
