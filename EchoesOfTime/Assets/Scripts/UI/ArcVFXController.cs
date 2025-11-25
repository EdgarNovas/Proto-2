using System.Collections;
using UnityEngine;

public class ArcVFXController : MonoBehaviour
{
    [SerializeField] float timeToDisable = 0.2f;
    [SerializeField] Transform travelingChild;   
    [SerializeField] Transform originPoint;      

    private Coroutine fireRoutine;

    [Header("Configuración")]
    [SerializeField] float travelSpeed = 150f;   
    [SerializeField] float stayDuration = 0.2f; 
                                                 
    public void FireArc(Vector3 hitPoint)
    {
        
        gameObject.SetActive(true);

        
        if (fireRoutine != null) StopCoroutine(fireRoutine);
        fireRoutine = StartCoroutine(TravelRoutine(hitPoint));
    }

    IEnumerator TravelRoutine(Vector3 targetPos)
    {
       
        transform.position = originPoint.position;

        travelingChild.localPosition = Vector3.zero;

       
        transform.LookAt(targetPos);

        float distance = Vector3.Distance(transform.position, targetPos);
        float currentDist = 0f;

        while (currentDist < distance)
        {
            
            transform.position = originPoint.position;
          
            transform.LookAt(targetPos);

            currentDist += Time.deltaTime * travelSpeed;

            // Opción A: Mover en local Z (Si el padre mira al objetivo)
            travelingChild.localPosition = new Vector3(0, 0, currentDist);

            // Opción B: Si prefieres moverlo en mundo (descomenta si la Opción A falla)
            // travelingChild.position = Vector3.MoveTowards(travelingChild.position, targetPos, Time.deltaTime * travelSpeed);

            yield return null;
        }

        // PASO 3: Asegurar llegada exacta
        travelingChild.position = targetPos;

        // PASO 4: Esperar (Impacto)
        float timer = 0;
        while (timer < stayDuration)
        {
            timer += Time.deltaTime;
            // Mantenemos el origen pegado al jugador
            transform.position = originPoint.position;
            // El hijo se queda pegado al impacto
            travelingChild.position = targetPos;

            // Re-orientamos para que el cuerpo del rayo se estire correctamente
            transform.LookAt(targetPos);

            yield return null;
        }

        // PASO 5: Apagar
        gameObject.SetActive(false);
    }
}
