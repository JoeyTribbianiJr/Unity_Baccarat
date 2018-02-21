using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace WsBaccarat
{
	public class GameController
	{
		private List<Session> sessions;
		public Session cur_session;
		public List<Round> all_rounds;
		public int all_rounds_num;
		public int round_idx;

		public void InitAsset()
		{

		}
		public void InitSetting()
		{

		}
		public GameController()
		{
			ResetGame();
		}
		public void ResetGame()
		{

			all_rounds_num = Setting.Instance.GetIntSetting("all_rounds_num");
			all_rounds = CreateRounds(all_rounds_num);

			sessions = new List<Session>();
			cur_session = new Session(0, all_rounds);
			sessions.Add(cur_session);
			round_idx = 0;
		}
		public void StartGame()
		{
			var shuf_tm = Setting.Instance.GetIntSetting("shuf_tm");
			//CountDown(120);
		}
		//private void CountDown(int secs)
		//{

		//}
		// 1th step
		private List<Round> CreateRounds(int rounds_num)
		{
			var rounds = new List<Round>();
			for (int i = 0; i < rounds_num; i++)
			{
				var hand_cards = Deck.Instance.deal_native_hand();
				var round = new Round()
				{
					hand_card = hand_cards,
					winner = Desk.Instance.GetWinner(hand_cards)
				};
				rounds.Add(round);
			}
			return rounds;
		}
		void Start()
		{

			//InitMenu();
		}



		///// <summary>
		///// 初始化菜单面板
		///// </summary>
		//public void InitMenu()
		//{
		//	GameObject panel = NGUITools.AddChild(UICamera.mainCamera.gameObject, (GameObject)Resources.Load("StartPanel"));
		//	panel.AddComponent<Menu>();
		//	panel.transform.Find("NoticeLabel").gameObject.SetActive(false);
		//}

		//// <summary>
		///// 初始化场景個数据
		///// </summary>
		//public void InitScene()
		//{
		//	string fileName = "";
		//	if (Application.platform == RuntimePlatform.Android)
		//	{
		//		fileName = Application.persistentDataPath;
		//	}
		//	else
		//	{
		//		fileName = Application.dataPath;
		//	}
		//	FileInfo info = new FileInfo(fileName + @"\data.json");

		//	GameObject bg = NGUITools.AddChild(UICamera.mainCamera.gameObject,
		//(GameObject)Resources.Load("BackgroundPanel"));
		//	bg.name = bg.name.Replace("(Clone)", "");
		//	GameObject scene = NGUITools.AddChild(UICamera.mainCamera.gameObject,
		//		(GameObject)Resources.Load("ScenePanel"));
		//	scene.name = scene.name.Replace("(Clone)", "");
		//	GameObject player = scene.transform.Find("Player").gameObject;
		//	HandCards playerCards = player.AddComponent<HandCards>();
		//	playerCards.cType = CharacterType.Player;
		//	player.AddComponent<PlayCard>();

		//	GameObject computerOne = scene.transform.Find("ComputerOne").gameObject;
		//	HandCards computerOneCards = computerOne.AddComponent<HandCards>();
		//	computerOneCards.cType = CharacterType.ComputerOne;
		//	computerOne.AddComponent<SimpleSmartCard>();
		//	computerOne.transform.Find("ComputerNotice").gameObject.SetActive(false);

		//	GameObject computerTwo = scene.transform.Find("ComputerTwo").gameObject;
		//	HandCards computerTwoCards = computerTwo.AddComponent<HandCards>();
		//	computerTwoCards.cType = CharacterType.ComputerTwo;
		//	computerTwo.AddComponent<SimpleSmartCard>();
		//	computerTwo.transform.Find("ComputerNotice").gameObject.SetActive(false);

		//	GameObject desk = scene.transform.Find("Desk").gameObject;
		//	desk.transform.Find("NoticeLabel").gameObject.SetActive(false);

		//	if (!info.Exists)
		//	{
		//		playerCards.Integration = 1000;
		//		computerOneCards.Integration = 1000;
		//		computerTwoCards.Integration = 1000;
		//	}
		//	else
		//	{
		//		GameData data = GetDataWithoutBOM(fileName);

		//		playerCards.Integration = data.playerIntegration;
		//		computerOneCards.Integration = data.computerOneIntegration;
		//		computerTwoCards.Integration = data.computerTwoIntegration;
		//	}

		//	GameController.UpdateIntegration(CharacterType.Player);
		//	GameController.UpdateIntegration(CharacterType.ComputerOne);
		//	GameController.UpdateIntegration(CharacterType.ComputerTwo);

		//}

		///// <summary>
		///// 洗牌
		///// </summary>
		//public void DealCards()
		//{
		//	//洗牌
		//	Deck.Instance.Shuffle();

		//	//发牌
		//	CharacterType currentCharacter = CharacterType.Player;
		//	for (int i = 0; i < 51; i++)
		//	{
		//		if (currentCharacter == CharacterType.Desk)
		//		{
		//			currentCharacter = CharacterType.Player;
		//		}
		//		DealTo(currentCharacter);
		//		currentCharacter++;
		//	}

		//	for (int i = 0; i < 3; i++)
		//	{
		//		DealTo(CharacterType.Desk);
		//	}

		//	//
		//	for (int i = 1; i < 5; i++)
		//	{
		//		MakeHandCardsSprite((CharacterType)i, false);
		//	}
		//}

		///// <summary>
		///// 游戏结束
		///// </summary>
		//public void GameOver()
		//{
		//	GameObject currentCharactor = GameObject.Find(OrderController.Instance.Type.ToString());
		//	Identity identity = currentCharactor.GetComponent<HandCards>().AccessIdentity;

		//	//统计所有人的分数
		//	for (int i = 1; i < 4; i++)
		//	{
		//		StatisticalIntegral((CharacterType)i, identity);
		//	}

		//	//结束界面
		//	GameObject gameover = NGUITools.AddChild(UICamera.mainCamera.gameObject,
		//(GameObject)Resources.Load("GameOverPanel"));
		//	gameover.AddComponent<Restart>();

		//	int playerIntegration = GameObject.Find(CharacterType.Player.ToString()).GetComponent<HandCards>().Integration;
		//	int computerOneIntegration = GameObject.Find(CharacterType.ComputerOne.ToString()).GetComponent<HandCards>().Integration;
		//	int computerTwoIntegration = GameObject.Find(CharacterType.ComputerTwo.ToString()).GetComponent<HandCards>().Integration;
		//	if (playerIntegration > 0)
		//	{
		//		gameover.GetComponent<Restart>().SetTimeToNext(3.0f);

		//		if (computerOneIntegration <= 0)
		//		{
		//			StartCoroutine(ChangeComputer(CharacterType.ComputerOne));
		//		}

		//		if (computerTwoIntegration <= 0)
		//		{
		//			StartCoroutine(ChangeComputer(CharacterType.ComputerTwo));
		//		}

		//		DisplayOverInfo(true, gameover, identity);
		//		//数据存储
		//		GameData data = new GameData
		//		{

		//			playerIntegration = playerIntegration > 0 ? playerIntegration : 1000,
		//			computerOneIntegration = computerOneIntegration > 0 ? computerOneIntegration : 1000,
		//			computerTwoIntegration = computerTwoIntegration > 0 ? computerTwoIntegration : 1000
		//		};
		//		SaveDataWithUTF8(data);
		//	}
		//	else
		//	{
		//		DisplayOverInfo(false, gameover, identity);
		//		//数据存储
		//		GameData data = new GameData
		//		{
		//			playerIntegration = 1000,
		//			computerOneIntegration = 1000,
		//			computerTwoIntegration = 1000
		//		};
		//		SaveDataWithUTF8(data);
		//	}
		//}

		///// <summary>
		///// 统计积分
		///// </summary>
		///// <param name="type"></param>
		///// <param name="winner"></param>
		//void StatisticalIntegral(CharacterType type, Identity winner)
		//{
		//	//积分计算
		//	HandCards cards = GameObject.Find(type.ToString()).GetComponent<HandCards>();

		//	int integration = cards.Multiples * Multiples * basePointPerMatch * (int)(cards.AccessIdentity + 1);

		//	if (cards.AccessIdentity != winner)
		//		integration = -integration;

		//	cards.Integration = cards.Integration + integration;

		//	//更新显示
		//	UpdateIntegration(type);
		//}

		///// <summary>
		///// 更换电脑玩家
		///// </summary>
		///// <param name="type"></param>
		///// <returns></returns>
		//IEnumerator ChangeComputer(CharacterType type)
		//{
		//	DisplayDeskNotice(true);
		//	yield return new WaitForSeconds(3.0f);
		//	GameObject oldComputer = GameObject.Find(type.ToString());
		//	Destroy(oldComputer);
		//	MatchNewComputer(type);
		//}

		///// <summary>
		///// 匹配新的电脑玩家
		///// </summary>
		///// <param name="type"></param>
		//void MatchNewComputer(CharacterType type)
		//{
		//	BackToDeck();
		//	DestroyAllSprites();
		//	DeskCardsCache.Instance.Clear();
		//	OrderController.Instance.ResetSmartCard();

		//	DisplayDeskNotice(false);
		//	GameObject newComputer = NGUITools.AddChild(GameObject.Find("ScenePanel"), (GameObject)Resources.Load(type.ToString()));
		//	newComputer.name = newComputer.name.Replace("(Clone)", "");
		//	newComputer.AddComponent<HandCards>().cType = type;
		//	newComputer.AddComponent<HandCards>().Integration = 1000;
		//	newComputer.transform.Find("ComputerNotice").gameObject.SetActive(false);
		//	newComputer.AddComponent<SimpleSmartCard>();
		//	if (Random.Range(1, 3) == 1)
		//	{
		//		newComputer.transform.Find("HeadPortrait").gameObject.GetComponent<UISprite>().spriteName = "role1";
		//	}
		//	else
		//	{
		//		newComputer.transform.Find("HeadPortrait").gameObject.GetComponent<UISprite>().spriteName = "role";
		//	}
		//}

		///// <summary>
		///// 存储数据
		///// </summary>
		///// <param name="data"></param>
		//void SaveDataWithUTF8(GameData data)
		//{
		//	string fileName = "";
		//	if (Application.platform == RuntimePlatform.Android)
		//	{
		//		fileName = Application.persistentDataPath;
		//	}
		//	else
		//	{
		//		fileName = Application.dataPath;
		//	}

		//	Stream stream = new FileStream(fileName + @"\data.json", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		//	StreamWriter writter = new StreamWriter(stream, Encoding.UTF8);
		//	XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
		//	xmlSerializer.Serialize(writter, data);
		//	writter.Close();
		//	stream.Close();
		//}

		///// <summary>
		///// 跳过utf-8 xml BOM的方法
		///// </summary>
		///// <param name="fileName"></param>
		///// <returns></returns>
		//GameData GetDataWithoutBOM(string fileName)
		//{
		//	GameData data = new GameData();
		//	Stream stream = new FileStream(fileName + @"\data.json", FileMode.Open, FileAccess.Read, FileShare.None);
		//	StreamReader streamReader = new StreamReader(stream, true);//true跳过bom

		//	XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
		//	data = xmlSerializer.Deserialize(streamReader) as GameData;
		//	streamReader.Close();
		//	stream.Close();

		//	return data;
		//}

		///// <summary>
		///// 场上的所有牌回到牌库中
		///// </summary>
		//public void BackToDeck()
		//{
		//	HandCards[] handCards = new HandCards[3];
		//	handCards[0] = GameObject.Find("Player").GetComponent<HandCards>();
		//	handCards[1] = GameObject.Find("ComputerOne").GetComponent<HandCards>();
		//	handCards[2] = GameObject.Find("ComputerTwo").GetComponent<HandCards>();

		//	for (int i = 0; i < handCards.Length; i++)
		//	{
		//		while (handCards[i].CardsCount != 0)
		//		{
		//			Card last = handCards[i][handCards[i].CardsCount - 1];
		//			Deck.Instance.AddCard(last);
		//			handCards[i].PopCard(last);
		//		}
		//	}
		//}

		///// <summary>
		///// 销毁精灵
		///// </summary>
		//public void DestroyAllSprites()
		//{
		//	CardSprite[][] cardSprites = new CardSprite[3][];
		//	cardSprites[0] = GameObject.Find("Player").GetComponentsInChildren<CardSprite>();
		//	cardSprites[1] = GameObject.Find("ComputerOne").GetComponentsInChildren<CardSprite>();
		//	cardSprites[2] = GameObject.Find("ComputerTwo").GetComponentsInChildren<CardSprite>();

		//	for (int i = 0; i < cardSprites.GetLength(0); i++)
		//	{
		//		for (int j = 0; j < cardSprites[i].Length; j++)
		//		{
		//			cardSprites[i][j].Destroy();
		//		}
		//	}
		//}

		///// <summary>
		///// 发底牌
		///// </summary>
		///// <param name="type"></param>
		//public void CardsOnTable(CharacterType type)
		//{
		//	GameObject parentObj = GameObject.Find(type.ToString());
		//	HandCards cards = parentObj.GetComponent<HandCards>();
		//	//积分翻倍
		//	cards.Multiples = 2;
		//	//销毁底牌精灵
		//	CardSprite[] sprites = GameObject.Find("Desk").GetComponentsInChildren<CardSprite>();
		//	for (int i = 0; i < sprites.Length; i++)
		//	{
		//		sprites[i].Destroy();
		//	}

		//	while (DeskCardsCache.Instance.CardsCount != 0)
		//	{
		//		Card card = DeskCardsCache.Instance.Deal();
		//		cards.AddCard(card);
		//	}
		//	MakeHandCardsSprite(type, true);
		//	//更新身份
		//	UpdateIndentity(type, Identity.Landlord);
		//}

		///// <summary>
		///// 发牌
		///// </summary>
		///// <param name="person"></param>
		//void DealTo(CharacterType person)
		//{
		//	if (person == CharacterType.Desk)
		//	{
		//		Card movedCard = Deck.Instance.Deal();
		//		DeskCardsCache.Instance.AddCard(movedCard);
		//	}
		//	else
		//	{
		//		GameObject playerObj = GameObject.Find(person.ToString());
		//		HandCards cards = playerObj.GetComponent<HandCards>();

		//		Card movedCard = Deck.Instance.Deal();
		//		Deck.Instance.ShuffledDeck[2] = movedCard;

		//		cards.AddCard(movedCard);
		//	}
		//}

		///// <summary>
		///// 使卡牌精灵化
		///// </summary>
		///// <param name="type"></param>
		///// <param name="card"></param>
		///// <param name="selected"></param>
		//void MakeSprite(CharacterType type, Card card, bool selected)
		//{
		//	if (!card.isSprite)
		//	{
		//		GameObject obj = Resources.Load("poker") as GameObject;
		//		GameObject poker = NGUITools.AddChild(GameObject.Find(type.ToString()), obj);
		//		CardSprite sprite = poker.gameObject.GetComponent<CardSprite>();
		//		sprite.Poker = card;
		//		sprite.Select = selected;
		//	}
		//}

		///// <summary>
		///// 精灵化角色手牌
		///// </summary>
		///// <param name="type"></param>
		///// <param name="isSelected"></param>
		//void MakeHandCardsSprite(CharacterType type, bool isSelected)
		//{
		//	if (type == CharacterType.Desk)
		//	{
		//		DeskCardsCache instance = DeskCardsCache.Instance;
		//		for (int i = 0; i < instance.CardsCount; i++)
		//		{
		//			MakeSprite(type, instance[i], isSelected);
		//		}
		//	}
		//	else
		//	{
		//		GameObject parentObj = GameObject.Find(type.ToString());
		//		HandCards cards = parentObj.GetComponent<HandCards>();
		//		//排序
		//		cards.Sort();
		//		//精灵化
		//		for (int i = 0; i < cards.CardsCount; i++)
		//		{
		//			if (!cards[i].isSprite)
		//			{
		//				MakeSprite(type, cards[i], isSelected);
		//			}
		//		}

		//		//显示剩余扑克
		//		UpdateLeftCardsCount(cards.cType, cards.CardsCount);
		//	}
		//	//调整精灵位置
		//	AdjustCardSpritsPosition(type);
		//}


		///// <summary>
		///// 调整手牌位置
		///// </summary>
		///// <param name="type"></param>
		//public static void AdjustCardSpritsPosition(CharacterType type)
		//{
		//	if (type == CharacterType.Desk)
		//	{
		//		DeskCardsCache instance = DeskCardsCache.Instance;
		//		CardSprite[] cs = GameObject.Find(type.ToString()).GetComponentsInChildren<CardSprite>();
		//		for (int i = 0; i < cs.Length; i++)
		//		{
		//			for (int j = 0; j < cs.Length; j++)
		//			{
		//				if (cs[j].Poker == instance[i])
		//				{
		//					cs[j].GoToPosition(GameObject.Find(type.ToString()), i);
		//				}
		//			}
		//		}
		//	}
		//	else
		//	{
		//		HandCards hc = GameObject.Find(type.ToString()).GetComponent<HandCards>();
		//		CardSprite[] cs = GameObject.Find(type.ToString()).GetComponentsInChildren<CardSprite>();
		//		for (int i = 0; i < hc.CardsCount; i++)
		//		{
		//			for (int j = 0; j < cs.Length; j++)
		//			{
		//				if (cs[j].Poker == hc[i])
		//				{
		//					cs[j].GoToPosition(GameObject.Find(type.ToString()), i);
		//				}
		//			}
		//		}
		//	}

		//}

		///// <summary>
		///// 获取指定数组的权值
		///// </summary>
		///// <param name="cards"></param>
		///// <param name="rule"></param>
		///// <returns></returns>
		//public static int GetWeight(Card[] cards, CardsType rule)
		//{
		//	int totalWeight = 0;
		//	if (rule == CardsType.ThreeAndOne || rule == CardsType.ThreeAndTwo)
		//	{
		//		for (int i = 0; i < cards.Length; i++)
		//		{
		//			if (i < cards.Length - 2)
		//			{
		//				if (cards[i].GetCardWeight == cards[i + 1].GetCardWeight &&
		//					cards[i].GetCardWeight == cards[i + 2].GetCardWeight)
		//				{
		//					totalWeight += (int)cards[i].GetCardWeight;
		//					totalWeight *= 3;
		//					break;
		//				}
		//			}
		//		}
		//	}
		//	else
		//	{
		//		for (int i = 0; i < cards.Length; i++)
		//		{
		//			totalWeight += (int)cards[i].GetCardWeight;
		//		}
		//	}

		//	return totalWeight;
		//}

		///// <summary>
		///// 更新剩余扑克显示
		///// </summary>
		///// <param name="type"></param>
		///// <param name="cardsCount"></param>
		//public static void UpdateLeftCardsCount(CharacterType type, int cardsCount)
		//{
		//	GameObject obj = GameObject.Find(type.ToString()).transform.Find("LeftPoker").gameObject;
		//	obj.GetComponent<UILabel>().text = "剩余扑克:" + cardsCount;
		//}

		///// <summary>
		///// 更新身份
		///// </summary>
		///// <param name="type"></param>
		///// <param name="identity"></param>
		//public static void UpdateIndentity(CharacterType type, Identity identity)
		//{
		//	GameObject obj = GameObject.Find(type.ToString()).transform.Find("Identity").gameObject;
		//	//改变属性
		//	GameObject.Find(type.ToString()).GetComponent<HandCards>().AccessIdentity = identity;
		//	//更改显示
		//	obj.GetComponent<UISprite>().spriteName = "Identity_" + identity.ToString();
		//}

		///// <summary>
		///// 更新积分显示
		///// </summary>
		///// <param name="type"></param>
		///// <param name="integration"></param>
		//public static void UpdateIntegration(CharacterType type)
		//{
		//	int integration = GameObject.Find(type.ToString()).GetComponent<HandCards>().Integration;
		//	GameObject obj = GameObject.Find(type.ToString()).transform.Find("IntegrationLabel").gameObject;
		//	obj.GetComponent<UILabel>().text = "积分:" + integration;
		//}

		///// <summary>
		///// 电脑玩家离开提示
		///// </summary>
		///// <param name="show"></param>
		//public static void DisplayDeskNotice(bool show)
		//{
		//	GameObject.Find("Desk").transform.Find("NoticeLabel").gameObject.SetActive(show);
		//}

		///// <summary>
		///// 显示结束信息
		///// </summary>
		///// <param name="enough"></param>
		///// <param name="gameovePanel"></param>
		///// <param name="winner"></param>
		//public static void DisplayOverInfo(bool enough, GameObject gameovePanel, Identity winner)
		//{
		//	if (enough)
		//	{
		//		gameovePanel.transform.Find("Button").gameObject.SetActive(false);
		//		gameovePanel.GetComponent<Restart>().SetTimeToNext(3.0f);
		//	}
		//	if (winner == Identity.Farmer)
		//	{
		//		gameovePanel.GetComponentInChildren<UISprite>().spriteName = "role1";
		//		gameovePanel.GetComponentInChildren<UILabel>().text = "农民获得胜利";
		//	}
		//	else
		//	{
		//		gameovePanel.GetComponentInChildren<UISprite>().spriteName = "role";
		//		gameovePanel.GetComponentInChildren<UILabel>().text = "地主获得胜利";
		//	}
		//}
	}
}
