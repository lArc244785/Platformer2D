using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Die : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Die;

		// 기반타입이 생성자 오버로드를 가지면,
		// 하위타입에서 해당 오버로드에 인자를 전달할 수 있도록 파라미터들을 가지는 오버로드가 필요하다.
		// (최소 한개)
		public Die(CharacterMachine machine )
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
            controller.invincible = true;

            animator.Play("Die");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            return nextID;
        }

	}
}
