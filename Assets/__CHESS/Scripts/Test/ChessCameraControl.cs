using System.Collections;
using UnityEngine;

public class ChessCameraControl : MonoBehaviour
{
	public float speed = 5.0f;
	public float farPosition = 0f;
	public float closePosition = 30f;
	private float currentPosition = 0f;
	private bool isFar = true;

	public void SwapCameraPosition()
	{
		StopAllCoroutines();
		StartCoroutine(EnumCameraPosition());
	}

	public IEnumerator EnumCameraPosition ()
	{
		float movePosition = (isFar) ? closePosition : farPosition;
		isFar = !isFar;

		while (Mathf.Abs(currentPosition - movePosition) > 1.0f)
		{
			currentPosition = Mathf.Lerp(currentPosition, movePosition, Time.deltaTime * speed);
			transform.rotation = Quaternion.Euler(new Vector3(currentPosition,0f,0f));
			yield return null;
		}
		currentPosition = movePosition;
	}
}
