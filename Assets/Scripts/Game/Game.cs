using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WsUtils;
using WsFSM;

namespace WsBaccarat
{
	public enum XYindex
	{
		tie = 0,
		banker = 1,
		player = 2
	}
	public class Game : MonoBehaviour
	{
		#region 状态机
		private WsStateMachine _fsm;

		private WsState _preparingState;  //点击开始按钮后
		private WsState _shufflingState;    //洗牌中
		private WsState _bettingState;  //押注中
		private WsState _accountingState;  //结算中

		private WsTransition _prepareShuffle;
		private WsTransition _shuffleBet;
		private WsTransition _betAccount;
		private WsTransition _accountBet;	//结算后直接开下一局
		private WsTransition _accountShuffle;	//结算后重新洗牌,洗牌前先对单

		private bool _isLoaded;
		private float _shufTime;
		#endregion

		#region 游戏对象
		public GameObject chipPrefab;
		public List<List<GameObject>> chips;
		public List<List<int>> keymap = new List<List<int>>();
		#endregion

		#region 游戏参数
		private int count_down = 30;
		private GameObject desk;
		private Vector3[,] chipxy;
		#endregion

		void Awake()
		{
			InitFsm();
		}

		private void InitFsm()
		{
			#region 资源加载
			_preparingState = new WsState("preparing");
			_preparingState.OnEnter += LoadResource;
			_prepareShuffle = new WsTransition("preShuffle", _preparingState, _shufflingState);

			_prepareShuffle.OnCheck += ResLoaded;
			_preparingState.AddTransition(_prepareShuffle);
			#endregion

			#region 洗牌中
			_shufflingState = new WsState("shuffling");
			_shufflingState.OnEnter += GetShufTime;
			_shufflingState.OnUpdate += Shuffle;

			_shuffleBet = new WsTransition("shuffleBet", _shufflingState, _bettingState);
			_shuffleBet.OnCheck += Shuffled;
			_shufflingState.AddTransition(_shuffleBet);
			#endregion


			#region 开始押注
			_bettingState = new WsState("betting");
			_bettingState.OnEnter += GetBetTime;
			_bettingState.OnUpdate += Bet;

			_betAccount = new WsTransition("betAccount", _bettingState, _accountingState);
			_betAccount.OnCheck += Betted;
			_bettingState.AddTransition(_betAccount);
			#endregion


			#region 一局结束后结算
			_accountingState = new WsState("accounting");
			_accountingState.OnEnter += GetAccountTime;
			_accountingState.OnUpdate += Account;

			//直接开下局押注
			_accountBet = new WsTransition("accountBet", _accountingState, _bettingState);
			_accountBet.OnCheck += RoundAccounted;
			_accountingState.AddTransition(_accountBet);

			//每场最后局结束，要重新洗牌
			_accountShuffle = new WsTransition("accountShuffle", _accountingState, _shufflingState);
			_accountShuffle.OnCheck += SessionAccounted;
			_accountingState.AddTransition(_accountShuffle);
			#endregion


			_fsm = new WsStateMachine("baccarat", _preparingState);
			_fsm.AddState(_preparingState);
			_fsm.AddState(_shufflingState);
			_fsm.AddState(_bettingState);
			_fsm.AddState(_accountingState);

		}

		private bool RoundAccounted()
		{
			throw new NotImplementedException();
		}

		private bool SessionAccounted()
		{
			throw new NotImplementedException();
		}

		private void Account(float f)
		{
			throw new NotImplementedException();
		}

		private void GetAccountTime(IState state)
		{
			throw new NotImplementedException();
		}

		private bool Betted()
		{
			throw new NotImplementedException();
		}

		private void Bet(float f)
		{
			throw new NotImplementedException();
		}

		private void GetBetTime(IState state)
		{
			throw new NotImplementedException();
		}

		private bool Shuffled()
		{
			throw new NotImplementedException();
		}

		private void Shuffle(float f)
		{
			throw new NotImplementedException();
		}

		private void GetShufTime(IState state)
		{
			throw new NotImplementedException();
		}

		private bool ResLoaded()
		{
			return _isLoaded;
		}

		private void LoadResource(IState state)
		{
			throw new NotImplementedException();
		}

		//private Transform position;
		void InitXY()
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
		void InitPositions()
		{
			foreach (var i in chips)
			{
				foreach (var obj in i)
				{

					obj.transform.parent = desk.transform;
				}
			}
		}

		void InitKeymap()
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
		private void Start()
		{
			//initXY();
			InitKeymap();
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
}
