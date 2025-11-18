using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static TimeManager Instance {  get; private set; }

    [SerializeField] TextMeshProUGUI timeText;

    [SerializeField] float timer = 0f;

    int timeToShow = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        timeToShow = (int)timer;
        timeText.text = timeToShow.ToString();
    }


    public void AddTime(float timeToAdd)
    {
        timer += timeToAdd;
    }


    public void RemoveTime(float timeToRemove)
    {
        timer -= timeToRemove;
    }


}
