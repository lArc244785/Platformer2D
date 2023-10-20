using UnityEngine;

namespace Platformer.FSM.Character
{
	class Land : CharacterStateBase
	{
		public Land(CharacterMachine machine) : base(machine)
		{
		}

		public override CharacterStateID id => CharacterStateID.Land;


		public override void OnStateEnter()
		{
			base.OnStateEnter();
			controller.isDirectionChangeable = false;
			controller.isMoveable = false;
			controller.Stop();

			animator.Play("Land");
		}

		public override CharacterStateID OnStateUpdate()
		{
			CharacterStateID nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				return id;

			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
				nextID = CharacterStateID.Idle;


			return nextID;
		}
	}
}


