/* This script is used to get data from IndoTraq tag that is connected to an Android device.
 * example: IndoTraq tag on head and connected to Gear VR mask for VR Demo. The data is being
 * fed into the Android phone which is running the Unity VR app.
 * xpos,ypos,zpos is the tag0 on the head and controls character's position
 * With firmware that enables tag to “see” other tag coordinates then tag0 can also transmit
 * tracking data for tag1. This data is associated with tag1_xxx and is obtained via the getRTLS.cs 
 * script
*/

using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class RTLS : MonoBehaviour {

	private		StreamWriter	OutputStream;
	private		string			LogFile = "/sdcard/logAX.dat";
	public		bool			DATALOGGING = false;

	public GameObject anchor1; //used to track location of anchor1
	public GameObject anchor2; //used to track location of anchor2
	public GameObject anchor3; //used to track location of anchor3

	public		static bool		codeGunFired = false; //modified in Shooting.cs to track when gun is fired for data logging

	public Vector3 offset;
	// * * * * * * * * * * * * *
	// RTLSManager declarations
	AndroidJavaObject ActivityObject; //use for non-static calls
	AndroidJavaClass ActivityClass; //use for non-static calls
	// * * * * * * * * * * * * *

	private string locMessage = "";
	private string anchorMessage = "";
	private bool anchorLocationFound = false;
	
	private float xpos = 0.0f;
	private float zpos = 0.0f;
	private float ypos = 0.0f;

	public static float tag1_xpos = 0.0f;
	public static float tag1_ypos = 0.0f;
	public static float tag1_zpos = 0.0f;
	public static float tag1_rw = 0.0f;
	public static float tag1_rx = 0.0f;
	public static float tag1_ry = 0.0f;
	public static float tag1_rz = 0.0f;
	public static int tag1_io = 0;

	void Start () {

		if (DATALOGGING) {
			// Open the log file to append the new log to it.
			OutputStream = new StreamWriter (LogFile, false);
		}
		
		// * * * * * * * * * * * * *
		// RTLSManager declarations (plugin)
		AndroidJNI.AttachCurrentThread();
		ActivityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		ActivityObject = ActivityClass.GetStatic<AndroidJavaObject>("currentActivity");
		//********************************
	}

	void FixedUpdate () {

		getNewXYZ ();
		getTag1 ();
		MoveToPoint ();

		if (DATALOGGING) {
			//log data to file
			Vector3 positions = transform.position;
			string message = positions.x + "," + positions.y + "," + positions.z;

			long millis = (long)(DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalMilliseconds;
			message = millis + "," + message;

			if (OutputStream != null) {
				OutputStream.WriteLine (message);
				OutputStream.Flush ();
			} else
				Debug.Log ("Error: Output stream not created!");
		}
	}


			
	public virtual void getNewXYZ() {

		char[] delimiterChars = { '=',' ' };
		string[] dist;
		locMessage = ActivityObject.Call<string> ("getRTLSUpdate");
		//RTLSLocation = "tagid=" + tagid + " x=" + x + " y=" + y + " z=" + z + " rw=" + rw + " rx=" + rx + " ry=" + ry + " rz=" + rz;
		Debug.Log ("Location= " + locMessage);
		if(locMessage != "Unknown") {
			dist = locMessage.Split(delimiterChars);
			try{
				xpos = float.Parse (dist[3])/1000; //raw data comes in with units of MM, need to scale to M
				zpos = float.Parse (dist[5])/1000;
				ypos = float.Parse (dist[7])/1000;
				Debug.Log ("Location x=" + xpos + " y=" + ypos + " z=" + zpos);
			}
			catch(Exception e){
				Debug.Log ("Error Parsing RTLSLocation: " + e);
			}
		}

		if(!anchorLocationFound){
			anchorMessage = ActivityObject.Call<string> ("getAnchorLocation");
			Debug.Log ("Anchor Location= " + anchorMessage);
			if(anchorMessage != "Unknown") {
				dist = anchorMessage.Split(delimiterChars);
				//dist = {tagid=,tagid,x=,x,y=,y,z=,z,rw=,rw,rx=,rx,ry=,ry,rz=,rz,io1=,tagio1}
				try{ //anchor data in meters
					anchor1.transform.position = new Vector3(float.Parse (dist[1]), anchor2.transform.position.y, float.Parse (dist[3])); //raw data comes in with units of MM, need to scale to M
					anchor2.transform.position = new Vector3(float.Parse (dist[7]), anchor2.transform.position.y, float.Parse (dist[9])); //raw data comes in with units of MM, need to scale to M
					anchor3.transform.position = new Vector3(float.Parse (dist[13]), anchor3.transform.position.y, float.Parse (dist[15]));
					Debug.Log ("Location A1x=" + anchor1.transform.position.x + " A1y=" + anchor1.transform.position.y + " A1z=" + anchor1.transform.position.z);
					Debug.Log ("Location A2x=" + anchor2.transform.position.x + " A2y=" + anchor2.transform.position.y + " A2z=" + anchor2.transform.position.z);
					Debug.Log ("Location A3x=" + anchor3.transform.position.x + " A3y=" + anchor3.transform.position.y + " A3z=" + anchor3.transform.position.z);
					anchorLocationFound=true;
				}
				catch(Exception e){
					Debug.Log ("Error Parsing AnchorLocation: " + e);
				}
			}
		}
		
	}

	public virtual void getTag1() {

		char[] delimiterChars = { '=',' ' };
		string[] dist;
		locMessage = ActivityObject.Call<string> ("getRTLSUpdate_1");
		//RTLSLocation = "tagid=" + tagid + " x=" + x + " y=" + y + " z=" + z + " rw=" + rw + " rx=" + rx + " ry=" + ry + " rz=" + rz + " io1=" + tagio1;
		//Debug.Log ("Location= " + locMessage);
		if(locMessage != "Unknown") {
			dist = locMessage.Split(delimiterChars);
			try{
				tag1_xpos = float.Parse (dist[3])/1000; //raw data comes in with units of MM, need to scale to M
				tag1_zpos = float.Parse (dist[5])/1000;
				tag1_ypos = float.Parse (dist[7])/1000;

				tag1_rw = float.Parse (dist[11])/10000;
				tag1_rx = float.Parse (dist[13])/10000;
				tag1_ry = float.Parse (dist[15])/10000;
				tag1_rz = float.Parse (dist[9])/10000;

				tag1_io = int.Parse (dist[17]);
				//Debug.Log ("Location x=" + xpos + " y=" + ypos + " z=" + zpos);
			}
			catch(Exception e){
				Debug.Log ("Error Parsing RTLSLocation_1: " + e);
			}
		}
	}
		

	void OnDestory()
	{
		if (DATALOGGING) {
			if (OutputStream != null) {
				OutputStream.Close ();
				OutputStream = null;
			}
		}
	}

	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus)
			Debug.Log ("Application Paused");
		else
			Debug.Log ("Application Resumed");
	}

	public virtual void MoveToPoint()
	{
		if (xpos != 0 && ypos != 0 && zpos != 0) {
			Vector3 targetPosition = new Vector3 (xpos + offset.x, ypos, zpos + offset.z);
			Debug.Log ("Location= posiX=" + targetPosition.x + " posiY=" + targetPosition.y + " posiZ=" + targetPosition.z);
			transform.position = targetPosition;
		}
		return;

	}


}

