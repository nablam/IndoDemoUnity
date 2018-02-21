using UnityEngine;
using System.Collections;

public class Draw_Tag : MonoBehaviour {

	private bool hasBeenReset = false;
	float x;
	float y;
	float z;

	Quaternion firstQuat,rotationQuaternion,secondQuat;
	private float nx,ny,nz,nw;
	private float nrx,nry,nrz,nrw;
	float norm_value;

	public float updateSpeed = 0;

	void Start ()
	{
		transform.rotation = new Quaternion (0, 0, 0, 1);
	}

	void Update ()
	{
		transform.position = new Vector3 (SampleMessageListener.xpos, SampleMessageListener.ypos, SampleMessageListener.zpos);
		firstQuat = new Quaternion (SampleMessageListener.rx, SampleMessageListener.ry, SampleMessageListener.rz,  SampleMessageListener.rw);

			if (hasBeenReset) {
				//apply offset rotation
				secondQuat = firstQuat * rotationQuaternion;
				//convert right hand coordinates to left hand coordinates
				transform.rotation = new Quaternion(secondQuat.x, -secondQuat.y, secondQuat.z, secondQuat.w);
			}
			else
				transform.rotation = Quaternion.Inverse(firstQuat);
	}

	public void ResetOrientation()
	{
		hasBeenReset = true;
		//calculate norm of the quaternion
		norm_value = Mathf.Sqrt (Mathf.Pow(SampleMessageListener.rx,2)+Mathf.Pow(SampleMessageListener.ry,2)+Mathf.Pow(SampleMessageListener.rz,2)+Mathf.Pow(SampleMessageListener.rw,2));
		if (norm_value > 0) {
			//convert quaternion to unit quaternion by normalization
			nx = SampleMessageListener.rx / norm_value;
			ny = SampleMessageListener.ry / norm_value;
			nz = SampleMessageListener.rz / norm_value;
			nw = SampleMessageListener.rw / norm_value;
		}
		//calculate offset rotation to bring position back to 0,0,0,1
		rotationQuaternion = new Quaternion (-nx,-ny,-nz,nw);
	}
}
