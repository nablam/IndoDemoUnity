/* This script does nothing. It is just used to describe this project.
 * 
 * This project is used to demostrate getting IndoTraq data from either Anchor0 via a PC or
 * from Tag0 via Android device.
 * 
 * Anchor0 / PC
 * 1. Enable everything with -PC and disable everything with -Android.
 * 2. Under SerialInput...SerialController, change Port Name to match Anchor0's comm port
 * 3. Press play in editor.
 * 4. You will see tag0 moving the character Ethan and tag1 moving the gun.
 * 5. The gun orientation can be reset with "r" on the keyboard, provided you have the Game view selected.
 * 
 * Tag0 / Android
 * 1. Enable everything with -Android and disable everything with -PC.
 * 2. Change build settings for Android.
 * 3. Configure player settings as needed. For example app name and id
 * 4. Compile to Android device.
 * 5. Connect tag0 to Android device and turn it on.
 * 6. Accept usb permission (if needed).
 * 7. Click on Reset Orientation button on screen to demonstrate how to reset the orientation of the gun.
 * 8. In your application, be sure to include the latest IndoTraq plugin in plugins/Android. At the time of this 
 * writting, rtlsupdater6lib.jar was the latest.
 */

using UnityEngine;
using System.Collections;

public class README : MonoBehaviour {
}
