/* This script is used to get x,y,z and rw,rx,ry,rz data from IndoTraq tag1. In this case, Anchor0 is connected direclty to 
 * a PC. The "r" key on the keyboard will reset the tag1 rotation.
*/
using UnityEngine;
using System.Collections;

public class RTLS_PC_1 : MonoBehaviour {

	public Vector3 offset;
	public static RTLS_PC_1 instance;

	public bool testing = true;
	private bool firstReset = false;
	public float x;
	public float y;
	public float z;

	public Quaternion firstQuat,rotationQuaternion,secondQuat;
	private float nx,ny,nz,nw;
	private float nrx,nry,nrz,nrw;
	public float norm_value;

	public void Awake()
	{
		instance = this;
		transform.rotation = new Quaternion (0, 0, 0, 1);
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

	// Use this for initialization
	public  void MoveToPoint(Vector3 pos, Vector4 rot)
	{
		//if (xpos != 0 && ypos != 0 && zpos != 0) {
		Vector3 targetPosition = new Vector3 (pos.x + offset.x, pos.y, pos.z + offset.z);
		//Debug.Log ("Location_1= posiX=" + targetPosition.x + " posiY=" + targetPosition.y + " posiZ=" + targetPosition.z);
		transform.position = targetPosition;
		//}

		firstQuat = new Quaternion (rot.x, rot.y, rot.z, rot.w);

		bool value = Input.GetKey ("r");

		if ( value )
		{
			ResetOrientation(rot);
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
}
