using UnityEngine;
using System.Collections;

public class ShakeThatCamera : MonoBehaviour
{

	public float duration = 0.5f;
	public float speed = 1.0f;
	public float magnitude = 0.55f;

	private float xFrequenty = 150;
	private float yFrequenty = 35;
	private float zFrequenty = 15;

	private Transform cam;
	private Vector3 shakedCameraPosition;
	private float shakeCoolDown = 0.3f;
	private Quaternion cameraYRot;
	private Vector3 smoothPivotOffset;

	public Transform playerTr;

	MouseOrbit mo;

	void Start ()
	{
		playerTr = GameObject.FindGameObjectWithTag ("Player").transform;
		mo = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<MouseOrbit> ();
		//smoothPivotOffset = Vector3.Lerp (smoothPivotOffset, mo.target, 10f * Time.deltaTime);
		//cameraYRot = Quaternion.Euler (0, 0, 0);
	}

	public void PlayShake ()
	{
		//shakedCameraPosition = playerTr.position + cameraYRot + 
		StopAllCoroutines ();
		StartCoroutine (Shake ());
	}

	void Update ()
	{
		PlayShake ();
		shakedCameraPosition = playerTr.position + mo.y * smoothPivotOffset;
	}

	IEnumerator Shake ()
	{
		float elapsed = 0.0f;

		Vector3 originalCamPos = Camera.main.transform.position;
		//Vector3 shakeVector = Vector3.zero;

		//float randomStart = Random.Range (-1000.0f, 1000.0f);

		while (elapsed < duration) {
			elapsed += Time.deltaTime;

			float percentComplete = elapsed / duration;
			float damper = 1.0f - Mathf.Clamp (4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;
			Camera.main.transform.position = new Vector3 (x, y, originalCamPos.z);
			yield return null;
			/*float alpha = randomStart + speed * percentComplete;
			float perlin = Mathf.PerlinNoise (elapsed, elapsed);
			shakeVector = transform.right * Mathf.Cos (perlin + xFrequenty) + transform.up * Mathf.Cos (perlin + yFrequenty) + transform.forward * Mathf.Cos (perlin * zFrequenty);
			shakeVector *= magnitude * damper;
			cam.position = shakedCameraPosition + shakeVector;
			transform.position = new Vector3 (x, y, originalCamPos.z);
			yield return new WaitForFixedUpdate ();*/
		}
		//Camera.main.transform.position = originalCamPos;
		cam.transform.position = originalCamPos; //camera?
		//yield return new WaitForSeconds (shakeCoolDown);
	}
}
