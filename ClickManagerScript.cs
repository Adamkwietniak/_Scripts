/*using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class ClickManagerScript : MonoBehaviour
{

	public float MaxTimeToClick { get { return maxTimeToClick; } set { maxTimeToClick = value; } }

	public float MinTimeToClick { get { return minTimeToClick; } set { minTimeToClick = value; } }

	//public bool IsDebug { get { return Isdebug; } set { Isdebug = value; } }

	private float maxTimeToClick = 0.60f;
	private float minTimeToClick = 0.05f;
	private bool ClickedTwice = false;

	private float minCurrentTime;
	private float maxCurrentTime;
	[SerializeField]
	private AviatorController controller;
	[SerializeField]
	private JointsPoseController posController;


	public bool DoubleClick ()
	{
		if (Time.time >= minCurrentTime && Time.time <= maxCurrentTime) {
			Debug.Log ("2click");
			ClickedTwice = true;
			minCurrentTime = 0f;
			maxCurrentTime = 0f;
			return true;
		}

		minCurrentTime = Time.time + minTimeToClick;
		maxCurrentTime = Time.time + MaxTimeToClick;

		return false;
	}


	void Update ()
	{
		Debug.Log (controller.rotationY);
		if (posController.NewPoseName == "Right turn") {
			DoubleClick ();
			if (ClickedTwice == true && posController.NewPoseName == "Right turn") {
				controller.rotationY = 9.0f;

			}

		}
	}
}*/
