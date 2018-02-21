using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WsFSM
{
	public interface ITransition
	{
		string Name { get; }
		IState From { get; set; }
		IState To { get; set; }

		/// <summary>
		/// 过渡回调
		/// </summary>
		/// <returns>回调是否执行完毕</returns>
		bool TransitionCallback();

		/// <summary>
		/// 是否开始进行过渡
		/// </summary>
		/// <returns>是否可以开始</returns>
		bool ShouldBegin();
	}
}
