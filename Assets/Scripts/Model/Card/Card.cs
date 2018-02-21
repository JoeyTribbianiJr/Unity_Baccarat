using UnityEngine;
using System.Collections;

namespace WsBaccarat
{
	/// <summary>
	/// 牌类
	/// </summary>
	public class Card
	{
		private readonly string cardName;
		private readonly Weight weight;
		private readonly Suits color;
		private bool makedSprite;

		public Card(string name, Weight weight, Suits color)
		{
			makedSprite = false;
			cardName = name;
			this.weight = weight;
			this.color = color;
		}

		/// <summary>
		/// 返回牌名
		/// </summary>
		public string GetCardName
		{
			get { return cardName; }
		}

		/// <summary>
		/// 返回权值
		/// </summary>
		public Weight GetCardWeight
		{
			get { return weight; }
		}

		/// <summary>
		/// 返回花色
		/// </summary>
		public Suits GetCardSuit
		{
			get { return color; }
		}

		/// <summary>
		/// 是否精灵化
		/// </summary>
		public bool isSprite
		{
			set { makedSprite = value; }
			get { return makedSprite; }
		}


	}
}
