using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WsFSM
{
	public delegate bool WsTransitionDelegate();

	public class WsTransition : ITransition
	{
		private string _name;
		private IState _from;
		private IState _to;
		public event WsTransitionDelegate OnTransition;
		public event WsTransitionDelegate OnCheck;

		public WsTransition (string name ,IState from,IState to)
		{
			_name = name;
			_from = from;
			_to = to;
		}
		public string Name
		{
			get
			{
				return _name;
			}
		}

		public IState From {
			get { return _from; }
			set { _from = value; }
		}
		public IState To
		{
			get { return _to; }
			set { _to = value; }
		} 

		public bool TransitionCallback()
		{
			if(OnTransition != null)
			{
				return OnTransition();
			}
			return true;
		}

		public bool ShouldBegin()
		{
			if(OnCheck != null)
			{
				return OnCheck();
			}
			return false;
		}
	}
}