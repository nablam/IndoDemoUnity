
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

[System.Serializable]
public static class TestFlightSDK 
{
#if TESTFLIGHT_SDK_ENABLED && UNITY_IPHONE && (!UNITY_EDITOR)
	[DllImport ("__Internal")]
	private static extern void TestFlight_SubmitFeedback(string feedbackString);
	public static void SubmitFeedback(string feedbackString)
	{
		TestFlight_SubmitFeedback(feedbackString);
	}

	[DllImport ("__Internal")]
	private static extern void TestFlight_PassCheckpoint(string checkpointName);
	public static void PassCheckpoint(string checkpointName) 
	{
		TestFlight_PassCheckpoint(checkpointName);
	}	
	
	[DllImport ("__Internal")]
	private static extern void TestFlight_AddCustomEnvironmentInformation(string information, string forKey);
	public static void AddCustomEnvironmentInformation(string information, string forKey) 
	{
		TestFlight_AddCustomEnvironmentInformation(information, forKey);
	}

	[DllImport ("__Internal")]
	private static extern void TestFlight_Log(string text);
	public static void Log(string text) 
	{
		TestFlight_Log(text);
	}
#else
	public static void PassCheckpoint(string checkpointName) {}
	public static void AddCustomEnvironmentInformation(string information, string forKey) {}
	public static void SubmitFeedback(string feedbackString) {}
	public static void Log(string text) {} 
#endif
	public static void OpenFeedbackView() { UnityEngine.Debug.LogError("OpenFeedbackView is no longer supported, use SubmitFeedback(string) instead!"); }
}