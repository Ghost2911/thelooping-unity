using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform camTransform;
    public float SmoothTime = 0.1f;

    public float shakeMagnitude = 1f;
    public Vector3 Offset = new Vector3(0f, 10f, -10f);
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        Camera.main.transparencySortMode = TransparencySortMode.CustomAxis;
        Camera.main.transparencySortAxis = new Vector3(0, 0, 1);
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + Offset;
            camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        }
    }

    public void CameraShake()
    {
        camTransform.position = camTransform.position - camTransform.up * shakeMagnitude;
    }

}