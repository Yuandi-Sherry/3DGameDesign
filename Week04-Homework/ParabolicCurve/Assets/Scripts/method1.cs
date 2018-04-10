using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class method1 : MonoBehaviour {

	public float initialVelocityRight;
	public float initialVelocityUp;

	float count;
	// Update is called once per frame
	void Start() {
		this.transform.position = new Vector3(-5,2,0);
		count = 0.0f;
	}
	void Update () {
		count += Time.deltaTime;
		transform.position += new Vector3(initialVelocityRight, initialVelocityUp-9.8f*count, 0.0f)*Time.deltaTime;
	}
}
