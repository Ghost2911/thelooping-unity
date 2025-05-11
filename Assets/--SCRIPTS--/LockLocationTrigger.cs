using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LockLocationTrigger : LocationTrigger
{
    public GameObject lockSphere;
    public Color colorMaterial;
    public Color color2Material;
    public float dissolveDuration = 2f;
    public AnimationCurve dissolveCurve = AnimationCurve.EaseInOut(0, 1, 1, 0); 
    public float sphereRadius = 26f;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            LocationPresentor.instance?.ShowLocationName(locationName);
            Statistic.instance?.OnEnterInArea(locationName);
            OnLocationEnter.Invoke();
            if (soundtrack != null)
                GlobalSettings.instance.ChangeBackgroundSoundtrack(soundtrack);
            if (cameraPosition != null)
                GlobalSettings.instance.SetCameraTraget(cameraPosition);
            if (lockSphere != null)
            {
                player.GetComponent<EntityStats>().DeathEvent.AddListener(this.HideSphere);
                StartCoroutine(ShowSphere());
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            OnLocationExit.Invoke();
            if (soundtrack != null)
                GlobalSettings.instance.ChangeBackgroundSoundtrack(null);
            if (cameraPosition != null)
                GlobalSettings.instance.SetCameraTraget(other.transform);
        }
    }

    void Update()
    {
        if (lockSphere != null)
        {
            if (player != null)
            {
                float distanceToCenter = Vector3.Distance(transform.position, player.transform.position);
                
                if (distanceToCenter > sphereRadius)
                {
                    Vector3 directionToCenter = (transform.position - player.transform.position).normalized;
                    player.transform.position = transform.position + directionToCenter * sphereRadius;
                }
            }
        }
    }

    IEnumerator ShowSphere()
    {
        Renderer renderer = lockSphere.GetComponent<Renderer>();
        Material targetMaterial = null;
       
        if (renderer != null)
            targetMaterial = renderer.material;
        
        if (targetMaterial == null) yield break;

        float elapsedTime = 0f;

        targetMaterial.SetColor(Shader.PropertyToID("_Color"), colorMaterial);
        targetMaterial.SetColor(Shader.PropertyToID("_Color2"), color2Material);
        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / dissolveDuration);
            float curveValue = dissolveCurve.Evaluate(normalizedTime);
            
            float currentValue = Mathf.Lerp(1f, 0f, curveValue);
            targetMaterial.SetFloat(Shader.PropertyToID("_DissolveAmount"), currentValue);

            yield return null;
        }

        targetMaterial.SetFloat(Shader.PropertyToID("_DissolveAmount"), 0f);
    }

    public void HideSphere()
    {
        StartCoroutine(HideSphereCoroutine()); 
        player.GetComponent<EntityStats>().DeathEvent.RemoveListener(this.HideSphere);
    }

    IEnumerator HideSphereCoroutine()
    {
        Renderer renderer = lockSphere.GetComponent<Renderer>();
        Material targetMaterial = null;
       
        if (renderer != null)
            targetMaterial = renderer.material;
        
        if (targetMaterial == null) yield break;

        float elapsedTime = 0f;

        targetMaterial.SetColor(Shader.PropertyToID("_Color"), colorMaterial);
        targetMaterial.SetColor(Shader.PropertyToID("_Color2"), color2Material);
        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / dissolveDuration);
            float curveValue = dissolveCurve.Evaluate(normalizedTime);
            
            float currentValue = Mathf.Lerp(0f, 1f, curveValue);
            targetMaterial.SetFloat(Shader.PropertyToID("_DissolveAmount"), currentValue);

            yield return null;
        }

        targetMaterial.SetFloat(Shader.PropertyToID("_DissolveAmount"), 1f);
    }
}
