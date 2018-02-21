using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 牌库
/// </summary>
public class Deck
{
	private const int deck_count= 8;

    private static Deck instance;
    private List<Card> library;
    private List<Card> shuf_deck;

	public delegate List<Card>[] NativeDealHand();
	public NativeDealHand deal_native_hand;

    public static Deck Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Deck();
            }
            return instance;
        }
    }

    /// <summary>
    /// 索引器
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Card this[int index]
    {
        get
        {
            return library[index];
        }
    }

    /// <summary>
    /// 私有构造
    /// </summary>
    private Deck()
    {
		ResetDeck();
    }

	public void ResetDeck(NativeDealHand deal_method =null)
	{
		deal_native_hand = deal_method;
        library = new List<Card>();
        CreateDeck(deck_count);
	}
    /// <summary>
    /// 创建n副牌
    /// </summary>
    void CreateDeck(int deck_count)
    {
		library.Clear();
		//指定几副牌
		for (int i = 0; i < deck_count; i++)
		{
			//创建普通扑克
			for (int color = 0; color < 4; color++)
			{
				for (int value = 0; value < 13; value++)
				{
					Weight w = (Weight)value;
					Suits s = (Suits)color;
					string name = s.ToString() + w.ToString();
					Card card = new Card(name, w, s);
					library.Add(card);
				}
			}
		}
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    private void Shuffle()
    {
        if (library.Count % 52 ==0)
        {
            System.Random random = new System.Random();

			shuf_deck.Clear();

            foreach (Card item in library)
            {
                shuf_deck.Insert(random.Next(shuf_deck.Count + 1), item);
            }
        }
		else
		{
			throw new System.Exception("洗牌时牌总数不是52的倍数");
		}
    }

    /// <summary>
    /// 发牌
    /// </summary>
    public Card Deal()
    {
		if(shuf_deck.Count == 0)
		{
			Shuffle();
		}
        Card ret = shuf_deck[shuf_deck.Count - 1];
        shuf_deck.Remove(ret);
        return ret;
    }

}
