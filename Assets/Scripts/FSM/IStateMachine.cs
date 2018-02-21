using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WsFSM
{
	public interface IStateMachine
	{
		/// <summary>
		/// 当前状态
		/// </summary>
		IState CurrentState { get; }

		/// <summary>
		/// 默认状态
		/// </summary>
		IState DefaultState { get; set; }

		/// <summary>
		/// 添加状态
		/// </summary>
		/// <param name="state"></param>
		void AddState(IState state);

		/// <summary>
		/// 删除状态
		/// </summary>
		/// <param name="state"></param>
		void RemoveState(IState state);

		/// <summary>
		/// 通过标签查找状态
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		IState GetStateWithTag(string tag);
	}
}
