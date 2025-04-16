using UnityEngine;

public class MistTrigger : MonoBehaviour
{
    public ParticleSystem mistEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mistEffect.Play();
        }
    }
}
