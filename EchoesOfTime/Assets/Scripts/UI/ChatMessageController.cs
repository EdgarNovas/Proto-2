using UnityEngine;
using TMPro;
using System.Collections;

public class ChatMessageController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] CanvasGroup canvasGroup;


    private float fadeDuration = 0.5f;

    public void Setup(string userName, string message, Color userColor, float lifeSpan)
    {
        //Color to Hex for TMPro
        string hexColor = ColorUtility.ToHtmlStringRGB(userColor);

        messageText.text = $"<color=#{hexColor}><b>{userName}</b></color> {message}";

        StartCoroutine(LifeCycle(lifeSpan));
    }

    IEnumerator LifeCycle(float lifeSpan)
    {
        //POP 
        transform.localScale = Vector3.zero;
        float t = 0;
        float popDuration = 0.3f;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            float progress = t / popDuration;

            
            float scaleAmount = Mathf.Sin(progress * Mathf.PI) * 0.2f + 1.0f;

    
            float s = 1.71f;
            float p = progress - 1;
            float easeBackOut = (p * p * ((s + 1) * p + s) + 1);

            transform.localScale = Vector3.one * easeBackOut;

            yield return null;
        }
        transform.localScale = Vector3.one;

        
        yield return new WaitForSeconds(lifeSpan);

        //Fade Out
        t = 0;
        float startAlpha = canvasGroup.alpha;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, t / fadeDuration);
            yield return null;
        }

        Destroy(gameObject);
    }
}
