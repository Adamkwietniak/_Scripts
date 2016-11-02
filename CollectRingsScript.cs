using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CollectRingsScript : MonoBehaviour
{

	[HideInInspector]public int i = 0;
	public int SetAmountOfTriggers = 2;
	// tablica colliderów
	public GameObject[] trigger = new GameObject[1];
	// tablica obiektów
	//[HideInInspector]public int numberOfRingsCollected;
	// liczba zebranych kółeczek
	public Text displayCollectedRings;
	// tekst pokazujący nasz wynik
	public int maximumRings;



	// Use this for initialization
	void Start ()
	{
		//numberOfRingsCollected = 0;
		for (int z = 0; z == SetAmountOfTriggers; z++) {
			trigger [z] = GameObject.FindGameObjectWithTag ("Trigger");
		}
		ChangeTrigger (i);
	}

	void Update ()
	{
		//displayCollectedRings.text = "Rings collected: " + numberOfRingsCollected + "/" + maximumRings;
	}


	void OnTriggerEnter (Collider other)
	{

		if (other.tag == "Trigger") {
			if (Missions (i) == true) {
				i++;
				//numberOfRingsCollected++;
				ChangeTrigger (i);

			}
		}
			
	}

	void ChangeTrigger (int i)
	{
		for (int z = 0; z < SetAmountOfTriggers; z++) {
			if (i == z)
				trigger [z].SetActive (true);
			else
				trigger [z].SetActive (false);
				
		}
	}

	bool Missions (int i)
	{
		switch (i) {

		default:
			return true;
		
		}
		return false;
	}
}
