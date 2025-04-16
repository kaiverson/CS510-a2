using UnityEngine;

public class FloatingOrb : MonoBehaviour
{
    public Transform[] waypoints;

    public float moveSpeed = 2f;
    public float pauseTime = 1f;
    private int currentWaypoint = 0;
    private float pauseTimer = 0f;
    private bool isMoving = true;

    public Transform player;
    public Renderer orbRenderer;
    public float maxIntensity = 10f;

    private Material orbMaterial;
    private static readonly int EmissiveColor = Shader.PropertyToID("_EmissionColor");

    void Start()
    {
        orbMaterial = orbRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toOrb = (transform.position - player.position).normalized;
        float dot = Vector3.Dot(player.forward, toOrb);
        float intensityFactor = Mathf.Max(0f, dot) * maxIntensity;
        Color targetColor = Color.Lerp(Color.white, Color.red, intensityFactor / maxIntensity);
        orbMaterial.SetColor(EmissiveColor, targetColor * intensityFactor);


        if (waypoints.Length == 0) return;

        if (isMoving)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                waypoints[currentWaypoint].position,
                Time.deltaTime * moveSpeed
            );

            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
            {
                isMoving = false;
                pauseTimer = pauseTime;
            }
        }
        else
        {
            // Pause before moving to next waypoint
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                isMoving = true;
            }
        }
    }
}
