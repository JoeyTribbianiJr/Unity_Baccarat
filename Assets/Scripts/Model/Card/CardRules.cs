using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WsBaccarat
{
	/// <summary>
	/// 出牌规则
	/// </summary>
	public class CardRules
	{
		/// <summary>
		/// 卡牌数组排序
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static void SortCards(List<Card> cards, bool ascending)
		{
			cards.Sort(
				(Card a, Card b) =>
				{
					if (!ascending)
					{
					//先按照权重降序，再按花色升序
					return -a.GetCardWeight.CompareTo(b.GetCardWeight) * 2 +
							a.GetCardSuit.CompareTo(b.GetCardSuit);
					}
					else
					//按照权重升序
					return a.GetCardWeight.CompareTo(b.GetCardWeight);
				}
			);
		}

		/// <summary>
		/// 是否是单
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static bool IsSingle(Card[] cards)
		{
			if (cards.Length == 1)
				return true;
			else
				return false;
		}

		/// <summary>
		/// 是否是对子
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static bool IsDouble(Card[] cards)
		{
			if (cards.Length == 2)
			{
				if (cards[0].GetCardWeight == cards[1].GetCardWeight)
					return true;
			}

			return false;
		}

		/// <summary>
		/// 是否是顺子
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static bool IsStraight(Card[] cards)
		{
			if (cards.Length < 5 || cards.Length > 12)
				return false;
			for (int i = 0; i < cards.Length - 1; i++)
			{
				Weight w = cards[i].GetCardWeight;
				if (cards[i + 1].GetCardWeight - w != 1)
					return false;

				//不能超过A
				if (w > Weight.One || cards[i + 1].GetCardWeight > Weight.One)
					return false;
			}

			return true;
		}

		/// <summary>
		/// 是否是双顺子
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static bool IsDoubleStraight(Card[] cards)
		{
			if (cards.Length < 6 || cards.Length % 2 != 0)
				return false;

			for (int i = 0; i < cards.Length; i += 2)
			{
				if (cards[i + 1].GetCardWeight != cards[i].GetCardWeight)
					return false;

				if (i < cards.Length - 2)
				{
					if (cards[i + 2].GetCardWeight - cards[i].GetCardWeight != 1)
						return false;

					//不能超过A
					if (cards[i].GetCardWeight > Weight.One || cards[i + 2].GetCardWeight > Weight.One)
						return false;
				}
			}

			return true;
		}

		/// <summary>
		/// 飞机不带
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static bool IsTripleStraight(Card[] cards)
		{
			if (cards.Length < 6 || cards.Length % 3 != 0)
				return false;

			for (int i = 0; i < cards.Length; i += 3)
			{
				if (cards[i + 1].GetCardWeight != cards[i].GetCardWeight)
					return false;
				if (cards[i + 2].GetCardWeight != cards[i].GetCardWeight)
					return false;
				if (cards[i + 1].GetCardWeight != cards[i + 2].GetCardWeight)
					return false;

				if (i < cards.Length - 3)
				{
					if (cards[i + 3].GetCardWeight - cards[i].GetCardWeight != 1)
						return false;

					//不能超过A
					if (cards[i].GetCardWeight > Weight.One || cards[i + 3].GetCardWeight > Weight.One)
						return false;
				}
			}

			return true;
		}

		/// <summary>
		/// 三不带
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static bool IsOnlyThree(Card[] cards)
		{
			if (cards.Length % 3 != 0)
				return false;
			if (cards[0].GetCardWeight != cards[1].GetCardWeight)
				return false;
			if (cards[1].GetCardWeight != cards[2].GetCardWeight)
				return false;
			if (cards[0].GetCardWeight != cards[2].GetCardWeight)
				return false;

			return true;
		}


		/// <summary>
		/// 三带一
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static bool IsThreeAndOne(Card[] cards)
		{
			if (cards.Length != 4)
				return false;

			if (cards[0].GetCardWeight == cards[1].GetCardWeight &&
				cards[1].GetCardWeight == cards[2].GetCardWeight)
				return true;
			else if (cards[1].GetCardWeight == cards[2].GetCardWeight &&
				cards[2].GetCardWeight == cards[3].GetCardWeight)
				return true;
			return false;
		}

		/// <summary>
		/// 三代二
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static bool IsThreeAndTwo(Card[] cards)
		{
			if (cards.Length != 5)
				return false;

			if (cards[0].GetCardWeight == cards[1].GetCardWeight &&
				cards[1].GetCardWeight == cards[2].GetCardWeight)
			{
				if (cards[3].GetCardWeight == cards[4].GetCardWeight)
					return true;
			}

			else if (cards[2].GetCardWeight == cards[3].GetCardWeight &&
				cards[3].GetCardWeight == cards[4].GetCardWeight)
			{
				if (cards[0].GetCardWeight == cards[1].GetCardWeight)
					return true;
			}

			return false;
		}

		/// <summary>
		/// 炸弹
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static bool IsBoom(Card[] cards)
		{
			if (cards.Length != 4)
				return false;

			if (cards[0].GetCardWeight != cards[1].GetCardWeight)
				return false;
			if (cards[1].GetCardWeight != cards[2].GetCardWeight)
				return false;
			if (cards[2].GetCardWeight != cards[3].GetCardWeight)
				return false;

			return true;
		}


		/// <summary>
		/// 王炸
		/// </summary>
		/// <param name="cards"></param>
		/// <returns></returns>
		public static bool IsJokerBoom(Card[] cards)
		{


			return false;
		}


	}
}
