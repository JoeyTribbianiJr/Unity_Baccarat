using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WsFSM;
using UnityEngine;
using UnityEngine.UI;

public class TestFSM:MonoBehaviour
{
	public float speed = 2f;

	private WsStateMachine _fsm;
	private WsState _idle;
	private WsState _move;
	private WsTransition _idleMove;
	private WsTransition _moveIdle;
	private bool _isMove;

	private void Awake()
	{
		_idle = new WsState("idle");
		_idle.OnEnter += (IState state) =>
		{
			print("进入idle状态");
		};
		_idle.OnUpdate += (float f) => { print("idle中"); };

		_move = new WsState("move");
		_move.OnEnter += (IState state) => { print("进入move状态"); };
		_move.OnUpdate += (float f) =>
		{
			transform.position += transform.forward * f * speed;
			print("move 了一下");
		};

		_idleMove = new WsTransition("idelMove", _idle, _move);
		_idleMove.OnCheck += () =>
		{
			return _isMove;
		};
		_idle.AddTransition(_idleMove);

		_moveIdle = new WsTransition("moveIdle", _move, _idle);
		_moveIdle.OnCheck += () =>
		{
			return !_isMove;
		};
		_move.AddTransition(_moveIdle);

		_fsm = new WsStateMachine("root", _idle);
		_fsm.AddState(_move);
	}

	void Update()
	{
		_fsm.UpdateCallback(Time.deltaTime);
	}
	private void OnGUI()
	{
		if(GUILayout.Button("Move"))
		{
			_isMove = true;
		}
		if(GUILayout.Button("Idle"))
		{
			_isMove = false;
		}
	}

}