using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WsBaccarat
{

	public class Chip : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			transform.position -= new Vector3(Time.deltaTime, 0, 0);
			if (transform.position.x < -11.36f)
			{
				transform.position = new Vector3(11.36f, transform.position.y, 0);
			}
		}
	}

}