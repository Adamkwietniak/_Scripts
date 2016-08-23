using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class WindScript : MonoBehaviour
{

	public float hoverForce;
	public Rigidbody rbWingman;
	private AudioSource source;
	public AudioClip windSound;
	private float randomNumber;
	private bool wingDirectionBool;
	public Image windWarningImage;
	private float valueOfAlpha = 0f;
	private bool upColor = false;
	private float blinkingSpeed = 3f;

	void Start ()
	{
		source = GetComponent <AudioSource> ();
		wingDirectionBool = (Random.Range (0, 2) == 0);
		windWarningImage.enabled = false;
	}



	void OnTriggerStay (Collider other)
	{
		
		if (other.tag == "Player") {

			if (wingDirectionBool == true) {
				rbWingman.AddForce (Vector3.right * hoverForce, ForceMode.Acceleration); // podmuch wiatru
			} else {
				rbWingman.AddForce (Vector3.back * hoverForce, ForceMode.Acceleration);
			}

			source.PlayOneShot (windSound);
			BlinkingImage ();

		}
	}

	

	void OnTriggerExit (Collider other)
	{
		rbWingman.velocity = Vector3.zero;  /// powrót do normalnej prędkości
		rbWingman.angularVelocity = Vector3.zero;
		windWarningImage.enabled = false;

	}

	private void BlinkingImage () // metoda odpowiedzialna za mryganie znaczka "uważaj wiater se wieje"
	{
		windWarningImage.enabled = true;
		if (valueOfAlpha <= 1 && upColor == true) {
			valueOfAlpha += Time.deltaTime * blinkingSpeed;
			if (valueOfAlpha >= 1) {
				upColor = false;
				valueOfAlpha = 1;
			}
		}

		if (valueOfAlpha >= 0 && upColor == false) {
			valueOfAlpha -= Time.deltaTime * blinkingSpeed;
			if (valueOfAlpha <= 0) {
				upColor = true;
				valueOfAlpha = 0;
			}
		}
		windWarningImage.color = new Color (255f, 255f, 255f, valueOfAlpha);
	}

	

}

