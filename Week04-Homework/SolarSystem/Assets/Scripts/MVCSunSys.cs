using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVCSunSys : MonoBehaviour {
	public Transform sun;
	public Transform Mercury;
	public Transform Venus;
	public Transform Earth;
	public Transform moon;
	public Transform Mars;
	public Transform Jupiter;
	public Transform Saturn;
    public Transform Uranus;
    public Transform Neptune;
    Vector3 axisMercury;
    Vector3 axisVenus;
    Vector3 axisEarth;
    Vector3 axisMars;
    Vector3 axisJupiter;
    Vector3 axisSaturn;
    Vector3 axisUranus;
    Vector3 axisNeptune;
	// Use this for initialization
	void Start () {
		sun.position = Vector3.zero;
        Mercury.position = new Vector3 (6, 0, 0);
        Venus.position = new Vector3 (8, 0, 0);
        Earth.position = new Vector3 (10, 0, 0);
        moon.position = new Vector3 (11, 0, 0);
        Mars.position = new Vector3 (15, 0, 0);
        Jupiter.position = new Vector3 (18, 0, 0);
        Saturn.position = new Vector3 (23, 0, 0);
        Uranus.position = new Vector3 (27, 0, 0);
        Neptune.position = new Vector3 (32, 0, 0);

        axisMercury = new Vector3 (0, Random.Range(0, 100), Random.Range(0, 100));
	    axisVenus = new Vector3 (0, Random.Range(0, 100), Random.Range(0, 100));
	    axisEarth = new Vector3 (0, Random.Range(0, 100), Random.Range(0, 100));
	    axisMars = new Vector3 (0, Random.Range(0, 100), Random.Range(0, 100));
	    axisJupiter = new Vector3 (0, Random.Range(0, 100), Random.Range(0, 100));
	    axisSaturn = new Vector3 (0, Random.Range(0, 100), Random.Range(0, 100));
	    axisUranus = new Vector3 (0, Random.Range(0, 100), Random.Range(0, 100));
	    axisNeptune = new Vector3 (0, Random.Range(0, 100), Random.Range(0, 100));
	}
	
	void Update () {
       
        Mercury.RotateAround (sun.position, axisMercury, 20*Time.deltaTime);
        Mercury.Rotate (Vector3.up*50*Time.deltaTime);

        Venus.RotateAround (sun.position, axisVenus, 10*Time.deltaTime);
        Venus.Rotate (Vector3.up*30*Time.deltaTime);

        Earth.RotateAround (sun.position, axisEarth, 10*Time.deltaTime);
        Earth.Rotate (Vector3.up*30*Time.deltaTime);
        moon.transform.RotateAround (Earth.position, Vector3.up, 359 * Time.deltaTime);

        Mars.RotateAround (sun.position, axisMars, 8*Time.deltaTime);
        Mars.Rotate (Vector3.up*30*Time.deltaTime);

        Jupiter.RotateAround (sun.position, axisJupiter, 7*Time.deltaTime);
        Jupiter.Rotate (Vector3.up*30*Time.deltaTime);

        Saturn.RotateAround (sun.position, axisSaturn, 6*Time.deltaTime);
        Saturn.Rotate (Vector3.up*30*Time.deltaTime);

        Uranus.RotateAround (sun.position, axisUranus, 5*Time.deltaTime);
        Uranus.Rotate (Vector3.up*30*Time.deltaTime);

        Neptune.RotateAround (sun.position, axisNeptune, 4*Time.deltaTime);
        Neptune.Rotate (Vector3.up*30*Time.deltaTime);
	}
}
