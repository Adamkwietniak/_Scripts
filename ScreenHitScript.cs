using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class ScreenHitScript : MonoBehaviour
{
	
	public GameObject[] buttonsToHide;
	private int screenshotCount = 0;
	// ;iczba, która pojawia się po nazwie screenshot


	/*private string folderPath = Application.dataPath + "/screenshots/";
	private string fileName;*/

	void Start ()
	{

	}
	// Update is called once per frame
	void Update ()
	{

		/*if (!System.IO.Directory.Exists (folderPath)) 
		{
			System.IO.Directory.CreateDirectory (folderPath);
		}*/
		if (Input.GetKeyDown (KeyCode.F9)) { // jak przyciskamy F9 to robi screena
			
			for (int i = 1; i < buttonsToHide.Length; i++) {
				buttonsToHide [i].gameObject.SetActive (false);
			}

			string screenshotFilename;
			do {
				screenshotCount++;
				string path = Environment.GetFolderPath (Environment.SpecialFolder.MyPictures) + "/Dev4Play/Base Jump/";
				screenshotFilename = "screenshot" + screenshotCount + ".png";

				//FileStream plik = File.Create(Application.persistentDataPath + "/profile.data");
				//Debug.Log (Application.persistentDataPath + "/profile.data");

				//string SaveScreen = System.Environment.ExpandEnvironmentVariables("%USERPROFILE%\\Screenshots"); // chyba kurwa działa :P
				//System.IO.File.WriteAllBytes(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "%USERPROFILE%\\Screenshot.png");
			} while (System.IO.File.Exists (screenshotFilename));
			Application.CaptureScreenshot (screenshotFilename);
			for (int i = 1; i < buttonsToHide.Length; i++) {

				buttonsToHide [i].gameObject.SetActive (true);

			}
		}

	}
}


