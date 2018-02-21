using UnityEngine;
using System.Collections;

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
    Ten =0,
    Jack =0,
    Queen =0,
    King=0,
    
}

/// <summary>
/// 身份
/// </summary>
public enum Identity
{
    Farmer,
    Landlord
}

/// <summary>
/// 出牌类型
/// </summary>
public enum CardsType
{
    //未知类型
    None,
    //王炸
    JokerBoom,
    //炸弹
    Boom,
    //三个不带
    OnlyThree,
    //三个带一
    ThreeAndOne,
    //三个带二
    ThreeAndTwo,
    //顺子 五张或更多的连续单牌
    Straight,
    //双顺 三对或更多的连续对牌
    DoubleStraight,
    //三顺 二个或更多的连续三张牌
    TripleStraight,
    //对子
    Double,
    //单个
    Single
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