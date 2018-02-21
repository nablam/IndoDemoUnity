/* This script is used to get x,y,z data from IndoTraq tag0. In this case, Anchor0 is connected direclty to 
 * a PC.
*/

using UnityEngine;
using System.Collections;

public class RTLS_PC : MonoBehaviour {


	public Vector3 offset;
	public static RTLS_PC instance;

	public void Awake()
	{
		instance = this;
	
	}
	// Use this for initialization
	public  void MoveToPoint(Vector3 pos)
	{
		//if (xpos != 0 && ypos != 0 && zpos != 0) {
			Vector3 targetPosition = new Vector3 (pos.x + offset.x, pos.y, pos.z + offset.z);
			Debug.Log ("Location= posiX=" + targetPosition.x + " posiY=" + targetPosition.y + " posiZ=" + targetPosition.z);
			transform.position = targetPosition;
		//}


	}
}
