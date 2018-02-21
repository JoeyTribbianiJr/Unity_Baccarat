using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	public GameObject chipPrefab;
	public List<List<GameObject>> chips;
	public List<List<int>> keymap = new List<List<int>>();

	private int count_down = 30;
	private GameObject desk;
	private Vector3[,] chipxy;

	enum xyindex
	{
		tie = 0,
		banker = 1,
		player = 2
	}
	//private Transform position;
	void initXY()
	{
		desk = GameObject.Find("Desk");

		chipxy = new Vector3[14, 3];
		chips = new List<List<GameObject>>();

		for (int i = 0; i < 7; i++)
		{
			var objs = new List<GameObject>();
			for (int j = 0; j < 3; j++)
			{
				var x = -1.7f + j * 0.55f;
				var y = -1.76f + i * 0.38f;
				var v = new Vector3(x, y);
				chipxy[i, j] = v;

				GameObject obj = Instantiate(chipPrefab);//, v, new Quaternion(0, 0, 0, 0)) as GameObject;
				var tr = desk.transform;
				obj.transform.SetParent(tr);

				obj.transform.SetPositionAndRotation(v, new Quaternion());

				obj.SetActive(false);
				objs.Add(obj);
			}
			chips.Add(objs);
		}
		for (int i = 7; i < 14; i++)
		{
			var objs = new List<GameObject>();
			for (int j = 0; j < 3; j++)
			{
				var x = -1.7f + j * 0.55f;
				var y = -1.76f + i * 0.38f;
				var v = new Vector3(x, y);
				chipxy[i, j] = v;

				GameObject obj = Instantiate(chipPrefab);//, v, new Quaternion(0, 0, 0, 0)) as GameObject;
				var tr = desk.transform;
				obj.transform.SetParent(tr);
				//obj.transform.SetPositionAndRotation(v, new Quaternion());
				obj.SetActive(false);
				objs.Add(obj);
			}
			chips.Add(objs);
		}

	}
	void initPositions()
	{
		foreach (var i in chips)
		{
			foreach (var obj in i)
			{

				obj.transform.parent = desk.transform;
			}
		}
	}

	void initKeymap()
	{
		var content = FileUtils.getInstance().readStreamingFile("keymap.txt");
		var lines = content.Split('\n');

		//Debug.Log(keymap.Length);
		for (int i = 0; i < lines.Length - 1; i += 8)
		{
			List<int> keys = new List<int>();
			for (int j = 0; j < 8; j++)
			{
				var arr = lines[i + j].Split('=');
				if (0 < j && j < 7 && arr.Length == 2)
				{
					//var x = Convert.ToInt32(arr[0].Substring(arr[0].Length - 1, 1));
					keys.Add(Convert.ToInt32(arr[1]));
				}
			}
			keymap.Add(keys);
		}

	}
	//void Awake()
	//{
	//	initXY();
	//}
	private void Start()
	{
		//initXY();
		initKeymap();
		FullScreen();
		CountDown(120);
	}
	void Update()
	{
	}
	private void OnGUI()
	{
		if (Input.anyKeyDown)
		{

			Event e = Event.current;
			if (e.isKey)
			{
				int code = (int)e.keyCode;
				HandleKey(code);
			}
		}
		if (GUI.Button(new Rect(10, 50, 100, 30), "client"))
		{
			ClientConnectServerAction();
		}
		if (GUI.Button(new Rect(10, 50, 100, 30), "add player"))
		{
			ClientScene.AddPlayer(31);
		}

	}


	void HandleKey(int keycode)
	{

	}
	GameObject FindChip(int keycode)
	{
		foreach (var map in keymap)
		{
			if (map.Contains(keycode))
			{
				var p_idx = keymap.IndexOf(map);
				var k_idx = map.IndexOf(keycode);
				var chip = chips[p_idx][k_idx];
				return chip;

				//Debug.Log("code:" + code + ",p_idx:" + p_idx + ",k_idx:" + k_idx);
			}
		}
		return null;
	}
	void SetChipText(GameObject chip, int num)
	{
		chip.SetActive(true);
		var canvas = chip.GetComponent<Canvas>();
		var tt = canvas.transform.Find("Text").GetComponent<Text>();
		tt.text = num.ToString();
		chip.SetActive(true);

	}

	#region 倒计时 .Done
	void CountDown(int secounds)
	{
		StartCoroutine(Timer(secounds));
	}
	IEnumerator Timer(int time)
	{
		while (time > 0)
		{
			yield return new WaitForSeconds(1);
			SetCountDownText(time);
			time--;
		}
	}
	void SetCountDownText(int time)
	{
		var canvas = GameObject.Find("Cvs_clock");
		var tt = canvas.transform.Find("Clock").GetComponent<Text>();
		tt.text = time.ToString();
	}
	#endregion

	void FullScreen()
	{

		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		float cameraHeight = Camera.main.orthographicSize * 2;
		Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
		Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

		Vector2 scale = transform.localScale;
		if (cameraSize.x >= cameraSize.y)
		{ // Landscape (or equal)
			scale *= cameraSize.x / spriteSize.x;
		}
		else
		{ // Portrait
			scale *= cameraSize.y / spriteSize.y;
		}

		transform.position = Vector2.zero; // Optional
		transform.localScale = scale;

	}


	#region 网络监听
	public NetworkClient svr_client;
	public GameObject playerPre;
	public void ClientConnectServerAction()
	{
		svr_client = new NetworkClient();
		//连接服务器
		//注册客户端连接之后的回调方法
		svr_client.RegisterHandler(MsgType.Connect, ClientConnectServerCallback);
		print("已注册回调");
		svr_client.Connect("127.0.0.1", 1345);
	}
	void ClientConnectServerCallback(NetworkMessage msg)
	{
		print("已连接");
		//告诉服务器准备完毕
		ClientScene.Ready(msg.conn);
		//向服务器注册预设体(预设体必须是网络对象)
		ClientScene.RegisterPrefab(playerPre);
		//向服务器发送添加游戏对象的请求
		//服务器在接收这个请求之后会自动调用 添加游戏玩家的回调方法
		ClientScene.AddPlayer(31);
	}
	#endregion
}
