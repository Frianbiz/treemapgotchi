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

	public IEnumerator getDatas(){
		Debug.Log("dans le store");

		// Create a download object
		WWW download = new WWW( api_url);

		// Wait until the download is done
		yield return download;

		Debug.Log("after return download");

		if(!string.IsNullOrEmpty(download.error)) {
			Debug.Log(download.error);
		} else {
			Debug.Log("result" + download.text);
			list = JsonUtility.FromJson<UserList> (download.text);
		}
	}
}
	
// sample
//{
//	"id":1,
//	"name":"gro",
//	"latitude":"48.68338749999999",
//	"longitude":"2.2056852000000617",
//	"number_scan":8,
//	"average_impact":"208.750000"
//}

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
