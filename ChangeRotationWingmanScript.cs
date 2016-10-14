using UnityEngine;
using System.Collections;

public class ChangeRotationWingmanScript : MonoBehaviour
{

	public Transform wingmanTransform;
	private float smooth = 1f;
	private Quaternion targetRotation;

	// Use this for initialization
	void Start ()
	{
		targetRotation = wingmanTransform.transform.rotation;

	}


	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player") {
			targetRotation *= Quaternion.AngleAxis (30, Vector3.right); //odwrotność to po prostu -30

		}

	}

	void Update ()
	{
		wingmanTransform.transform.rotation = Quaternion.Lerp (wingmanTransform.transform.rotation, targetRotation, 5 * smooth * Time.deltaTime);
	}
}

