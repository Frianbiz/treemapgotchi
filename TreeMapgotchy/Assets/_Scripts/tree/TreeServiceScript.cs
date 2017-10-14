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

//	public GeographicTransform coordinateFrame;
//	public Transform box;
	public bool initiated = false;

	public GameObject treeBad;
	public GameObject treeMiddle;
	public GameObject treeGood;

//
//	// Use this for initialization
//	void Start () {
//		
//	}
//
//	void OnEnable()
//	{
//		if (coordinateFrame != null) {
//			Debug.Log ("coordinateFrame is not null ");
//		}
//
//		if (Api.Instance != null) {
//			Debug.Log ("Instance is not null " + Api.Instance.ToString ());
//		} else {
//			Debug.Log ("Instance is null ");
//		}
//
//		if (Api.Instance.GeographicApi != null) {
//			Debug.Log ("GeographicApi is not null ");
//		}
//
//	}
//	
//	// Update is called once per frame
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
					// System.Console.WriteLine(element);
				}
				// StartCoroutine(Example());
//				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//				cube.AddComponent<Rigidbody>();
//				cube.transform.position = new Vector3(0.0f, 40.0f, 0);
//				box = cube.transform;
			} else {
				Debug.Log ("Instance is null ");
			}
			initiated = true;
		}
	}
//
//	IEnumerator Example()
//	{
//		Api.Instance.CameraApi.MoveTo(pointA, distanceFromInterest: 1000, headingDegrees: 0, tiltDegrees: 45);
//
//		while (true)
//		{
//			yield return new WaitForSeconds(2.0f);
//			calculateAltitude(pointA);
//			coordinateFrame.SetPosition(pointA);
//			yield return new WaitForSeconds(2.0f);
//			calculateAltitude(pointB);
//			coordinateFrame.SetPosition(pointB);
//		}
//	}

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
		// box.transform.p.addChild(representation);
		// boxAnchor.AddComponent<GeographicTransform> ();
		Api.Instance.GeographicApi.RegisterGeographicTransform(box.GetComponent<GeographicTransform>());
		box.GetComponent<GeographicTransform>().SetPosition(boxLocation);
//		var box = boxAnchor.transform.GetChild (0);
//		// box.localPosition = new Vector3 (0.0f, , 0.0f);
//		// Destroy (boxAnchor, 2.0f);
//		 var box = boxAnchor.transform;
		representation.transform.localPosition = new Vector3 (0.0f, 150.0f, 0.0f);
		representation.transform.localScale = new Vector3 (10.0f, 10.0f, 10.0f);

	}


//	void calculateAltitude(LatLong latLong){
//		double altitude;
//		var success = Api.Instance.BuildingsApi.TryGetAltitudeAtLocation(latLong, out altitude);
//		if (success) {
//			Debug.Log ("success " + altitude);
//			//			var boxLocation = LatLong.FromDegrees (latLong.GetLatitude (), latLong.GetLongitude ());
//			//			var boxAnchor = Instantiate (boxPrefab) as GameObject;
//			//			boxAnchor.GetComponent<GeographicTransform> ().SetPosition (boxLocation);
//
//			// var box = boxAnchor.transform.GetChild (0);
//			// box.localPosition = new Vector3 (0.0f, , 0.0f);
//			// Destroy (boxAnchor, 2.0f);
//			box.localPosition = new Vector3(0.0f, (float)altitude, 0.0f);
//		} else {
//			// Debug.Log ("error ");
//			box.localPosition = new Vector3(0.0f, 150.0f, 0.0f);
//		}
//	}


//	void MakeBox(LatLong latLong)
//	{
//		double altitude;
//		var success = Api.Instance.BuildingsApi.TryGetAltitudeAtLocation(latLong, out altitude);
//		if (success)
//		{
//			var boxLocation = LatLong.FromDegrees(latLong.GetLatitude(), latLong.GetLongitude());
//			var boxAnchor = Instantiate(boxPrefab) as GameObject;
//			boxAnchor.GetComponent<GeographicTransform>().SetPosition(boxLocation);
//
//			var box = boxAnchor.transform.GetChild(0);
//			box.localPosition = new Vector3(0.0f, (float)altitude, 0.0f);
//			Destroy(boxAnchor, 2.0f);
//		}
//	}

//
//	private void OnDisable()
//	{
//		StopAllCoroutines();
//		Api.Instance.GeographicApi.UnregisterGeographicTransform(coordinateFrame);
//	}
}