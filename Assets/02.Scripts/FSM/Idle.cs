using UnityEngine;

namespace Platformer.FSM.Character
{
	class Idle : CharacterStateBase
	{
		public Idle(CharacterMachine machine) : base(machine)
		{
		}

		public override CharacterStateID id => CharacterStateID.Idle;


		public override void OnStateEnter()
		{
			base.OnStateEnter();
			controller.isDirectionChangeable = true;
			controller.isMoveable = true;
			controller.hasDoubleJumped = false;
			controller.hasJumped = false;
			animator.Play("Idle");
		}

		public override CharacterStateID OnStateUpdate()
		{
			CharacterStateID nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				return id;

			if (Mathf.Abs(controller.horizontoal) > 0f)
				nextID = CharacterStateID.Move;
			if (!controller.isGrounded)
				nextID = CharacterStateID.Fall;


			return nextID;
		}
	}
}


