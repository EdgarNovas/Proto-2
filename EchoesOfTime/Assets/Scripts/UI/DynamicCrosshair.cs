using UnityEngine;

public class DynamicCrosshair : MonoBehaviour
{
    

    [Header("References")]
    [SerializeField] Rigidbody playerRb; 
    [SerializeField] RectTransform crosshairRect;

    [Header("Settings")]
    [SerializeField] float minSize = 50f;
    [SerializeField] float maxSize = 150f;
    [SerializeField] float speedMultiplier = 2f; 
    [SerializeField] float smoothTime = 0.1f; 

    private float currentSize;
    private float velocityRef; 

    void Update()
    {
        if (playerRb == null) return;

        
        float speed = playerRb.linearVelocity.magnitude;

       
        float targetSize = Mathf.Clamp(minSize + (speed * speedMultiplier), minSize, maxSize);

        
        currentSize = Mathf.SmoothDamp(currentSize, targetSize, ref velocityRef, smoothTime);

        
        crosshairRect.sizeDelta = new Vector2(currentSize, currentSize);

        
         crosshairRect.Rotate(Vector3.forward * speed * Time.deltaTime * 10f);
    }

    
    public void Pulse()
    {
        currentSize = maxSize * 1.5f; 
    }
}
