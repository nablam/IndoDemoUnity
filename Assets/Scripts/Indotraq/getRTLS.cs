/* This script is used to get data from IndoTraq tag that is connected to an object such as
 * a gun. See RTLS.cs for more details.
*/

using UnityEngine;
using System.Collections;

public class getRTLS : MonoBehaviour {

	public bool testing = true;
	private bool firstReset = false;
	public float x;
	public float y;
	public float z;

	public Quaternion firstQuat,rotationQuaternion,secondQuat;
	private float nx,ny,nz,nw;
	private float nrx,nry,nrz,nrw;
	public float norm_value;
	private bool resetOrient = false;

	public Vector3 offset;

	// Use this for initialization
	void Start () {
		transform.rotation = new Quaternion (0, 0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {

		//if (xpos != 0 && ypos != 0 && zpos != 0) {
		Vector3 targetPosition = new Vector3 (RTLS.tag1_xpos + offset.x, RTLS.tag1_ypos, RTLS.tag1_zpos + offset.z);
		//Debug.Log ("Location_1= posiX=" + targetPosition.x + " posiY=" + targetPosition.y + " posiZ=" + targetPosition.z);
		transform.position = targetPosition;
		//}

		firstQuat = new Quaternion (RTLS.tag1_rx, RTLS.tag1_ry, RTLS.tag1_rz, RTLS.tag1_rw);


		if ( Input.GetKeyDown(KeyCode.Escape) || resetOrient )
		{
			resetOrient = false;
			ResetOrientation( new Vector4(RTLS.tag1_rx, RTLS.tag1_ry, RTLS.tag1_rz, RTLS.tag1_rw) );

		}

		if (firstReset) {
			//apply offset rotation
			secondQuat = firstQuat * rotationQuaternion;
			//convert right hand coordinates to left hand coordinates
			transform.rotation = new Quaternion(secondQuat.x, -secondQuat.y, secondQuat.z, secondQuat.w);
		}
		else
			transform.rotation = Quaternion.Inverse(firstQuat);

	}

	public void ResetOrientation(Vector4 rot)
	{
		firstReset = true;
		//calculate norm of the quaternion
		norm_value = Mathf.Sqrt (Mathf.Pow(rot.x,2)+Mathf.Pow(rot.y,2)+Mathf.Pow(rot.z,2)+Mathf.Pow(rot.w,2));
		if (norm_value > 0) {
			//convert quaternion to unit quaternion by normalization
			nx = rot.x / norm_value;
			ny = rot.y / norm_value;
			nz = rot.z / norm_value;
			nw = rot.w / norm_value;
		}
		//calculate offset rotation to bring position back to 0,0,0,1
		rotationQuaternion = new Quaternion (-nx,-ny,-nz,nw);
	}

	public void changResetOrient(){
		resetOrient = true;
	}
}
