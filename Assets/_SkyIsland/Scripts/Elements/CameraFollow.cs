using UnityEngine;
using System.Collections;


/// <summary>
/// Cinematic Drone Camera Intro + Dynamic Third-Person Follow:
///   Phase 1: Dynamic Drone Swoop across the map.
///   Phase 2: Transition to a dynamic follow behind the player.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    [Header("Drone Settings")]
    public float flyByDuration = 7.5f;
    public float transitionDuration = 2.5f;

    [Header("Follow Settings")]
    // Tighter, more playable follow offset
    public Vector3 followOffset = new Vector3(0, 4f, -13f); 

    private bool _introFinished = false;


    [Header("Mouse Control")]
    public bool enableMouseControl = true;
    public float mouseSensitivity = 3f;
    private float currentYaw = 0f;
    private float currentPitch = 20f; // varsayılan kamera açısı

    public void Init(Transform player)
    {
        playerTransform = player;
        StopAllCoroutines();
        StartCoroutine(DroneSequence());
    }

    void LateUpdate()
    {
        if (!_introFinished || playerTransform == null) return;

        // Mouse input
        if (enableMouseControl)
        {
            currentYaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentPitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            currentPitch = Mathf.Clamp(currentPitch, -30f, 60f); // yukarı-aşağı sınır
        }

        Vector3 offset = Quaternion.Euler(currentPitch, currentYaw, 0f) * followOffset;
        transform.position = playerTransform.position + offset;

        transform.LookAt(playerTransform.position + Vector3.up * 1.5f);
    }

    IEnumerator DroneSequence()
    {
        _introFinished = false;
        yield return null;

        // Waypoints for the drone swoop [P1 -> P2 -> P3]
        Vector3 p1 = new Vector3(40f, 35f, -30f); 
        Vector3 p2 = new Vector3(8f, 6f, 0f);    
        Vector3 p3 = new Vector3(-30f, 30f, 25f); 

        Vector3 lookAtStart = new Vector3(0, 5, 0);
        Vector3 lookAtMid   = new Vector3(14, -2, 5);
        Vector3 lookAtFinal = new Vector3(-2, 11, 13);

        float elapsed = 0f;
        while (elapsed < flyByDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flyByDuration;
            float curvedT = Mathf.SmoothStep(0, 1, t);
            
            Vector3 pos = Vector3.Lerp(Vector3.Lerp(p1, p2, curvedT), Vector3.Lerp(p2, p3, curvedT), curvedT);
            
            Vector3 targetLook;
            if (t < 0.5f) targetLook = Vector3.Lerp(lookAtStart, lookAtMid, t * 2f);
            else targetLook = Vector3.Lerp(lookAtMid, lookAtFinal, (t - 0.5f) * 2f);
            
            transform.position = pos;
            transform.rotation = Quaternion.LookRotation(targetLook - transform.position);
            yield return null;
        }

        // Phase 2: Transition to Dynamic Follow
        if (playerTransform != null)
        {
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            elapsed = 0f;
            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / transitionDuration);
                
                Vector3 targetFollowPos = playerTransform.TransformPoint(followOffset);
                transform.position = Vector3.Lerp(startPos, targetFollowPos, t);
                
                Vector3 lookTarget = playerTransform.position + Vector3.up * 1.5f;
                transform.rotation = Quaternion.Slerp(startRot, Quaternion.LookRotation(lookTarget - transform.position), t);
                yield return null;
            }
        }

        _introFinished = true;
    }

    public void SetTargetInstant(Transform player)
    {
        playerTransform = player;
        StopAllCoroutines();
        _introFinished = true;
    }
}
