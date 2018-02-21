using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGo : MonoBehaviour {

	public GameObject obstaclePrefab;
	private float currentTime = 0;
	private float interval = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;//每一帧计算一下累计时间
		if (currentTime > interval)
		{//时间超过时间间隔 触发事件
			currentTime = 0;//重置计数器
			Instantiate(obstaclePrefab, Vector3.zero, Quaternion.identity);
		}
	}
}
