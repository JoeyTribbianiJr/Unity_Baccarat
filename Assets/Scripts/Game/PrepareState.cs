using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WsFSM;

namespace WsBaccarat
{
	public class PrepareState : WsState
	{
		public PrepareState(string name) : base(name)
		{
		}

		public override void UpdateCallback(float deltaTime)
		{
			base.UpdateCallback(deltaTime);
		}
	}
}
