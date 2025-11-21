using System.Collections;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static TimeManager Instance {  get; private set; }

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI timeAddedText;
    Vector3 postimeAddedStart;

    [SerializeField] float timer = 0f;

    int timeToShow = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            postimeAddedStart = timeAddedText.transform.position;
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
        timeAddedText.transform.position -= new Vector3(0,50,0);
    }

    IEnumerator TextAddedMove(float timetoAdd)
    {

        while (Vector3.Equals(postimeAddedStart, timeAddedText.transform.position))
        {



            yield return null;
        }



    }

    public void RemoveTime(float timeToRemove)
    {
        timer -= timeToRemove;
    }


}
