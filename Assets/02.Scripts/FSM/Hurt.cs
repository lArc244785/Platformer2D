using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Hurt : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Hurt;
		public override bool canExecute => base.canExecute && 
            machine.currentStateID != CharacterStateID.Die &&
            (machine.currentStateID == CharacterStateID.Idle ||
             machine.currentStateID == CharacterStateID.Move ||
             machine.currentStateID  == CharacterStateID.Jump ||
             machine.currentStateID  == CharacterStateID.DownJump ||
             machine.currentStateID  == CharacterStateID.DoubleJump ||
             machine.currentStateID  == CharacterStateID.Fall ||
             machine.currentStateID  == CharacterStateID.Land ||
             machine.currentStateID  == CharacterStateID.Dash);

		// 기반타입이 생성자 오버로드를 가지면,
		// 하위타입에서 해당 오버로드에 인자를 전달할 수 있도록 파라미터들을 가지는 오버로드가 필요하다.
		// (최소 한개)
		public Hurt(CharacterMachine machine)
            : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.hasJumped = true;
            controller.hasDoubleJumped = true;

            controller.Stop();

            animator.Play("Hurt");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();
            if (nextID == CharacterStateID.None)
                return id;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
			{
                nextID = CharacterStateID.Idle;
            }

            return nextID;
        }

	}
}
