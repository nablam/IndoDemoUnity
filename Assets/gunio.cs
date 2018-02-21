using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gunio : MonoBehaviour {

	//public Transform gun;
	private Text txt_gunio;

	// Use this for initialization
	void Start () {
		txt_gunio = transform.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		txt_gunio.text = "GUN IO: " + RTLS.tag1_io;
	}
}
