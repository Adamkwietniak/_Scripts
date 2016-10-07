using UnityEngine;
using System.Collections;

[System.Serializable]
public class AviatorController : MonoBehaviour
{
	public float MaxTimeToClick { get { return maxTimeToClick; } set { maxTimeToClick = value; } }

	public float MinTimeToClick { get { return minTimeToClick; } set { minTimeToClick = value; } }

	private float maxTimeToClick = 0.8f;
	private float minTimeToClick = 0.5f;
	private bool ClickedTwice = false;

	private float minCurrentTime;
	private float maxCurrentTime;

	public bool DoubleClick ()
	{
		if (Time.time >= minCurrentTime && Time.time <= maxCurrentTime) {
			ClickedTwice = true;
			minCurrentTime = 0f;
			maxCurrentTime = 0f;
			return true;
		}

		minCurrentTime = Time.time + minTimeToClick;
		maxCurrentTime = Time.time + MaxTimeToClick;

		return false;
	}

	public float timerToOpenUp = 8.0f;
	WindScript ws;
	UnityStandardAssets.ImageEffects.MotionBlur motionBlur;
	[SerializeField]
	private JointsPoseController posController;
	[SerializeField]
	private Transform root;

	[SerializeField]
	private JointsRandomAnimations[] joints;
	[SerializeField]
	private Transform
		hipSuit;
	private Transform[] hipSuitPoints;
	[SerializeField]
	private Transform[] armSuits;
	private Vector3[] hipSuitRotations;
	private Vector3[] armSuitRotations;
	[SerializeField]
	private Transform
		aviatorRoot;

	[HideInInspector]public float rotationY;
	[HideInInspector]public float velocityY;
	[HideInInspector]public float velocityZ;


	public Vector3 velocity {
		get;
		private set;
	}

	[SerializeField]
	private float suitFrequency;
	[SerializeField]
	private float suitMagnitude;
	private float time;
	[System.NonSerialized]
	public bool parachuteIsOpened = false;

	[SerializeField]
	public Transform parachute;
	private Vector3 parachuteStrSqale;
	private bool isMobilePlatform;

	public void OnAwake ()
	{
		ws = FindObjectOfType <WindScript> ();
		motionBlur = FindObjectOfType<UnityStandardAssets.ImageEffects.MotionBlur> ();
		parachuteStrSqale = parachute.localScale;
		parachute.localScale = 0.01f * Vector3.one;
		parachute.GetChild (0).GetComponent<MeshRenderer> ().enabled = false;
		hipSuitPoints = new Transform[hipSuit.childCount];
		hipSuitRotations = new Vector3[hipSuit.childCount];
		isMobilePlatform = Application.isMobilePlatform;
		for (int i = 0; i < hipSuitRotations.Length; i++) {
			hipSuitPoints [i] = hipSuit.GetChild (i);
			hipSuitRotations [i] = hipSuitPoints [i].localRotation.eulerAngles;
		}
		armSuitRotations = new Vector3[armSuits.Length];
		for (int i = 0; i < armSuitRotations.Length; i++) {
			armSuitRotations [i] = armSuits [i].localRotation.eulerAngles;
		}
		foreach (var item in joints) {
			item.OnStart ();
		}
		posController.OnStartAnimation += (JointsPoseController controller) => {
			if (controller.NewPoseName == "Open parachute") {
				parachute.GetChild (0).GetComponent<MeshRenderer> ().enabled = true;
			}
		};
		posController.OnAnimationComplete += (JointsPoseController controller) => {
			if (controller.NewPoseName == "Open parachute") {
				parachute.GetChild (0).GetComponent<MeshRenderer> ().enabled = true;
			}
			SetCurrentState ();
		};
	}

	void SetCurrentState ()
	{
		foreach (var item in joints) {
			item.SetCurrentValue ();
		}
	}

	void Update ()
	{


		if (parachuteIsOpened) {
			if (parachute.localScale.magnitude < parachuteStrSqale.magnitude) {
				parachute.localScale *= 1.0f + 5.0f * Time.deltaTime;

			} else {
				parachute.localScale = parachuteStrSqale;
			}
		}
		time += Time.deltaTime;
		if (time > 100000.0f) {
			time = 0.0f;
		}
		for (int i = 0; i < hipSuitPoints.Length; i++) {
			hipSuitPoints [i].localRotation = Quaternion.Euler (hipSuitRotations [i] + suitMagnitude * Mathf.Sin (suitFrequency * time + ((float)i)) * (Mathf.PI / 2.0f) * Vector3.right);
		}
		for (int i = 0; i < armSuits.Length; i++) {
			armSuits [i].localRotation = Quaternion.Euler (armSuitRotations [i] + 0.75f * suitMagnitude * Mathf.Sin (suitFrequency * time + ((float)i)) * (Mathf.PI / 2.0f) * Vector3.right);
		}
		if (!posController.inAnimate) {
			foreach (var item in joints) {
				item.AnimateJoint ();
			}
		} 
		VelocityControl ();
		if (posController.NewPoseName == "Salto" || posController.NewPoseName == "From Salto") {
			velocity = velocityY * Vector3.up + velocityZ * VectorOperator.getProjectXZ (velocity.normalized, true);
		} else if (posController.NewPoseName == "Open parachute" || (parachuteIsOpened && posController.NewPoseName == "ParachuteDown") || (parachuteIsOpened && posController.NewPoseName == "ParachuteUp")) {
			velocity = velocityY * Vector3.up - velocityZ * root.up;
			if (posController.NewPoseName == "ParachuteDown") {
				velocity = velocityY * Vector3.up - velocityZ * root.up * 1.5f;
			}
		} else {
			velocity = velocityY * Vector3.up + velocityZ * root.forward;
		}
		velocity *= 3.0f;
		transform.position += velocity * Time.deltaTime;
		transform.Rotate (rotationY * Time.deltaTime * Vector3.up);
		//transform.Rotate (rotationX * Time.deltaTime * Vector3.right);



	}

	public void TimerRecovery ()
	{
		if (timerToOpenUp < 8) {
			timerToOpenUp += Time.deltaTime;
		} 
	}

	void VelocityControl ()
	{


		Debug.Log (rotationY);
		if (posController.NewPoseName == "Squeeze") {
			suitFrequency = 320;
			suitMagnitude = 150;
			motionBlur.blurAmount = 0.8f;

		} else {
			suitFrequency = 50f;
			suitMagnitude = 30f;
			motionBlur.blurAmount = 0.55f;
		}
		if (posController.NewPoseName == "Stop n drop") {
			ClickedTwice = false;
			rotationY = 0.0f;
			velocityY = -5.4f;
			velocityZ = 10.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Slow n hold") {
			rotationY = 0.0f;
			velocityY = -9.0f;
			velocityZ = 10.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Energency stop") {
			rotationY = 0.0f;
			velocityY = -10.0f;
			velocityZ = 2.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Open up") {
			rotationY = 0.0f;
			velocityY = -3.0f;
			velocityZ = 6.0f;
			timerToOpenUp -= Time.deltaTime;

			while (true) {
				if (timerToOpenUp < 0) {
					timerToOpenUp = 8;
				}
				if (timerToOpenUp < 7.9 && timerToOpenUp > 6.5) {
					transform.Translate (Vector3.up * 13.0f * Time.deltaTime);
				} else {
					transform.Translate (Vector3.zero);
				}

				break;
			}

		} else if (posController.NewPoseName == "Squeeze") {
			rotationY = 0.0f;
			velocityY = -14.0f;
			velocityZ = 10.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Proper kinesthetic") {
			rotationY = 0.0f;
			velocityY = -4.0f;
			velocityZ = 7.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Backfly position 1") {
			rotationY = 0.0f;
			velocityY = -15.0f;
			velocityZ = 2.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Backfly position 2") {
			rotationY = 0.0f;
			velocityY = -8.0f;
			velocityZ = 2.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Backfly position 3") {
			rotationY = 0.0f;
			velocityY = -7.0f;
			velocityZ = 2.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Right turn") {
			rotationY = 3.0f /** posController.LerpTime*/;
			velocityY = -4.4f;
			velocityZ = 12.0f;
			DoubleClick ();
			TimerRecovery ();
			if (ClickedTwice == false) {
				transform.Translate (Vector3.right * 18.0f * Time.deltaTime);
			} else if (ClickedTwice == true)
				rotationY = 10.0f;
		} else if (posController.NewPoseName == "Left turn") {
			rotationY = -3.0f /** posController.LerpTime*/;
			velocityY = -4.5f;
			velocityZ = 7.5f;
			TimerRecovery ();
			DoubleClick ();
			if (ClickedTwice == false) {
				transform.Translate (Vector3.right * -18.0f * Time.deltaTime);
			} else if (ClickedTwice == true)
				rotationY = -10.0f;
		} else if (posController.NewPoseName == "Salto") {
			rotationY = 0.0f;
			velocityY = -8.0f;
			velocityZ = 10.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Rotate left") {
			rotationY = -1.5f;
			velocityY = -6.0f;
			velocityZ = 10.0f;
			transform.Translate (Vector3.right * -18.0f * Time.deltaTime);
			TimerRecovery ();
		} else if (posController.NewPoseName == "Rotate right") {
			rotationY = 1.5f;
			velocityY = -6.0f;
			velocityZ = 10.0f;
			transform.Translate (Vector3.right * 18.0f * Time.deltaTime);
			TimerRecovery ();
		} else if (posController.NewPoseName == "Open parachute" || (parachuteIsOpened && posController.NewPoseName == "ParachuteDown") || (parachuteIsOpened && posController.NewPoseName == "ParachuteUp")) {
			float horizontal = 0.0f;
			if (isMobilePlatform) {
				horizontal = Mathf.Clamp (3.0f * Input.gyro.gravity.x, -1.0f, 1.0f);

				if (Mathf.Abs (horizontal) < 0.3f) {
					horizontal = 0.0f;
				}
			} else {
				horizontal = Input.GetAxis ("Horizontal");
			}
			rotationY = 10.0f * horizontal;
			velocityY = -1.5f;
			velocityZ = 2.0f;
		}
	}


	public void SetDefaultRotations ()
	{
		foreach (var item in joints) {
			item.minValue = item.joint.localRotation.eulerAngles;
			item.maxValue = item.joint.localRotation.eulerAngles;
		}
	}
}

[System.Serializable]
public class JointsRandomAnimations
{
	public Transform joint;
	[System.NonSerialized]
	public float time;
	[System.NonSerialized]
	public Vector3 startValue;
	public Vector3 minValue;
	public Vector3 maxValue;
	private Vector3 currentValue;
	public float frequency = 1.0f;
	private Vector3 minValueDelta;
	private Vector3 maxValueDelta;

	public void OnStart ()
	{
		startValue = joint.localRotation.eulerAngles;
		minValueDelta = minValue - startValue;
		maxValueDelta = maxValue - startValue;
		time = Random.Range (0.0f, 0.5f * Mathf.PI / frequency);
	}

	public void SetCurrentValue ()
	{
		startValue = joint.localRotation.eulerAngles;
		currentValue = startValue;
		minValue = startValue + minValueDelta;
		maxValue = startValue + maxValueDelta;
	}

	public void AnimateJoint ()
	{
		time += Time.deltaTime;
		if (time > 100000.0f) {
			time = 0.0f;
		}
		currentValue = Vector3.Lerp (minValue, maxValue, 0.5f + 0.5f * Mathf.Sin (frequency * time));
		joint.localRotation = Quaternion.Lerp (joint.localRotation, Quaternion.Euler (currentValue), 10.0f * Time.deltaTime);
	}

}
