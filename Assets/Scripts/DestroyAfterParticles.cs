using UnityEngine;

public class DestroyAfterParticles : MonoBehaviour
{
    private ParticleSystem particles;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();

        if (particles == null)
        {
            Debug.LogWarning("No ParticleSystem component found on instantiated object.");
        }
    }

    private void Update()
    {
        // Check if the particle system has stopped emitting particles
        if (particles != null && particles.isStopped && particles.particleCount == 0)
        {
            Destroy(gameObject); // Destroy the instantiated particle GameObject
        }
    }
}
