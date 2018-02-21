using UnityEngine;
using System.Collections;

namespace WsBaccarat
{
	/// <summary>
	/// 角色类型
	/// </summary>
	public enum CharacterType
	{
		Library = 0,
		Player,
		ComputerOne,
		ComputerTwo,
		Desk
	}


	/// <summary>
	/// 花色
	/// </summary>
	public enum Suits
	{
		Club,
		Diamond,
		Heart,
		Spade,
		None
	}

	/// <summary>
	/// 卡牌权值
	/// </summary>
	public enum Weight
	{
		One = 1,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten = 0,
		Jack = 0,
		Queen = 0,
		King = 0,

	}

	/// <summary>
	/// 存储数据类型
	/// </summary>
	[System.Serializable]
	public class GameData
	{
		public int playerIntegration;
		public int computerOneIntegration;
		public int computerTwoIntegration;
	}
}