using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WsFSM
{
	public delegate void WsStateDelegate();
	public delegate void WsStateDelegateFloat(float f);
	public delegate void WsateDelegateState(IState state);

	public class WsState : IState
	{

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string Tag
		{
			get { return _tag; }
			set { _tag = value; }
		}

		public float Timer
		{
			get { return _timer; }
		}

		public List<ITransition> Transitions
		{
			get { return _transitions; }
		}

		public IStateMachine Parent
		{
			get
			{
				return _parent;
			}

			set
			{
				_parent = value;
			}
		}

		public event WsateDelegateState OnEnter;
		public event WsateDelegateState OnExit;
		public event WsStateDelegateFloat OnUpdate;
		public event WsStateDelegateFloat OnLateUpdate;
		public event WsStateDelegate OnFixedUpdate;

		public WsState(string name)
		{
			_name = name;
			_transitions = new List<ITransition>();
		}
		public void AddTransition(ITransition t)
		{
			if (t != null && !_transitions.Contains(t))
			{
				_transitions.Add(t);
			}
		}

		public virtual void EnterCallback(IState prev)
		{
			_timer = 0f;
			if (OnEnter != null)
			{
				OnEnter(prev);
			}
		}

		public virtual void ExitCallback(IState next)
		{
			if (OnExit != null)
			{
				OnExit(next);
			}
			_timer = 0f;
		}

		public virtual void FixedUpdateCallback()
		{
			if (OnFixedUpdate != null)
			{
				OnFixedUpdate();
			}
		}

		public virtual void LateUpdateCallback(float deltaTime)
		{
			if (OnLateUpdate != null)
			{
				OnLateUpdate(deltaTime);
			}
		}

		public virtual void UpdateCallback(float deltaTime)
		{
			_timer += deltaTime;
			if (OnUpdate != null)
			{
				OnUpdate(deltaTime);
			}
		}

		private string _name;
		private string _tag;
		private List<ITransition> _transitions;
		private float _timer;
		private IStateMachine _parent;
	}
}
