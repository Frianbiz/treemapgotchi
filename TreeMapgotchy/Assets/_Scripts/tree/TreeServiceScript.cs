using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Wrld;
//using Wrld.Space;


public class TreeServiceScript : MonoBehaviour 
{

	 // Use this for initialization
		void Start () {
			
		}

	// Update is called once per frame
		void Update () {
			
		}
//	static LatLong pointA = LatLong.FromDegrees(37.783372, -122.400834);
//	static LatLong pointB = LatLong.FromDegrees(37.784560, -122.402092);
//
//	public GeographicTransform coordinateFrame;
//	public Transform box;
//
//	// Use this for initialization
//	void Start () {
//		
//	}
//
//	void OnEnable()
//	{
//		Api.Instance.GeographicApi.RegisterGeographicTransform(coordinateFrame);
//		StartCoroutine(Example());
//		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//		cube.AddComponent<Rigidbody>();
//		cube.transform.position = new Vector3(0.0f, 40.0f, 0);
//		box = cube.transform;
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//
//	IEnumerator Example()
//	{
//		Api.Instance.CameraApi.MoveTo(pointA, distanceFromInterest: 1000, headingDegrees: 0, tiltDegrees: 45);
//		box.localPosition = new Vector3(0.0f, 40.0f, 0.0f);
//
//		while (true)
//		{
//			yield return new WaitForSeconds(2.0f);
//			coordinateFrame.SetPosition(pointA);
//			yield return new WaitForSeconds(2.0f);
//			coordinateFrame.SetPosition(pointB);
//		}
//	}
//
//	private void OnDisable()
//	{
//		StopAllCoroutines();
//		Api.Instance.GeographicApi.UnregisterGeographicTransform(coordinateFrame);
//	}
}