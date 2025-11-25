using System.Collections;
using UnityEngine;

public class PlatformLoop : MonoBehaviour, ITimeReversible,ITimeStoppable
{

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField, Range(0, 1)] private float t;

    public float speed = 3f;

    [Header("Shader")]
    public Material mat;
    string propertyChange = "_Disolve_Amount";
    [SerializeField] float dissolveSpeed = 2f;
    bool teleporting = false;

    bool isStopped = false;

    public bool IsRewinding { get; set; }

    private void Start()
    {
        IsRewinding = false;
    }

    void Update()
    {
        if (isStopped)
        {
            return;
        }
        if (teleporting) return;
        t += (IsRewinding) ? -speed * Time.deltaTime : speed * Time.deltaTime;
        transform.position = t * endPos + (1 - t) * startPos;
        if (t >= 1)
        {
            t = 0;
            StartCoroutine(Dissolve());
        }
        else if (t < 0)
        {
            t = 1;
            StartCoroutine(Dissolve());
        }

        IEnumerator Dissolve()
        {
            teleporting=true;
            float s = 0f;
            while(s < 1f)
            {
                s += Time.deltaTime * dissolveSpeed;
                mat.SetFloat(propertyChange, s);
                yield return null;
            }


            transform.position = t * endPos + (1 - t) * startPos;

            s = 1f;
            while (s > 0f)
            {
                s -= Time.deltaTime * dissolveSpeed;
                mat.SetFloat(propertyChange, s);
                yield return null;
            }

            teleporting = false;
        }
    }

    public void StartRewind()
    {
        IsRewinding = true;
    }

    public void StopRewind()
    {
        IsRewinding = false;
    }

    public void ToggleFreeze()
    {
        isStopped = !isStopped;
    }
}
