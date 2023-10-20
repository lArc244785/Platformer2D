using System;
using System.Diagnostics;

namespace Platformer.FSM
{
	public abstract class StateBase<T> : IState<T>
		where T : Enum
	{
		public abstract T id { get; }
		public virtual bool canExecute => true;

		private bool _hasFixedUpdated;

		protected StateMachine<T> machine;

		public StateBase(StateMachine<T> machine)
		{
			this.machine = machine;
		}

		public virtual void OnStateEnter()
		{
			UnityEngine.Debug.Log($"State Enter to {id}");
			_hasFixedUpdated = false;
		}

		public virtual void OnStateExit()
		{
		}

		public virtual void OnStateFixedUpdate()
		{
            if (!_hasFixedUpdated)
            {
				_hasFixedUpdated = true;
            }
        }

		public virtual T OnStateUpdate()
		{
			return _hasFixedUpdated ? id : default(T);
		}
	}
}

