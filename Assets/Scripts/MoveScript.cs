using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {

	// 1 - Designer variables
	// Object speed 
	public Vector2 speed = new Vector2(10, 10);
	// Moving direction 
	public Vector2 direction = new Vector2(-1, 0);
	private Vector2 movement;
	void Update()
	{
		// 2 - Movement 
		movement = new Vector2(speed.x * direction.x, speed.y * direction.y);
	}
	void FixedUpdate()
	{
		// Apply movement to the rigidbody 
		GetComponent<Rigidbody2D>().velocity = movement;
	}
}
