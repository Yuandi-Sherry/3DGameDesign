using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class method2 : MonoBehaviour {
	public float initialVelocityRight;
	public float initialVelocityUp;
	private Rigidbody rb;
	float count;
	// Update is called once per frame
	void Start() {
		gameObject.AddComponent<Rigidbody>();
		rb = gameObject.GetComponent<Rigidbody>();
		rb.useGravity = true;
		rb.velocity = new Vector3(initialVelocityRight, initialVelocityUp, 0.0f);
	}
	void Update () {
		
	}
}
