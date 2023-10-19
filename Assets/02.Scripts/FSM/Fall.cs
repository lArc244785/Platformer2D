using UnityEngine;

namespace Platformer.FSM.Character
{
	class Fall : CharacterStateBase
	{
		public Fall(CharacterMachine machine) : base(machine)
		{
		}

		public override CharacterStateID id => CharacterStateID.Fall;
		public override bool canExecute => base.canExecute && 
			(machine.currentStateID == CharacterStateID.Idle || 
			machine.currentStateID == CharacterStateID.Move ||
			machine.currentStateID == CharacterStateID.Jump);

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			controller.isMoveable = false;
			controller.isDirectionChangeable = true;
			animator.Play("Fall");
		}

		public override CharacterStateID OnStateUpdate()
		{
			CharacterStateID nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				return id;

			if (controller.isGrounded)
				nextID = CharacterStateID.Idle;

			return nextID;
		}
	}
}


