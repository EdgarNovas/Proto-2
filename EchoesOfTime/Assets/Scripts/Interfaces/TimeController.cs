using System.Threading;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private float sphereCastRadius = 3f;
    [SerializeField] private LayerMask hitMask;

    [Header("VFX")]
    [SerializeField] ArcVFXController arcVFX;


    private ITimeReversible currentReversible;


    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
    
        if (currentReversible != null && !currentReversible.IsRewinding)
        {
            currentReversible = null;
        }

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryRaycastAndExecute(hit =>
            {
                if (hit.collider.TryGetComponent<ITimeStoppable>(out var stoppable))
                {
                    stoppable.ToggleFreeze();
                    AudioManager.instance.sfxSource.loop = false;
                    AudioManager.instance.PlaySFX("StopTime");
                    ChatManager.Instance.MessageBurst(ChatData.Emotion.EXCITED);
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            
            if (currentReversible == null)
            {
                
                TryRaycastAndExecute(hit =>
                {
                    if (hit.collider.TryGetComponent<ITimeReversible>(out var reversible))
                    {
                        reversible.StartRewind();
                        currentReversible = reversible; 
                        TimeManager.Instance.AddTime(10);
                        AudioManager.instance.sfxSource.loop = false;
                        AudioManager.instance.PlaySFX("ReverseTime");
                        ChatManager.Instance.MessageBurst(ChatData.Emotion.EXCITED);
                    }
                });
            }
           
            else
            {
                
                currentReversible.StopRewind();
                currentReversible = null; 
            }
        }

       if(Input.GetKeyDown(KeyCode.V))
        {
            
            TryRaycastAndExecute(hit =>
            {
                if (!hit.collider.TryGetComponent(out ITimeExplodable explodable)) return;

                Debug.Log("boom");
                AudioManager.instance.sfxSource.loop = false;
                AudioManager.instance.PlaySFX("ExplodeTime");
                ChatManager.Instance.MessageBurst(ChatData.Emotion.EXCITED);

                explodable.Explode(this.GetComponent<Player>());
            }
            );
        }

        
    }



    private void TryRaycastAndExecute(System.Action<RaycastHit> onHit)
    {
        
        Transform originTransform = mainCamera != null ? mainCamera.transform : Camera.main.transform;

        
        Debug.Log($"Raycast lanzado desde el objeto: {originTransform.gameObject.name}");

        
        Vector3 origin = originTransform.position;
        Vector3 direction = originTransform.forward;

        Ray ray = new Ray(origin, direction);

        
        Debug.DrawRay(origin, direction * raycastDistance, Color.red, 2.0f);



       
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.yellow, 1.0f);


        if (Physics.SphereCast(ray, sphereCastRadius, out RaycastHit hit, raycastDistance, hitMask))
        {
            if (arcVFX != null)
            {
                arcVFX.FireArc(hit.point);
            }

            onHit?.Invoke(hit);
        }
    }
}
