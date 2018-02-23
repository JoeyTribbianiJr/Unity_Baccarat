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



		
	}
}
