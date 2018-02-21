using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WsBaccarat
{
	public class Session
	{
		public int session_id;
		private List<Round> rounds;
		private int round_num;
		private DateTime start_tm;

		//private static Session instance;
		//public static Session Instance
		//{
		//	get
		//	{
		//		if (instance == null)
		//		{
		//			instance = new Session();
		//		}
		//		return instance;
		//	}
		//}

		public Session(int id, List<Round> master_round = null)
		{
			this.session_id = id;
			ResetSession(master_round);
		}
		public void ResetSession(List<Round> master_round = null)
		{
			round_num = Setting.Instance.GetIntSetting("round_num");
			if (master_round == null)
			{
				rounds = create_rounds();
			}
			else
			{
				rounds = master_round.GetRange(round_num * (session_id - 1), round_num);
			}
			start_tm = DateTime.Now;
		}

		private List<Round> create_rounds()
		{
			var new_round = new List<Round>();
			for (int i = 0; i < round_num; i++)
			{
				var round = new Round();
				new_round.Add(round);
			}

			return new_round;
		}
	}
}
