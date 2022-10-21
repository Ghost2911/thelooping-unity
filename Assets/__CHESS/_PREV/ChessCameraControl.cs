using System.Collections;
using UnityEngine;

public class ChessCameraControl : MonoBehaviour
{
	public float speed = 5.0f;
	public Vector3 closePosition = new Vector3(0,0,0);
	public Vector3 closeRotationVector = new Vector3(0,0,0);
	private Quaternion closeRotation;

	private Vector3 farPosition = new Vector3(0,0,0); 
	private Quaternion farRotation;
	public bool isFar = true;

	private void Start()
	{
		closeRotation = Quaternion.Euler(closeRotationVector);
		farPosition = transform.position;
		farRotation = transform.rotation;
	}
	
	public void SwapCameraPosition()
	{
		StopAllCoroutines();
		StartCoroutine(EnumCameraPosition());
	}

	public IEnumerator EnumCameraPosition()
	{
		Quaternion targetRotation = (isFar) ? closeRotation : farRotation;
		Vector3 targetPosition = (isFar) ? closePosition : farPosition;
		isFar = !isFar;

		while (Vector3.Distance(transform.position,targetPosition)>0.1f)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);

			yield return null;
		}
	}
}
