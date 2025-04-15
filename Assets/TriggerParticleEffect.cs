using UnityEngine;

public class TriggerParticleEffect : MonoBehaviour
{
    public ParticleSystem particles; 
    public string triggerTag = "ParticleTrigger"; 
    public Transform particlesTransform;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag(triggerTag))
        {
            if (particles != null)
            {
                particlesTransform.position = new Vector3(transform.position.x,0.01f,transform.position.z);
                particles.Play();
            }
        }
    }
}