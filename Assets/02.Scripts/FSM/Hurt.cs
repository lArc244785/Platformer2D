using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Hurt : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Hurt;
		public override bool canExecute => base.canExecute && machine.currentStateID != CharacterStateID.Die;

		private float _hurtTime;
        private float _time;

		// 기반타입이 생성자 오버로드를 가지면,
		// 하위타입에서 해당 오버로드에 인자를 전달할 수 있도록 파라미터들을 가지는 오버로드가 필요하다.
		// (최소 한개)
		public Hurt(CharacterMachine machine , float hurtTime = 0.2f)
            : base(machine)
        {
            _hurtTime = hurtTime;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.hasJumped = true;
            controller.hasDoubleJumped = true;

            controller.Stop();
            _time = _hurtTime;
            controller.invincible = true;

            animator.Play("Hurt");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            //nextID = id;

			_time -= Time.deltaTime;
            if(_time <= 0.0f)
            {
                nextID = CharacterStateID.Idle;
            }

            return nextID;
        }

		public override void OnStateExit()
		{
			base.OnStateExit();
			controller.invincible = false;
		}

	}
}
