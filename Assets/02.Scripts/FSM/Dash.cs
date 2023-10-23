using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Dash : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Dash;
        //Idle, Move, Jump, DownJump, DoubleJump, Fall
        public override bool canExecute => base.canExecute && 
            (machine.currentStateID == CharacterStateID.Idle ||
             machine.currentStateID == CharacterStateID.Move || 
             machine.currentStateID == CharacterStateID.Jump ||
             machine.currentStateID == CharacterStateID.DoubleJump ||
             machine.currentStateID == CharacterStateID.Fall) &&
            machine.currentStateID != CharacterStateID.Dash;


        private float _ditance;
        private float _speed;
        private float _time;

        private bool _enterUpdate = false;

		// 기반타입이 생성자 오버로드를 가지면,
		// 하위타입에서 해당 오버로드에 인자를 전달할 수 있도록 파라미터들을 가지는 오버로드가 필요하다.
		// (최소 한개)
		public Dash(CharacterMachine machine , float distance = 1.0f)
            : base(machine)
        {
            _ditance = distance;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.hasJumped = false;
            controller.hasDoubleJumped = false;

            controller.Stop();

            animator.Play("Dash");
            _enterUpdate = false;

            rigidbody.bodyType = RigidbodyType2D.Kinematic;

        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();
            if (nextID == CharacterStateID.None)
                return id;

            if(!_enterUpdate)
			{
                _time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                _speed = _ditance / _time;


                _enterUpdate = true;
            }

            transform.position += Vector3.right * controller.direction * _speed * Time.deltaTime;
            _time -= Time.deltaTime;
            if (_time <= 0.0f || controller.isWallDetected)
                nextID = CharacterStateID.Idle;

            return nextID;
        }

		public override void OnStateExit()
		{
			base.OnStateExit();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
		}

	}
}
