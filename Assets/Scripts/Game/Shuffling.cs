using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WsFSM;

namespace WsBaccarat
{
	public class Shuffling : WsState
	{
		public Shuffling(string name) : base(name)
		{
		}

		public override void EnterCallback(IState prev)
		{
			base.EnterCallback(prev);
		}

		public override void UpdateCallback(float deltaTime)
		{
			base.UpdateCallback(deltaTime);
		}
	}
}
