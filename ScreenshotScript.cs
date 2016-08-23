using UnityEngine;
using System.Collections;
using System;

public class ScreenshotScript : MonoBehaviour {

	public RenderTexture overviewTexture;
	GameObject OVcamera;
	public string path = "";


	void Start () 
	{
		OVcamera = GameObject.FindGameObjectWithTag ("PhotoCamera");
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{

		if (Input.GetKeyDown (KeyCode.F9)) 
		{
			StartCoroutine (TakeScreenShot());
		}
	}

	string fileName (int width, int height)
	{
		return string.Format ("Screen_{0}x{1}_{2}.png", width, height, System.DateTime.Now.ToString ("yy-MM-dd_HH-mm-ss"));
	}

	public IEnumerator TakeScreenShot () 
	{
		yield return new WaitForEndOfFrame ();
		Camera camOV = OVcamera.GetComponent<Camera>();
		RenderTexture currentRT = RenderTexture.active;
		RenderTexture.active = camOV.targetTexture;
		camOV.Render ();
		Texture2D imageOverview = new Texture2D (camOV.targetTexture.width, camOV.targetTexture.height, TextureFormat.RGB24, false);
		imageOverview.ReadPixels (new Rect (0, 0, camOV.targetTexture.width, camOV.targetTexture.height), 0, 0);
		imageOverview.Apply ();
		RenderTexture.active = currentRT;

		byte[] bytes = imageOverview.EncodeToPNG (); // convert texture to PNG

		string filename = fileName (Convert.ToInt32 (imageOverview.width), Convert.ToInt32 (imageOverview.height));  // save file on the drive
		path = Application.persistentDataPath + "/Screenshots/" + filename;

		System.IO.File.WriteAllBytes (path, bytes);
	}
}
