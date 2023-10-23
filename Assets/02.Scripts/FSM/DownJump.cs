using UnityEngine;

namespace Platformer.FSM.Character
{
    public class DownJump : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.DownJump;
        public override bool canExecute => base.canExecute &&
                                           controller.hasJumped == false &&
                                           (machine.currentStateID == CharacterStateID.Crouch) &&
                                           controller.isGrounded &&
                                           controller.isGroundBelowExist;

        private float _jumpForce;
        private int _step;

        private Collider2D _ignoringGround;
        private float _ignoreingDistance;
        private float _passingStartPosY;

        private float _timeout = 1.0f;
        private float _timoutMark;

        public DownJump(CharacterMachine machine, float jumpForce = 1.0f, float ignoreingDistance = 0.2f)
            : base(machine)
        {
            _jumpForce = jumpForce;
            _ignoreingDistance = ignoreingDistance;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = true;
            controller.isMovable = false;
            controller.hasJumped = true;
            controller.hasDoubleJumped = false;
            animator.Play("Jump");
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
            rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            Physics2D.IgnoreCollision(collision, controller.ground, true);
            _ignoringGround = controller.ground;

			_step = 0;
        }



        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            switch (_step)
            {
                case 0:
                    if(rigidbody.velocity.y <= 0.0f)
                    {
                        animator.Play("Fall");
                        _step++;
                    }
                    break;
                case 4:
                    nextID = CharacterStateID.Idle;
                    break;
            }

            if (CheckError() < 0)
                throw new System.Exception($"[DownJump] : Somthing wrong");

            return nextID;
        }

		public override void OnStateFixedUpdate()
		{
			base.OnStateFixedUpdate();

            switch (_step)
            {
                //발이 땅에서 떨어졌는지 체크(점프 체크)
				case 1:
                    if (!controller.isGrounded)
                        _step++;
					break;
                    // 살짝 뛰었다가 원래 밟고있던 땅을 다시 지나가기 시작하는지 체크
                case 2:
                    if(controller.isGrounded)
                    {
                        if(controller.ground == _ignoringGround)
                        {
                            _passingStartPosY = rigidbody.position.y;
                        _step++;
                        }
                    }
                    break;
                    //원래 밟고있던 땅을 무시하고 일정 거리를 지나갔는지 체크, 더 이상 무시하지 않음
                case 3:
                    if(_passingStartPosY - rigidbody.position.y > _ignoreingDistance)
                    {
						Physics2D.IgnoreCollision(collision, controller.ground, false);
                        _step++;
					}
                    break;
			}

        }

        public override void OnStateExit()
		{
			base.OnStateExit();
			Physics2D.IgnoreCollision(collision, controller.ground, false);
		}

        private int CheckError()
        {
            int errorCode = 0;
            switch(_step)
            {
                case 0:
                    _timoutMark = Time.time;
				break;
                case 1:
                case 2:
                case 3:
                    if(Time.time - _timoutMark > _timeout)
                    {
                        errorCode = -1;
					}
                    break;
                default:
                    break;
            }
            return errorCode;
        }
	}
}
