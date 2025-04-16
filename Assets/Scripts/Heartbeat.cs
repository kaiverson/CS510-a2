using UnityEngine;
using UnityEngine.Audio;

public class GhostHeartbeat : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer masterMixer;
    public AudioSource heartbeatSource;
    public string snapshotParameter = "GhostProximity";
    public float maxHeartbeatDistance = 10f;

    [Header("Ghosts")]
    public Transform[] ghosts; 
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        heartbeatSource.loop = true;
    }

    void Update()
    {
        float closestDistance = Mathf.Infinity;
        foreach (Transform ghost in ghosts)
        {
            float dist = Vector3.Distance(player.position, ghost.position);
            if (dist < closestDistance) closestDistance = dist;
        }

        float heartbeatIntensity = Mathf.Clamp01(1 - (closestDistance / maxHeartbeatDistance));

        masterMixer.SetFloat(snapshotParameter, heartbeatIntensity);

        heartbeatSource.pitch = 0.8f + (heartbeatIntensity * 0.4f);

        if (heartbeatIntensity > 0.1f && !heartbeatSource.isPlaying)
        {
            heartbeatSource.Play();
        }
        else if (heartbeatIntensity <= 0.1f && heartbeatSource.isPlaying)
        {
            heartbeatSource.Stop();
        }
    }
}