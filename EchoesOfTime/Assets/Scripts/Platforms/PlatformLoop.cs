using System.Collections;
using UnityEngine;

public class PlatformLoop : MonoBehaviour
{

    public float alturaMax = 5f;     
    public float alturaMin = 0f;
    public float speed = 3f;

    [Header("Shader")]
    public Material mat;
    string propertyChange = "_Disolve_Amount";
    [SerializeField] float dissolveSpeed = 2f;
    bool teleporting = false;

    void Update()
    {
        if (teleporting) return;
        transform.position += Vector3.up * speed * Time.deltaTime;
        if (transform.position.y >= alturaMax)
        {
            StartCoroutine(DissolveYTeleport());
        }

        IEnumerator DissolveYTeleport()
        {
            teleporting=true;
            float t = 0f;
            while(t < 1f)
            {
                t += Time.deltaTime * dissolveSpeed;
                mat.SetFloat(propertyChange, t);
                yield return null;
            }

            Vector3 pos = transform.position;
            pos.y = alturaMin;
            transform.position = pos;

            t = 1f;
            while (t > 0f)
            {
                t -= Time.deltaTime * dissolveSpeed;
                mat.SetFloat(propertyChange, t);
                yield return null;
            }

            teleporting = false;
        }
    }
}
