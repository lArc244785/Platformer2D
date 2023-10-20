using UnityEngine;

namespace Platformer.FSM.Character
{
	class Move : CharacterStateBase
	{
		public Move(CharacterMachine machine) : base(machine)
		{
		}

		public override CharacterStateID id => CharacterStateID.Move;


		public override void OnStateEnter()
		{
			animator.Play("Run");
			controller.isDirectionChangeable = true;
			controller.isMoveable = true;
		}

		public override CharacterStateID OnStateUpdate()
		{
			CharacterStateID nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				return id;

			if (controller.horizontoal == 0f)
				nextID = CharacterStateID.Idle;
			if (!controller.isGrounded)
				nextID = CharacterStateID.Fall;

			return nextID;
		}


	}
}


