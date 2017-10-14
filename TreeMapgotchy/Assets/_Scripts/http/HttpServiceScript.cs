using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HttpServiceScript : MonoBehaviour {

	string api_url = "http://api.treemagotchi.frianbiz.com/api/users";

	public static UserList list = null;

	// Use this for initialization
	void Start () {
		StartCoroutine(getDatas());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator getDatas(){
		Debug.Log("dans le store");

		// Create a form object for sending high score data to the server
		// WWWForm form = new WWWForm();

		// Create a download object
		WWW download = new WWW( api_url);

		// Debug.Log("after create download");

		// Wait until the download is done
		yield return download;

		Debug.Log("after return download");
//		if (download.responseHeaders.Count > 0)
//		{
//			foreach (KeyValuePair<string, string> entry in download.responseHeaders)
//			{
//				Debug.Log(entry.Value + "=" + entry.Key);
//			}
//		}

		if(!string.IsNullOrEmpty(download.error)) {
			Debug.Log(download.error);
			// print( "Error downloading: " + download.error );
		} else {
			Debug.Log("result" + download.text);
			// string data = "{/""list":"+download.text+"}";
			// string data = "[{"id":1,"name":"gro","latitude":"48.68338749999999","longitude":"2.2056852000000617","number_scan":8,"average_impact":"208.750000"}];
			// Debug.Log("datas" + data);
			// User user = JsonUtility.FromJson<User> (download.text);
			list = JsonUtility.FromJson<UserList> (download.text);

			// show the highscores
			// Debug.Log("User " + list.ToString());
//			Debug.Log("result" + list.ToString());

//			User user = JsonUtility.FromJson<UserList> ("{list:"+download.text+"}");
//			// show the highscores
//			Debug.Log("result" + list.ToString());
		}
	}
}

[Serializable]
public class User
{
	public int id;
	public string name;
	public float latitude;
	public float longitude;
	public int number_scan;
	public float average_impact;
}

[Serializable]
public class UserList
{
	public List<User> result;
}

//{
//	"id":1,
//	"name":"gro",
//	"latitude":"48.68338749999999",
//	"longitude":"2.2056852000000617",
//	"number_scan":8,
//	"average_impact":"208.750000"
//}