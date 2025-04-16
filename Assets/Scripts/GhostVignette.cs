using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GhostVignette : MonoBehaviour
{
    public PostProcessVolume volume;
    public Transform[] ghosts;
    public float maxDistance = 5f;
    private Vignette vignette;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume.profile.TryGetSettings(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        float closestGhostDist = GetClosestGhostDistance();
        float lerpValue = Mathf.Clamp01(1 - (closestGhostDist / maxDistance));

        vignette.intensity.value = Mathf.Lerp(0f, 0.4f, lerpValue);
    }

    float GetClosestGhostDistance()
    {
        float minDist = Mathf.Infinity;
        foreach (Transform ghost in ghosts)
        {
            float dist = Vector3.Distance(transform.position, ghost.position);
            if (dist < minDist) minDist = dist;
        }
        return minDist;
    }
}
