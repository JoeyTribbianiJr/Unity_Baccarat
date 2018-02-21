using System.Collections.Generic;
using System.Linq;

namespace WsBaccarat
{
	public enum BetSide
	{
		banker = 0,
		tie,
		player
	}
	public enum BetDenomination
	{
		big = 100,
		small = 10
	}

	public class Player
	{

		private int id;
		private int balance;
		private int last_add;
		private int add_score;
		private int last_sub;
		private int sub_score;
		private bool bet_hide;
		private Dictionary<BetSide, int> bet_score;

		public Dictionary<BetSide, int> BetScore
		{
			get { return bet_score; }
		}

		public BetDenomination denomination;

		public delegate int NativeCountRule(BetSide winner, Dictionary<BetSide, int> bet);
		public NativeCountRule count_rule;

		private void ClearBet()
		{
			bet_score = new Dictionary<BetSide, int>()  //本次押注
		{
			{BetSide.banker,0 },
			{BetSide.player,0 },
			{BetSide.tie,0 },
		};
		}
		public Player(int id, NativeCountRule count_rule)
		{
			this.id = id;
			balance = 0;        //总积分;
			last_add = 0;       //最后上分
			add_score = 0;      //总上分
			last_sub = 0;       //最后下分
			sub_score = 0;      //总下分
			bet_score = new Dictionary<BetSide, int>()  //本次押注
		{
			{BetSide.banker,0 },
			{BetSide.player,0 },
			{BetSide.tie,0 },
		};
			denomination = BetDenomination.big;  //押注筹码大小
			bet_hide = false;       //是否隐藏压哪边

			this.count_rule = count_rule;
		}

		public void Bet(BetSide side)
		{
			var add_score = balance >= (int)denomination ? (int)denomination : balance;

			balance -= add_score;
			bet_score[side] += add_score;
		}
		public void CancleBet()
		{
			var cancle_score = bet_score.Values.Sum();
			balance += cancle_score;

			ClearBet();
		}


		public void SetAmount()
		{
			denomination = denomination == BetDenomination.big ? BetDenomination.small : BetDenomination.big;
		}
		public void SetHide()
		{
			bet_hide = bet_hide == true ? false : true;
		}

		public void Count(BetSide winner)
		{
			balance += count_rule(winner, bet_score);
		}
	}
}