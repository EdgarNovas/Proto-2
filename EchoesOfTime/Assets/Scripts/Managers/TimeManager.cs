using System.Collections;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static TimeManager Instance {  get; private set; }

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI timeAddedText;

    [Header("Settings")]
    [SerializeField] float flyDuration = 0.5f;       
    [SerializeField] float pauseDuration = 0.3f;      
    [SerializeField] Color penaltyColor = Color.red;
    [SerializeField] Color normalColor = Color.white;

    private float timer = 0f;
    private Vector3 initialPenaltyPos;
    private Vector3 targetPos;
    private Coroutine currentPenaltyRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

       
        if (timeAddedText != null)
        {
            initialPenaltyPos = timeAddedText.transform.position;
            timeAddedText.alpha = 0;
        }

        if (timeText != null)
        {
            targetPos = timeText.transform.position;
            timeText.color = normalColor;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // "F2" significa 2 decimales.
        timeText.text = timer.ToString("F2");
    }

    public void AddTime(float timeToAdd)
    {
       
        timer += timeToAdd;

      
        if (currentPenaltyRoutine != null) StopCoroutine(currentPenaltyRoutine);

        currentPenaltyRoutine = StartCoroutine(PenaltySequence(timeToAdd));
    }

    public void RemoveTime(float timeToRemove)
    {
        timer -= timeToRemove;
    }

   
    IEnumerator PenaltySequence(float amount)
    {
        // POP 
        timeAddedText.transform.position = initialPenaltyPos;
        timeAddedText.text = "+" + amount.ToString("F1") + "s";
        timeAddedText.alpha = 1;
        timeAddedText.transform.localScale = Vector3.one * 1.5f; 

        
        float t = 0;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            timeAddedText.transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, t / 0.2f);
            yield return null;
        }

        
        yield return new WaitForSeconds(pauseDuration);

        
        float journey = 0f;
        while (journey < 1f)
        {
            journey += Time.deltaTime / flyDuration;

            
            timeAddedText.transform.position = Vector3.Lerp(initialPenaltyPos, timeText.transform.position, journey);

            
            timeAddedText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.5f, journey);

            yield return null;
        }

        
        timeAddedText.alpha = 0; 
        StartCoroutine(ShakeTimerEffect());
    }

    
    IEnumerator ShakeTimerEffect()
    {
        timeText.color = penaltyColor; 
        Vector3 originalPos = timeText.transform.position;

        float elapsed = 0.0f;
        float duration = 0.3f;
        float magnitude = 10f; 

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            timeText.transform.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        timeText.transform.position = originalPos; 
        timeText.color = normalColor; 
    }


}
