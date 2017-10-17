using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrld;
using Wrld.Space;


public class TreeServiceScript : MonoBehaviour 
{

	 // Use this for initialization
		void Start () {
			
		}

	static LatLong pointA = LatLong.FromDegrees(48.6887105, 2.2077153);
	static LatLong pointB = LatLong.FromDegrees(48.6881722, 2.2088526);

	public bool initiated = false;

	public GameObject treeBad;
	public GameObject treeMiddle;
	public GameObject treeGood;

	// Update is called once per frame
	void Update () {
		if (!initiated && HttpServiceScript.list != null) {
			if (Api.Instance != null) {
				Debug.Log ("Initiate datas ");
				// Api.Instance.GeographicApi.RegisterGeographicTransform(coordinateFrame);
				Debug.Log ("userList " + HttpServiceScript.list.result.Count);
				foreach (User currentUser in HttpServiceScript.list.result)
				{
					Debug.Log ("generateTree " + currentUser.name + " " + currentUser.id  + " " + currentUser.average_impact + " " + currentUser.latitude + " " + currentUser.longitude);
					generateTree (currentUser);
				}
			} else {
				Debug.Log ("Instance is null ");
			}
			initiated = true;
		}
	}

	void generateTree(User user){
		var boxLocation = LatLong.FromDegrees (user.latitude, user.longitude);

		GameObject box = new GameObject ();
		box.AddComponent<GeographicTransform> ();
		GameObject representation;
		if (user.average_impact <= 170f) {
			representation= Instantiate (treeGood) as GameObject;	
		} else if (user.average_impact >= 480f) {
			representation= Instantiate (treeBad) as GameObject;	
		} else {
			representation= Instantiate (treeMiddle) as GameObject;	
		}
		representation.transform.SetParent(box.transform);
		Api.Instance.GeographicApi.RegisterGeographicTransform(box.GetComponent<GeographicTransform>());
		box.GetComponent<GeographicTransform>().SetPosition(boxLocation);

		representation.transform.localPosition = new Vector3 (0.0f, 150.0f, 0.0f);
		representation.transform.localScale = new Vector3 (10.0f, 10.0f, 10.0f);
	}
}