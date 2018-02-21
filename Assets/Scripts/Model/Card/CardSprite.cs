using UnityEngine;
using System.Collections;

namespace WsBaccarat
{
	/// <summary>
	/// 用于卡片的显示
	/// </summary>
	public class CardSprite : MonoBehaviour
	{
		private Card card;
		//public UISprite sprite;
		private bool isSelected;

		void Start()
		{
		}

		/// <summary>
		/// sprite所装载的card
		/// </summary>
		public Card Poker
		{
			set
			{
				card = value;
				card.isSprite = true;
				SetSprite();
			}
			get { return card; }
		}

		/// <summary>
		/// 是否被点击中
		/// </summary>
		public bool Select
		{
			set { isSelected = value; }
			get { return isSelected; }
		}

		/// <summary>
		/// 设置UISprite的显示
		/// </summary>
		void SetSprite()
		{
			//if (card.Attribution == CharacterType.Player || card.Attribution == CharacterType.Desk)
			//{
			//    sprite.spriteName = card.GetCardName;
			//}
			//else
			//{
			//    sprite.spriteName = "SmallCardBack1";
			//}
		}

		/// <summary>
		/// 销毁精灵
		/// </summary>
		public void Destroy()
		{
			//精灵化false
			card.isSprite = false;
			//销毁对象
			Destroy(this.gameObject);
		}

		/// <summary>
		/// 调整位置
		/// </summary>
		public void GoToPosition(GameObject parent, int index)
		{
			//sprite.depth = index;
			//if (card.Attribution == CharacterType.Player)
			//{
			//    transform.localPosition =
			//        parent.transform.Find("CardsStartPoint").localPosition + Vector3.right * 25 * index;
			//    if (isSelected)
			//    {
			//        transform.localPosition += Vector3.up * 10;
			//    }
			//}
			//else if (card.Attribution == CharacterType.ComputerOne ||
			//    card.Attribution == CharacterType.ComputerTwo)
			//{
			//    transform.localPosition =
			//        parent.transform.Find("CardsStartPoint").localPosition + Vector3.up * -25 * index;
			//}
			//else if (card.Attribution == CharacterType.Desk)
			//{
			//    transform.localPosition =
			//        parent.transform.Find("PlacePoint").localPosition + Vector3.right * 25 * index;
			//}

		}

		/// <summary>
		/// 卡牌点击
		/// </summary>
		public void OnClick()
		{
			//if (card.Attribution == CharacterType.Player)
			//{
			//    if (isSelected)
			//    {
			//        transform.localPosition -= Vector3.up * 10;
			//        isSelected = false;
			//    }
			//    else
			//    {
			//        transform.localPosition += Vector3.up * 10;
			//        isSelected = true;
			//    }
			//}
		}
	}
}
