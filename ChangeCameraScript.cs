using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeCameraScript : MonoBehaviour
{

	public Camera[] cameras;
	[HideInInspector]public int cameraIndex;
	public Camera photoCameraView;
	public GameObject backButton;
	public Image brokenGlass;
	public Image bloodOnScreen;
	[HideInInspector]public bool changeCameraPossible;
	// bool umożliwiający lawirowanie między kamerami, będziemy go w pewnych momentach blokować.
	private bool onAndOffScreenshotCamera = false;
	// bool ułatwiające zmiane między trybem kamery screenshot, a powrotem do gry pod klawiszem "P".





	// Use this for initialization
	void Start ()
	{

		brokenGlass.enabled = false;
		bloodOnScreen.enabled = false;
		cameraIndex = 0;
		photoCameraView.enabled = false;
		backButton.SetActive (false);
		changeCameraPossible = true;


		for (int i = 1; i < cameras.Length; i++) {
			cameras [i].enabled = false; // tablica kamer. Po indexie wybiera, która z nich ma być aktywna.
		}

		if (cameras.Length > 0) {
			cameras [0].enabled = true; // ustawia pierwszą kamerę jako domyślną.
		}


	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.C) && changeCameraPossible == true) { // po wciśnięciu "C", lawirujemy między kamerami.
			cameraIndex++; // indeksy służą do przypisania kamer.
			if (cameraIndex < cameras.Length) {
				cameras [cameraIndex - 1].enabled = false;
				cameras [cameraIndex].enabled = true;
			} else {
			
				cameras [cameraIndex - 1].enabled = false;
				cameraIndex = 0;
				cameras [cameraIndex].enabled = true;
			}		
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			onAndOffScreenshotCamera = !onAndOffScreenshotCamera;
		}

		if (onAndOffScreenshotCamera == true) {
			PhotoCameraButton ();
		} else {
			BackFromPhotoView ();
		}
	}

	public void PhotoCameraButton ()
	{
		

		Time.timeScale = 0;
		photoCameraView.enabled = true;
		changeCameraPossible = false;
		for (int i = 1; i < cameras.Length; i++) {
			cameras [i].enabled = false; // tablica kamer. Po indexie wybiera, która z nich ma być aktywna.
		}
		backButton.SetActive (true);


	}

	public void BackFromPhotoView ()
	{

		Time.timeScale = 1;
		photoCameraView.enabled = false;
		changeCameraPossible = true;
		if (cameras.Length > 0) {
			cameras [0].enabled = true; // ustawia pierwszą kamerę jako domyślną.
		}
		backButton.SetActive (false);

	}
		


}
