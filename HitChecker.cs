﻿using UnityEngine;
using System.Collections;

public class HitChecker : MonoBehaviour
{
	ChangeCameraScript ccs;
	public GameObject bloodPart;
	//public Animator landingAnim;
	[SerializeField]
	private AviatorController controller;
	[SerializeField]
	private JointsPoseController posController;
	private Rigidbody[] bodies;
	private Rigidbody body;
	[SerializeField]
	private Rigidbody rootBody;
	[SerializeField]
	private Rigidbody leftHand;
	[SerializeField]
	private Rigidbody rightHand;
	CameraWithShakeScript cWSS;

	void Awake ()
	{
		cWSS = FindObjectOfType <CameraWithShakeScript> ();
		ccs = (ChangeCameraScript)FindObjectOfType (typeof(ChangeCameraScript)) as ChangeCameraScript;
		bloodPart.SetActive (false);
		//landingAnim.enabled = false;
		body = GetComponent<Rigidbody> ();
		body.useGravity = false;
		bodies = GetComponentsInChildren<Rigidbody> ();
		foreach (var item in bodies) {
			if (item != body) {
				item.isKinematic = true;
				Physics.IgnoreCollision (body.GetComponent<Collider> (), item.GetComponent<Collider> ());
				item.GetComponent<Collider> ().enabled = false;
			}
		}
	}

   
	void OnCollisionEnter (Collision collision)
	{
       
		if (collision.collider.tag == "Ground") {
			if (controller.parachuteIsOpened == false) {
				rootBody.gameObject.AddComponent<FixedJoint> ();
				leftHand.gameObject.AddComponent<FixedJoint> ();
				rightHand.gameObject.AddComponent<FixedJoint> ();
				StartCoroutine (WaitAndReload ());
				cWSS.shake = 0.0f;
			}
			Destroy (body);
			Destroy (GetComponent<Collider> ());
			body.useGravity = true;
			bloodPart.SetActive (true);
			bodies = GetComponentsInChildren<Rigidbody> ();
			if (ccs.cameraIndex == 1) {
				ccs.brokenGlass.enabled = true;
				ccs.changeCameraPossible = false;
			} 
			if (ccs.cameraIndex == 2) {
				ccs.bloodOnScreen.enabled = true;
				ccs.changeCameraPossible = false;
			}
			foreach (var item in bodies) {
				if (item != body) {
					item.isKinematic = false;
					item.GetComponent<Collider> ().enabled = true;

				}
			}
			Destroy (controller);
			Destroy (posController);
			if (ccs.cameraIndex == 0 && collision.collider.tag == "Ground") {
				ccs.changeCameraPossible = false;
			}
		} else if (collision.collider.tag == "Ground" && controller.parachuteIsOpened == true) {
			Destroy (controller);
			//landingAnim.enabled = true;

		}
	}

	IEnumerator WaitAndReload ()
	{
		yield return new WaitForSeconds (1.0f);
		Application.LoadLevel (Application.loadedLevel);
	}
}
