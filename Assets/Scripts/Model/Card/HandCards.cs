using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WsBaccarat
{
	/// <summary>
	/// 手牌
	/// </summary>
	public class HandCards : MonoBehaviour
	{
		public CharacterType cType;
		private List<Card> library;
		private int multiples;//玩家倍数
		private int integration;//积分
		void Start()
		{
			multiples = 1;
			library = new List<Card>();
		}


		/// <summary>
		/// 积分
		/// </summary>
		public int Integration
		{
			set { integration = value; }
			get { return integration; }
		}

		/// <summary>
		/// 玩家倍数
		/// </summary>
		public int Multiples
		{
			set { multiples *= value; }
			get { return multiples; }
		}

		/// <summary>
		/// 手牌数
		/// </summary>
		public int CardsCount
		{
			get { return library.Count; }
		}

		/// <summary>
		/// 获取手牌
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Card this[int index]
		{
			get { return library[index]; }
		}

		/// <summary>
		/// 获取值的索引
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public int this[Card card]
		{
			get { return library.IndexOf(card); }
		}

		/// <summary>
		/// 添加手牌
		/// </summary>
		/// <param name="card"></param>
		public void AddCard(Card card)
		{
			//card.Attribution = cType;
			library.Add(card);
		}

		/// <summary>
		/// 出牌
		/// </summary>
		/// <returns></returns>
		public void PopCard(Card card)
		{
			//从手牌移除
			library.Remove(card);
		}

		/// <summary>
		/// 手牌排序
		/// </summary>
		public void Sort()
		{
			CardRules.SortCards(library, false);
		}
	}
}
