/**
 * Author: Daniel Wilches
 */

using UnityEngine;
using System.Collections;
using System;

/**
 * When creating your message listeners you need to implement these two methods:
 *  - OnMessageArrived
 *  - OnConnectionEvent
 */
public class SampleMessageListener : MonoBehaviour
{
	//tag position
	public static float xpos = 0.0f;
	public static float zpos = 0.0f;
	public static float ypos = 0.0f;

	//tag rotation
	public static float rw = 0.0f;
	public static float rx = 0.0f;
	public static float ry = 0.0f;
	public static float rz = 0.0f;

	//anchor positions
	public static Vector3 anchor1;
	public static Vector3 anchor2;
	public static Vector3 anchor3;
	public static Vector3 anchor4;
	public static Vector3 anchor5;
	public static Vector3 anchor6;

	//anchor to anchor distances
	public float a0a1, a0a2, a0a3, a1a2, a0a4, a0a5;

	//tag to tag distances
	public float t0a0, t0a1, t0a2, t0a3, t0a4, t0a5;

	//linear acceleration
	public float lax, lay, laz;

	string CLASS = "SampleMessageListener: ";

	private const int MAX_MESSAGE_LEN = 185;
	private const int MIN_MESSAGE_LEN = 19; //don't include $ or *
	private const int MAX_MSG_BUFFER_LEN = MAX_MESSAGE_LEN*8+1;
	private const int TAGID_FIELD_LOC = 1;
	private const int COMMAND_FIELD_LOC = 0;
	private const char MSG_START_CHAR = '$';
	private const char MSG_END_CHAR = '*';
	private const char MSG_SPLIT_CHAR = ',';
	private const char NEGATIVE_SIGN = '-';
	private const string DATA_OUTPUT_CMD = "10001";
	private const string ANCHOR_DIST_CMD = "10002";
	private const string TAG_DIST_CMD = "10003";
	private const string LIN_AXCEL_CMD = "10004";
	private const string ANCHOR_POSITION_CMD = "10006";

	//output data locations
	private const int X_FIELD_LOC = 2;
	private const int Y_FIELD_LOC = X_FIELD_LOC+1;
	private const int Z_FIELD_LOC = Y_FIELD_LOC+1;
	private const int RW_FIELD_LOC = Z_FIELD_LOC+1;
	private const int RX_FIELD_LOC = RW_FIELD_LOC+1;
	private const int RY_FIELD_LOC = RX_FIELD_LOC+1;
	private const int RZ_FIELD_LOC = RY_FIELD_LOC+1;

	//anchor distances
	private const int A0A1 = 2;
	private const int A0A2 = A0A1+1;
	private const int A0A3 = A0A2+1;
	private const int A1A2 = A0A3+1;
	private const int A0A4 = A1A2+1;
	private const int A0A5 = A0A4+1;

	//tag distances
	private const int T0A0 = 2;
	private const int T0A1 = T0A0+1;
	private const int T0A2 = T0A1+1;
	private const int T0A3 = T0A2+1;
	private const int T0A4 = T0A3+1;
	private const int T0A5 = T0A4+1;

	//tag linear axcel
	private const int LAX = 2;
	private const int LAY = LAX+1;
	private const int LAZ = LAY+1;
	private const int CAL_STATUS = LAZ+1;

	//anchor positions
	private const int A0X = 2, 		A0Y = A0X+1, 	A0Z = A0Y+1;
	private const int A1X = A0Z+1, 	A1Y = A1X+1, 	A1Z = A1Y+1;
	private const int A2X = A1Z+1, 	A2Y = A2X+1, 	A2Z = A2Y+1;
	private const int A3X = A2Z+1, 	A3Y = A3X+1, 	A3Z = A3Y+1;
	private const int A4X = A3Z+1, 	A4Y = A4X+1, 	A4Z = A4Y+1;
	private const int A5X = A4Z+1, 	A5Y = A5X+1, 	A5Z = A5Y+1;

	private float a0x, a0y, a0z;
	private float a1x, a1y, a1z;
	private float a2x, a2y, a2z;
	private float a3x, a3y, a3z;
	private float a4x, a4y, a4z;
	private float a5x, a5y, a5z;

	//private string RTLSLocation = "Unknown";
	//private string AnchorLocation = "Unknown";
	//private string RTLSMeasurements = "Unknown";
	//private string AnchorDists = "Unknown";



	private string sTagID;

	private static byte[] data_msg = new byte[MAX_MSG_BUFFER_LEN]; //has to be global to remember bytes accumulated so far

    void debugdatabyte() {
        string test = System.Text.Encoding.ASCII.GetString(data_msg);
        print(test);
    }
	private static int idx_bytes = 0; //has to be global to remember number of bytes accumulated so far

	int msg_start = 0; //location in buffer of message start for parsing
	int msg_end = 0; //location in buffer of message end for parsing
	long cnt_rx_messages = 0;
	//long cnt_msg = 0;

	private	long current_time=0, prev_time=0,deltaTime=0;

	public Draw_Tag dt;

	void Start () {
		//startTm = (long)(DateTime.UtcNow - new DateTime (2015, 1, 1)).TotalMilliseconds;
	}

	ASCII_Parse parser = new ASCII_Parse();

	// Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
		Byte[] bytes;
		//current_time = (long)(DateTime.UtcNow - new DateTime (2015, 1, 1)).TotalMilliseconds;
		//deltaTime = current_time - prev_time;
		//prev_time = current_time;
		//Debug.Log (" Delta Time: " + deltaTime);

		if (true) { //msg.Length > MIN_MESSAGE_LEN && msg.StartsWith("$") && msg.EndsWith("*") TODO - CRC Check is needed
			current_time = (long)(DateTime.UtcNow - new DateTime (2015, 1, 1)).TotalMilliseconds;
			deltaTime = current_time - prev_time;
//			dt.updateSpeed = deltaTime;
			prev_time = current_time;
			//Debug.Log("Number of messages: " + cnt_msg);
			//Debug.Log("Message arrived: " + msg);

			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding ();
			bytes = encoding.GetBytes (msg);
			//Debug.Log("First Byte is: " + bytes[0] + " cnt:" + cnt_msg++ + " Delta Time: " + deltaTime);
			fillBuffer(bytes);
		}
		//if(bytes[0]=='$'){

		//}
	}

    // Invoked when a connect/disconnect event occurrs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }

	private void fillBuffer(byte[] data){
		bool EndFound = false;
		if(data.Length<=MAX_MSG_BUFFER_LEN-idx_bytes){ //protect against too much data
			for(int i=0;i<data.Length;i++){
				if(data[i]>='0' && data[i]<='9' || data[i]==MSG_START_CHAR || data[i]==MSG_END_CHAR || data[i]==MSG_SPLIT_CHAR || data[i]==NEGATIVE_SIGN){
					if(data[i]==MSG_END_CHAR) EndFound=true; //* is found at end
					//don't save CRC characters after *; only save $
					if(EndFound && (data[i]==MSG_END_CHAR || data[i]==MSG_START_CHAR) ) {
						data_msg[idx_bytes] = data[i];
						idx_bytes++;
					}
					//end has not been found so save characters
					else if (!EndFound){
						data_msg[idx_bytes] = data[i];
						idx_bytes++;
					}
					//start is found so reset EndFound
					if(data[i]==MSG_START_CHAR) EndFound = false;
				}
			}
		}
		parseData();
	}

	private void parseData(){
		msg_start = 0; msg_end = 0;
		while(idx_bytes-msg_start>=MIN_MESSAGE_LEN){	 //enough data in buffer to parse

			//find msg start character
			for(int i=msg_start;i<data_msg.Length;i++){
				if(data_msg[i]==MSG_START_CHAR) {
					msg_start=i;
					//Debug.Log(CLASS + "Parser, Msg Start = " + msg_start);
					break;
				}
			}

			//find msg end character
			for(int i=msg_start;i<data_msg.Length;i++){
				if(data_msg[i]==MSG_END_CHAR) {
					msg_end=i;
					//Debug.Log(CLASS + "Parser, Msg End = " + msg_end);
					break;
				}
			}
			if(msg_end<=msg_start) break;
			int msg_len = msg_end-msg_start+1;
			//Debug.Log(CLASS + "Parser, Received # bytes = " + msg_len);
			if(msg_len<=MAX_MESSAGE_LEN && msg_len>=MIN_MESSAGE_LEN){
				try{
					parser.parse(data_msg,msg_start,msg_len);
				}
				catch(Exception e){
					Debug.Log(CLASS + "parser Failed");
					Debug.Log(CLASS + "Exception: " + e);
				}
				if(parser.data_parsed && parser.fields[COMMAND_FIELD_LOC].Equals(DATA_OUTPUT_CMD)){
					saveRTLSData();
					cnt_rx_messages++;
				}
				else if(parser.data_parsed && parser.fields[COMMAND_FIELD_LOC].Equals(ANCHOR_DIST_CMD)){
					//saveAnchorDistances();
				}
				else if(parser.data_parsed && parser.fields[COMMAND_FIELD_LOC].Equals(TAG_DIST_CMD)){
					//saveTagDistances();
				}
				else if(parser.data_parsed && parser.fields[COMMAND_FIELD_LOC].Equals(LIN_AXCEL_CMD)){
					//saveLinearAxcel();
				}
				else if( parser.data_parsed && parser.fields[COMMAND_FIELD_LOC].Equals(ANCHOR_POSITION_CMD) ){
					//saveAnchorPositions();
				}
			}
			msg_start += msg_len;
		}

        debugdatabyte();

        //move extra data back to beginning and reset index
        //check for extra data
        if (msg_start>0 && data_msg[msg_start]==MSG_START_CHAR){
			int i = msg_start;
			idx_bytes=0; 
			while(data_msg[i]!='.'){
				data_msg[idx_bytes] = data_msg[i];
				idx_bytes++; i++;
			}
			for(i=idx_bytes;i<data_msg.Length;i++){ //clear rest of buffer
				data_msg[i] = (byte)'.';
			}
		}
		else if(msg_start!=0){
			idx_bytes=0;
			for(int i=0;i<data_msg.Length;i++){ //clear buffer
				data_msg[i] = (byte)'.';
			}
		}

	}

	private void saveRTLSData(){
		string sx="0", sy="0", sz="0", srw="0", srx="0", sry="0", srz="0";
		sTagID = parser.get_field(TAGID_FIELD_LOC);

			sx=parser.get_field(X_FIELD_LOC); sy=parser.get_field(Y_FIELD_LOC); sz=parser.get_field(Z_FIELD_LOC);
			srw=parser.get_field(RW_FIELD_LOC); srx=parser.get_field(RX_FIELD_LOC); sry=parser.get_field(RY_FIELD_LOC); srz=parser.get_field(RZ_FIELD_LOC);

			try{
				xpos = float.Parse (sx)/1000; //raw data comes in with units of MM, need to scale to M
				ypos = float.Parse (sz)/1000; //remap indotraq z to y
				zpos = float.Parse (sy)/1000; //remap indotraq y to z

				rx = float.Parse (sry)/10000; //map and scale IndoTraq y to x
				ry = float.Parse (srz)/10000; //map and scale IndoTraq z to y
				rz = float.Parse (srw)/10000; //map and scale IndoTraq w to z
				rw = float.Parse (srx)/10000; //map and scale IndoTraq x to w

				if( int.Parse(sTagID)==0 ) //tag0 - hmd
					RTLS_PC.instance.MoveToPoint(new Vector3(xpos,ypos,zpos));
				else if( int.Parse(sTagID)==1 ) //tag1 - gun
					RTLS_PC_1.instance.MoveToPoint(new Vector3(xpos,ypos,zpos), new Vector4(rx,ry,rz,rw) );
				//Debug.Log ("Location x=" + xpos + " z=" + ypos + " y=" + zpos + " rx=" + rx + " ry=" + ry + " rz=" + rz + " rw=" + rw);
			}
			catch(Exception e){
				Debug.Log ("Error Parsing RTLSData: " + e);
			}
		
	}

	private void saveAnchorDistances(){
		string str_a0a1, str_a0a2, str_a0a3, str_a1a2, str_a0a4, str_a0a5;
		sTagID = parser.get_field(TAGID_FIELD_LOC);
		str_a0a1=parser.get_field(A0A1);
		str_a0a2=parser.get_field(A0A2);
		str_a0a3=parser.get_field(A0A3);
		str_a1a2=parser.get_field(A1A2);
		str_a0a4=parser.get_field(A0A4);
		str_a0a5=parser.get_field(A0A5);
		try{
			a0a1 = float.Parse(str_a0a1)/1000; //raw data comes in with units of MM, need to scale to M
			a0a2 = float.Parse(str_a0a2)/1000;
			a0a3 = float.Parse(str_a0a3)/1000;
			a1a2 = float.Parse(str_a1a2)/1000;
			a0a4 = float.Parse(str_a0a4)/1000;
			a0a5 = float.Parse(str_a0a5)/1000;
		}
		catch(Exception e){
			Debug.Log(CLASS + "saveAnchorDistances Failed");
			Debug.Log(CLASS + "Exception: " + e);
		}
	}

	private void saveTagDistances(){
		string str_t0a0, str_t0a1, str_t0a2, str_t0a3, str_t0a4, str_t0a5;
		sTagID = parser.get_field(TAGID_FIELD_LOC);
		str_t0a0=parser.get_field(T0A0);
		str_t0a1=parser.get_field(T0A1);
		str_t0a2=parser.get_field(T0A2);
		str_t0a3=parser.get_field(T0A3);
		str_t0a4=parser.get_field(T0A4);
		str_t0a5=parser.get_field(T0A5);
		try{
			t0a0 = float.Parse(str_t0a0)/1000; //raw data comes in with units of MM, need to scale to M
			t0a1 = float.Parse(str_t0a1)/1000;
			t0a2 = float.Parse(str_t0a2)/1000;
			t0a3 = float.Parse(str_t0a3)/1000;
			t0a4 = float.Parse(str_t0a4)/1000;
			t0a5 = float.Parse(str_t0a5)/1000;
		}
		catch(Exception e){
			Debug.Log(CLASS + "saveTagDistances Failed");
			Debug.Log(CLASS + "Exception: " + e);
		}
	}

	void saveLinearAxcel(){
		string str_lax, str_lay, str_laz, str_cal_status;
		sTagID = parser.get_field(TAGID_FIELD_LOC);
		str_lax=parser.get_field(LAX);
		str_lay=parser.get_field(LAY);
		str_laz=parser.get_field(LAZ);
		str_cal_status=parser.get_field(CAL_STATUS);
		try{
			lax = float.Parse(str_lax)/100;
			lay = float.Parse(str_lay)/100;
			laz = float.Parse(str_laz)/100;
		}
		catch(Exception e){
			Debug.Log(CLASS + "saveLinearAxcel Failed");
			Debug.Log(CLASS + "Exception: " + e);
		}
	}

	void saveAnchorPositions(){
		string str_a0x, str_a0y, str_a0z;
		string str_a1x, str_a1y, str_a1z;
		string str_a2x, str_a2y, str_a2z;
		string str_a3x, str_a3y, str_a3z;
		string str_a4x, str_a4y, str_a4z;
		string str_a5x, str_a5y, str_a5z;

		sTagID = parser.get_field(TAGID_FIELD_LOC);
		str_a0x=parser.get_field(A0X); str_a0y=parser.get_field(A0Y); str_a0z=parser.get_field(A0Z);
		str_a1x=parser.get_field(A1X); str_a1y=parser.get_field(A1Y); str_a1z=parser.get_field(A1Z);
		str_a2x=parser.get_field(A2X); str_a2y=parser.get_field(A2Y); str_a2z=parser.get_field(A2Z);
		str_a3x=parser.get_field(A3X); str_a3y=parser.get_field(A3Y); str_a3z=parser.get_field(A3Z);
		str_a4x=parser.get_field(A4X); str_a4y=parser.get_field(A4Y); str_a4z=parser.get_field(A4Z);
		str_a5x=parser.get_field(A5X); str_a5y=parser.get_field(A5Y); str_a5z=parser.get_field(A5Z);

		try{ //anchor data in meters
			anchor1 = new Vector3(float.Parse (str_a0x), float.Parse (str_a0z), float.Parse (str_a0y)); //raw data comes in with units of MM, need to scale to M
			anchor2 = new Vector3(float.Parse (str_a1x), float.Parse (str_a1z), float.Parse (str_a1y)); 
			anchor3 = new Vector3(float.Parse (str_a2x), float.Parse (str_a2z), float.Parse (str_a2y));
			anchor4 = new Vector3(float.Parse (str_a3x), float.Parse (str_a3z), float.Parse (str_a3y));
			anchor5 = new Vector3(float.Parse (str_a4x), float.Parse (str_a4z), float.Parse (str_a4y));
			anchor6 = new Vector3(float.Parse (str_a5x), float.Parse (str_a5z), float.Parse (str_a5y));
//			Debug.Log ("Location A1x=" + anchor1.x + " A1y=" + anchor1.y + " A1z=" + anchor1.z);
//			Debug.Log ("Location A2x=" + anchor2.x + " A2y=" + anchor2.y + " A2z=" + anchor2.z);
//			Debug.Log ("Location A3x=" + anchor3.x + " A3y=" + anchor3.y + " A3z=" + anchor3.z);
//			Debug.Log ("Location A4x=" + anchor4.x + " A4y=" + anchor4.y + " A4z=" + anchor4.z);
//			Debug.Log ("Location A5x=" + anchor5.x + " A5y=" + anchor5.y + " A5z=" + anchor5.z);
//			Debug.Log ("Location A6x=" + anchor6.x + " A6y=" + anchor6.y + " A6z=" + anchor6.z);
		}
		catch(Exception e){
			Debug.Log ("Error Parsing AnchorLocation: " + e);
		}
	}
}
