using UnityEngine;
using System.Collections;
using System.Text;

public class ASCII_Parse : MonoBehaviour {

	//int n = 0;
	//$,Tag ID,Command ID,X,Y,Z,RW,RX,RY,RZ,*
	public string[] fields; 
	StringBuilder sb;
	int comma_counter;
	int MAX_FIELDS = 25;
	int field_data_len = 0;
	public bool data_parsed = false;
	string CLASS = "ASCII_Parse: ";
		
	public void parse(byte[] data, int msg_start, int len){
		for(int i=msg_start;i<msg_start+len;i++)
		{
			switch( (char)data[i] )
			{
			case '$':
				comma_counter = 0;
				fields = new string[]{"","","","","","","","","","","","","","","","","","","","","","","","",""};
				break;
			case ',':
				if(comma_counter>=1 && comma_counter<=MAX_FIELDS){
					fields[comma_counter-1] = sb.ToString();
					field_data_len=0;
				}
				else if(comma_counter>MAX_FIELDS) break;
				sb  = new StringBuilder(20);
				comma_counter++;
				break;
			case '*':
				data_parsed = true;
				//Debug.Log(CLASS + "Fields = " + fields[0] + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4] + "," + fields[5] + "," + fields[6] + "," + fields[7] + "," + fields[8] + "," + fields[9] + "," + fields[10] + "," + fields[11] + "," + fields[12] + "," + fields[13] + "," + fields[14] + "," + fields[15] + "," + fields[16] + "," + fields[17] + "," + fields[18] + "," + fields[19] + "," + fields[20] + "," + fields[21] + "," + fields[22] + "," + fields[23] + "," + fields[24]);
				break;
			default:
				if ( comma_counter<=MAX_FIELDS && comma_counter >=1 )
					field_data_len++;
				if(field_data_len>=8)
					Debug.Log(CLASS + "Parse Field Len>=8: " + sb.ToString() + "+" + (char)data[i]); 
				sb.Append((char)data[i]);

				break;
			}
		}
	}

	public string get_field(int field_idx) {

		return fields[field_idx];
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
