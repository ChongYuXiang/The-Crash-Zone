using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class EnableTrailOnAwake : MonoBehaviour
{
    private TrailRenderer trail;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        if (trail == null)
        {
            Debug.LogError("TrailRenderer component not found on " + gameObject.name);
            return;
        }

        Debug.Log("TrailRenderer found on " + gameObject.name);

        trail.Clear();
        Debug.Log("TrailRenderer cleared");

        trail.emitting = true;
        Debug.Log("TrailRenderer emitting set to TRUE");
    }

    void Update()
    {
        if (trail != null)
        {
            Debug.Log($"Trail emitting: {trail.emitting}, Position: {transform.position}");
        }
    }
}
