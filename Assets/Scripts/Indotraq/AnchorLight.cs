using UnityEngine;
using System.Collections;

public class AnchorLight : MonoBehaviour {
	
	Component halo; 

	// Use this for initialization
	void Start () {
		halo = GetComponent("Halo");
	}

	void OnTriggerEnter(Collider col){
		halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
	}
	
	void OnTriggerExit(Collider col){
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
	}
}
