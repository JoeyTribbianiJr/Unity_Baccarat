using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WsFSM
{
	public class WsStateMachine : WsState, IStateMachine
	{
		public WsStateMachine(string name, IState defaultState) : base(name)
		{
			_states = new List<IState>();
			_defaultState = defaultState;
		}

		public IState CurrentState
		{
			get
			{
				return _currentState;
			}
		}
		public IState DefaultState
		{
			get { return _defaultState; }
			set
			{
				AddState(value);
				//_defaultState = value;
			}
		}
		public override void UpdateCallback(float deltaTime)
		{
			if (_isTransition)
			{
				if(_curT.TransitionCallback ())
				{
					DoTransition(_curT);
				}
				return;
			}

			base.UpdateCallback(deltaTime);
			if(_currentState == null)
			{
				_currentState = _defaultState;
			}
			var trans = _currentState.Transitions;
			var count = trans.Count;
			for (int i = 0; i < count; i++)
			{
				var t = trans[i];
				if (t.ShouldBegin())
				{
					_curT = t;
					_isTransition = true;
					return;
				}
			}
			_currentState.UpdateCallback(deltaTime);
		}

		private void DoTransition(ITransition t)
		{
			_currentState.ExitCallback(t.To);
			_currentState = t.To;
			_currentState.EnterCallback(t.From);
			_isTransition = false;
		}

		public void AddState(IState state)
		{
			if (state != null && !_states.Contains(state))
			{
				_states.Add(state);
				state.Parent = this;
				if (_defaultState == null)
				{
					_defaultState = state;
				}
			}
		}
		public void RemoveState(IState state)
		{
			if (state != null && _states.Contains(state))
			{
				if (_currentState == state)
				{
					return;
				}
				_states.Remove(state);
				state.Parent = null;
				if (_defaultState == state)
				{
					_defaultState = _states.Count >= 1 ? _states[0] : null;
				}
			}
		}
		public IState GetStateWithTag(string tag)
		{
			return null;
		}


		private IState _defaultState;
		private IState _currentState;
		private List<IState> _states;
		private bool _isTransition = false;
		private ITransition _curT;
	}
}
