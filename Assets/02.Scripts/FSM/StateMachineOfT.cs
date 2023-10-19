using Platformer.Controllers;
using CharacterControllor = Platformer.Controllers.CharacterController;
using System;
using System.Collections.Generic;

namespace Platformer.FSM 
{
	public class StateMachine<T> where T : Enum
	{
		public T currentStateID;
		protected Dictionary<T, IState<T>> states;

		public void Init(IDictionary<T, IState<T>> copy)
		{
			//copy 해서 생성
			states = new Dictionary<T, IState<T>> (copy);
		}

		public bool ChangeState(T newStateID)
		{
			if (Comparer<T>.Default.Compare(newStateID, currentStateID) == 0)
				return false;
			//바꾸려는 상태가 실행가능한지 확인하고 안되면 바꾸지 않음
			if (!states[newStateID].canExecute)
				return false;
			states[currentStateID].OnStateExit();
			currentStateID = newStateID;
			states[currentStateID].OnStateEnter();
			return true;
		}

		public void UpdateState()
		{
			ChangeState(states[currentStateID].OnStateUpdate());
		}
	}
}
