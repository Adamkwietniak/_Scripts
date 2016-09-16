using UnityEngine;
using System.Collections;

public class CameraShakeScript : MonoBehaviour
{

	public Transform camTransform;
	public float shakeDuration = 0f;
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1f;
	private Vector3 originalPose;

	// Use this for initialization
	void Awake ()
	{
		if (camTransform == null) {
			camTransform = GetComponent (typeof(Transform)) as Transform;
		}
	}

	void OnEnable ()
	{
		originalPose = camTransform.localPosition;
	}
	// Update is called once per frame
	void Update ()
	{
		if (shakeDuration > 0) {
			camTransform.localPosition = originalPose + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
		} else {
			shakeDuration = 0f;
			camTransform.localPosition = originalPose;
		}
	}
}
