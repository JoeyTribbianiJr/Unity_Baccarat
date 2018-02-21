using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round {
	public List<Card>[] hand_card;
	public BetSide winner;
	public Round()
	{
		hand_card = new List<Card>[2];
	}
}
