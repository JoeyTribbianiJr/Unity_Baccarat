using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WsFSM
{
	public interface IState
	{
		/// <summary>
		/// 名字
		/// </summary>
		string Name { get; }

		/// <summary>
		/// 标签
		/// </summary>
		string Tag { get; set; }

		/// <summary>
		/// 进入状态开始计算的时长
		/// </summary>
		float Timer { get; }

		IStateMachine Parent { get; set; }
		/// <summary>
		/// 状态过渡
		/// </summary>
		List<ITransition> Transitions { get; }

		void AddTransition(ITransition t);

		void EnterCallback(IState prev);
		void ExitCallback(IState next);
		void UpdateCallback(float deltaTime);
		void LateUpdateCallback(float deltaTime);
		void FixedUpdateCallback();


	}
}
