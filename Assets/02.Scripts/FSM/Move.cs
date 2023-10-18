using UnityEngine;

namespace Platformer.FSM.Character
{
	class Move : CharacterStateBase
	{
		public override CharacterStateID id => CharacterStateID.Move;
		public Move(StateMachine<CharacterStateID> machine) : base(machine) { }

		public override void OnStateEnter()
		{
			animator.Play("Move");
		}

		public override CharacterStateID OnStateUpdate()
		{
			if (controller.horizontoal == 0f)
				return CharacterStateID.Idle;

			return id;
		}


	}
}


