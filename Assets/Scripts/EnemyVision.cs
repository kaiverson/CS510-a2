using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Player Detection Settings")]
    [Tooltip("The player's Transform (auto-detected if not set)")]
    public Transform player;

    [Tooltip("Vision cone angle in degrees")]
    [Range(0, 180)] public float visionAngle = 45f;

    [Tooltip("Maximum detection distance")]
    public float visionRange = 10f;

    [Header("Debug Settings")]
    public bool drawVisionGizmos = true;
    public Color detectionColor = Color.red;
    public Color normalColor = Color.gray;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    private void Update()
    {
        foreach (Transform enemy in transform)
        {
            if (player == null) return;

            // Calculate direction and distance to player
            Vector3 toPlayer = player.position - enemy.position;
            float distance = toPlayer.magnitude;

            if (distance <= visionRange)
            {
                // Normalize direction and get forward vector
                Vector3 dirNormalized = toPlayer.normalized;
                Vector3 forward = enemy.forward;

                // Calculate dot product and detection threshold
                float dotProduct = Vector3.Dot(forward, dirNormalized);
                float cosThreshold = Mathf.Cos(visionAngle * Mathf.Deg2Rad);

                // Check if player is within vision cone
                if (dotProduct >= cosThreshold)
                {
                    OnPlayerDetected(enemy);
                    continue;
                }
            }

            OnPlayerLost(enemy);
        }
    }

    private void OnPlayerDetected(Transform enemy)
    {
        // Change color when player is detected
        Renderer renderer = enemy.GetComponent<Renderer>();
        if (renderer != null) renderer.material.color = detectionColor;

        // Add your detection logic here (e.g., chase player, shoot, etc.)
    }

    private void OnPlayerLost(Transform enemy)
    {
        // Revert color when player is lost
        Renderer renderer = enemy.GetComponent<Renderer>();
        if (renderer != null) renderer.material.color = normalColor;
    }

    private void OnDrawGizmos()
    {
        if (!drawVisionGizmos) return;

        foreach (Transform enemy in transform)
        {
            // Draw vision cone
            Gizmos.color = Color.yellow;
            Vector3 forward = enemy.forward * visionRange;
            Quaternion leftRot = Quaternion.AngleAxis(-visionAngle, Vector3.up);
            Quaternion rightRot = Quaternion.AngleAxis(visionAngle, Vector3.up);

            Vector3 left = leftRot * forward;
            Vector3 right = rightRot * forward;

            Gizmos.DrawRay(enemy.position, forward);
            Gizmos.DrawRay(enemy.position, left);
            Gizmos.DrawRay(enemy.position, right);
            Gizmos.DrawLine(enemy.position + left, enemy.position + right);
        }
    }
}