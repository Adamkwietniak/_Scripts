using UnityEngine;
using System.Collections;

public class ShakeCameraScript : MonoBehaviour
{

	public float shakeTimer;
	public float shakeAmount;

	
	// Update is called once per frame
	void Update ()
	{
		if (shakeTimer >= 0) {
			Vector3 ShakePos = Random.insideUnitSphere * shakeAmount; 
			//Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;

			transform.position = new Vector3 (transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z + ShakePos.z);

			shakeTimer -= Time.deltaTime;
		}

		ShakeCameraTwo (0.1f, 1);

	}

	public void ShakeCameraTwo (float shakePower, float shakeDuration)
	{
		shakeAmount = shakePower;
		shakeTimer = shakeDuration;
	}
}
