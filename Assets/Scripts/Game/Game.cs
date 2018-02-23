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

		#region 游戏对象
		public GameObject chipPrefab;
		public GameObject cardPrefab;
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
			#region 资源初始化
			_preparingState = new WsState("preparing");
			//_preparingState.OnUpdate += InitGame;
			#endregion

			#region 洗牌中
			_shufflingState = new WsState("shuffling");
			_shufflingState.OnEnter += GetShufTime;
			_shufflingState.OnUpdate += Shuffle;
			_shufflingState.OnExit += (IState state) => { _isShuffled = false; };

			_bettingState = new WsState("betting");
			_bettingState.OnEnter += Start1Round;
			_bettingState.OnUpdate += Bet;
			_bettingState.OnExit += RoundOrSessionOver;

			_prepareShuffle = new WsTransition("preShuffle", _preparingState, _shufflingState);
			_prepareShuffle.OnTransition += InitGame;
			_prepareShuffle.OnCheck += GameInited;
			_preparingState.AddTransition(_prepareShuffle);

			_shuffleBet = new WsTransition("shuffleBet", _shufflingState, _bettingState);
			_shuffleBet.OnCheck += Shuffled;
			_shufflingState.AddTransition(_shuffleBet);
			#endregion


			#region 开始押注

			//开下一局
			_betNextBet = new WsTransition("betNextBet", _bettingState, _bettingState);
			_betNextBet.OnCheck += CanGoNextBet;
			//_betNextBet.OnTransition += DealCard;	//开牌动画，结算筹码
			_bettingState.AddTransition(_betNextBet);

			//本场结束，验单，重新洗牌
			_betShuffle = new WsTransition("betShuffle", _bettingState, _shufflingState);
			_betShuffle.OnCheck += SessionOver;
			_betShuffle.OnTransition += ExamineWaybill;
			_bettingState.AddTransition(_betShuffle);
			#endregion

			#region 押注结束后亮牌结算
			//_accountingState = new WsState("accounting");
			//_accountingState.OnEnter += GetAccountTime;
			//_accountingState.OnUpdate += Account;
			//_accountingState.OnExit += (IState s) => { _isAccounted = false; };

			////直接开下局押注
			//_accountBet = new WsTransition("accountBet", _accountingState, _bettingState);
			//_accountBet.OnCheck += RoundAccounted;
			//_accountingState.AddTransition(_accountBet);

			////每场最后局结束，要重新洗牌
			//_accountShuffle = new WsTransition("accountShuffle", _accountingState, _shufflingState);
			//_accountShuffle.OnCheck += SessionAccounted;
			//_accountingState.AddTransition(_accountShuffle);
			#endregion

			_fsm = new WsStateMachine("baccarat", _preparingState);
			_fsm.AddState(_preparingState);
			_fsm.AddState(_shufflingState);
			_fsm.AddState(_bettingState);
			//_fsm.AddState(_accountingState);
			print("init fsm over");
		}

		private bool ExamineWaybill()
		{
			SetClockText("检查路单中");
			//new WaitForSeconds(3);
			return true;
		}

		bool InitGame()
		{
			print("init game start");
			InitXY();
			InitKeymap();
			FullScreen();
			_isLoaded = true;
			print("init game over");
			return true;
		}
		private bool GameInited()
		{
			return _isLoaded = true;
		}

		private void GetShufTime(IState state)
		{
			//_shufTime = Setting.Instance.GetIntSetting("")
			print("start shuffle");
			_shufTime = 4;
			_shufTime++;
		}
		private void Shuffle(float f)
		{
			var timer = _shufflingState.Timer;
			if (timer > _shufTime + 1)
			{
				_isShuffled = true;
				return;
			}
			SetCountDownWithFloat(timer, _shufTime);
		}
		private bool Shuffled()
		{
			return _isShuffled;
		}

		private void Start1Round(IState state)
		{
			print("cur round : " + _curRoundIndex);
			print("开始押注");
			_roundNumPerSession = Setting.Instance.GetIntSetting("round_num_per_session");
			_roundNumPerSession = 3;
			_betTime = Setting.Instance.GetIntSetting("bet_tm");
			_betTime = 3;
			_is3SecOn = Setting.Instance.GetStrSetting("open_3_sec") == "3秒功能开" ? true : false;
			_curRoundIndex++;
		}
		private void Bet(float f)
		{
			if (_curRoundIndex > _roundNumPerSession)
			{
				_sessionOver = true;
				return;
			}
			_betTimer = _bettingState.Timer;

			if (_betTimer <= _betTime + 1)
			{
				SetCountDownWithFloat(_betTimer, _betTime);
			}
			else
			{
				if (_betTimer > _betTime + 8)
				{
					_animationing = false;
					_animationDone = true;
				}
				if (_animationing)
				{
					return;
				}
				else if (_animationDone)
				{
					_betRoundOver = true;
					_animationDone = false;
					return;
				}
				else
				{
					DealCard();
					_animationing = true;
					_animationDone = false;
				}
			}
		}
		private bool CanGoNextBet()
		{
			return _betRoundOver && !_sessionOver;
		}
		//private IEnumerable<WaitForSeconds> DealCard()
		private void DealCard()
		{
			SetClockText("正在开牌");

			var singleDouble = Setting.Instance.GetStrSetting("single_double");


			//Test
			singleDouble = "两张牌";


			if (singleDouble == "单张牌")
			{
				_curHandCards = Desk.Instance.DealSingleCard();
				OpenSingle();
			}
			if (singleDouble == "两张牌")
			{
				_curHandCards = Desk.Instance.DealTwoCard();
				OpenTwo();
			}
		}
		private void OpenSingle()
		{
			var playerCard = _curHandCards[1][0];
			var v2 = _cardPosition[3].transform.position;
			OpenCard(playerCard, v2,0);

			var bankerCard = _curHandCards[0][0];
			var v1 = _cardPosition[0].transform.position;
			OpenCard(bankerCard, v1,1);
		}
		private void OpenTwo()
		{
			var playerCard1 = _curHandCards[0][0];
			var v2 = _cardPosition[3].transform.position;
			OpenCard(playerCard1, v2,0);

			var bankerCard1 = _curHandCards[1][0];
			var v1 = _cardPosition[0].transform.position;
			OpenCard(bankerCard1, v1,1);

			var playerCard2 = _curHandCards[0][1];
			var v4 = _cardPosition[4].transform.position;
			OpenCard(playerCard2, v4,2);

			var bankerCard2 = _curHandCards[1][1];
			var v3 = _cardPosition[1].transform.position;
			OpenCard(bankerCard2, v3,3);

			if (_curHandCards[1].Count == 3)
			{
				var thdCard = _curHandCards[1][2];
				var bankerCard3 = thdCard;
				var v5 = _cardPosition[2].transform.position;
				OpenCard(bankerCard3, v5,4);
			}

			if (_curHandCards[0].Count == 3)
			{
				var fouCard = _curHandCards[0][2];
				var playerCard3 = fouCard;
				var v6 = _cardPosition[5].transform.position;
				OpenCard(playerCard3, v6,5);
			}
		}
		private void OpenCard(Card card, Vector3 v,int cache_i)
		{
			var sprite = InitCardSprite(card);
			//var sp_obj = Instantiate(cardPrefab);
			var sp_obj = _handCardCache[cache_i];
			sp_obj.transform.position = _cardPosition[6].transform.position;
			sp_obj.GetComponent<SpriteRenderer>().sprite = sprite;

			iTween.MoveTo(sp_obj, v, 2f);
		}
		private Sprite InitCardSprite(Card card)
		{
			var weight = card.GetCardWeight;
			var suit = card.GetCardSuit;
			if (suit == Suits.Back)
			{
				return PPTextureManage.getInstance().LoadAtlasSprite("card", "back");
			}
			else
			{
				var index = (int)suit * 13 + (int)weight;
				return PPTextureManage.getInstance().LoadAtlasSprite("card", index.ToString());
			}
		}
		private bool SessionOver()
		{
			return _sessionOver;
		}
		private void RoundOrSessionOver(IState state)
		{
			if (_sessionOver)
			{
				_curRoundIndex = 1;
				_sessionOver = false;
			}
			_betRoundOver = false;
		}


		#region 游戏初始化
		/// <summary>
		/// 初始化筹码坐标，适应缩放
		/// </summary>
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

					obj.transform.SetPositionAndRotation(v, new Quaternion());
					obj.SetActive(false);
					objs.Add(obj);
				}
				chips.Add(objs);
			}

			_cardPosition = new List<GameObject>();
			_handCardCache = new List<GameObject>();
			for (int i = 0; i < 7; i++)
			{
				GameObject obj = Instantiate(cardPrefab);
				var tr = desk.transform;
				obj.transform.SetParent(tr);

				var x = -4.5f + i * 0.45f;
				var y = -3.05f;

				if (i > 2)
				{
					x = 4.5f - (i - 3) * 0.45f;
				}
				if (i > 5)
				{
					x = -0.3f;
					y = 0.8f;
				}

				var v = new Vector3(x, y, -1);

				obj.transform.SetPositionAndRotation(v, new Quaternion());

				obj.SetActive(false);
				_cardPosition.Add(obj);

				var sp_obj = Instantiate(cardPrefab);
				_handCardCache.Add(sp_obj);
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
		#endregion

		void Start()
		{
			//InitGame(null);
			//CountDown(120);
		}
		void Update()
		{
			//print("update");
			_fsm.UpdateCallback(Time.deltaTime);
		}
		private void OnGUI()
		{
			if (_enableBet)
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
				//if (GUI.Button(new Rect(10, 50, 100, 30), "client"))
				//{
				//	ClientConnectServerAction();
				//}
				//if (GUI.Button(new Rect(10, 50, 100, 30), "add player"))
				//{
				//	ClientScene.AddPlayer(31);
				//}
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
		void SetClockText(string time)
		{
			var canvas = GameObject.Find("ClockCanvas");
			var tt = canvas.transform.Find("Clock").GetComponent<Text>();
			tt.text = time.ToString();
		}
		void SetCountDownWithFloat(float cur_timer, int seconds)
		{
			var count_down = seconds - (int)cur_timer;
			SetClockText(count_down.ToString());
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

		#region 状态机
		private WsStateMachine _fsm;

		private WsState _preparingState;  //点击开始按钮后
		private WsState _shufflingState;    //洗牌中
		private WsState _bettingState;  //押注中
		private WsState _accountingState;  //结算中

		private WsTransition _prepareShuffle;
		private WsTransition _shuffleBet;
		private WsTransition _betNextBet;
		private WsTransition _betShuffle;
		private WsTransition _accountBet;   //结算后直接开下一局
		private WsTransition _accountShuffle;   //结算后重新洗牌,洗牌前先对单

		private bool _isLoaded = false;
		private int _shufTime;
		private bool _isShuffled = false;
		private int _betTime;
		private bool _is3SecOn = false;
		//private bool _betFinished;
		private bool _enableBet = false;
		private bool _isAccounted = false;
		private float _betTimer;
		private bool _betRoundOver = false;
		private int _curRoundIndex = 1;
		private bool _sessionOver = false;
		private int _roundNumPerSession;
		private bool _isLoading;
		private bool _animationing = false;
		private bool _animationDone;
		private List<Card>[] _curHandCards;
		private List<GameObject> _cardPosition;
		private List<GameObject> _handCardCache;
		#endregion
	}
}
