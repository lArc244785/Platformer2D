using Platformer.FSM;
using UnityEngine;

namespace Platformer.Controllers
{
	public class PlayerController : CharacterController
	{
		public override float horizontal => Input.GetAxis("Horizontal");

		public override float vertical => Input.GetAxis("Vertical");

		private float _invincibleTimer;

		public void SetInvincible(float duration)
		{
			if (_invincibleTimer < duration)
				return;
			_invincibleTimer = duration;
		}

		protected override void Start()
		{
			base.Start();
			machine = new PlayerMachine(this);
			var machineData = StateMachineDataSheet.GetPlayerData(machine);
			machine.Init(machineData);
			onHpMin += () => machine.ChangeState(CharacterStateID.Die);
			onHpDepleted += (x) => machine.ChangeState(CharacterStateID.Hurt);
		}

		protected override void Update()
		{
			base.Update();

			if(_invincibleTimer > 0)
			{
				_invincibleTimer -= Time.deltaTime;
				if (_invincibleTimer <= 0.0f)
					invincible = false;
			}

			if (Input.GetKey(KeyCode.LeftAlt))
			{
				if (machine.ChangeState(CharacterStateID.DownJump) == false &&
					machine.ChangeState(CharacterStateID.Jump) == false &&
					Input.GetKeyDown(KeyCode.LeftAlt))
				{
					machine.ChangeState(CharacterStateID.DoubleJump);
				}
			}


			if (Input.GetKey(KeyCode.RightArrow) ||
				Input.GetKey(KeyCode.LeftArrow))
			{
				machine.ChangeState(CharacterStateID.WallSlide);
			}
			else if (machine.currentStateID == CharacterStateID.WallSlide)
			{
				machine.ChangeState(CharacterStateID.Idle);
			}

			if (Input.GetKeyDown(KeyCode.Space))
				machine.ChangeState(CharacterStateID.Slide);

			if(Input.GetKeyDown(KeyCode.UpArrow))
				machine.ChangeState(CharacterStateID.UpLadderClimb);

			if(Input.GetKeyDown(KeyCode.DownArrow))
				machine.ChangeState(CharacterStateID.DownLadderClimb);

			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				machine.ChangeState(CharacterStateID.Crouch);
			}
			else if (Input.GetKeyUp(KeyCode.DownArrow))
			{
				if (machine.currentStateID == CharacterStateID.Crouch)
					machine.ChangeState(CharacterStateID.Idle);
			}

			if (Input.GetKeyDown(KeyCode.LeftShift))
				machine.ChangeState(CharacterStateID.Dash);

			if (Input.GetKey(KeyCode.LeftControl))
				machine.ChangeState(CharacterStateID.Attack);

		}

		public override void DepleteHp(object subject, float amount)
		{
			base.DepleteHp(subject, amount);

			SetInvincible(0.7f);

			if (subject.GetType().Equals(typeof(Transform)))
			{
				Transform target = (Transform)subject;
				float dir = target.position.x - transform.position.x < 0 ? 1.0f : -1.0f;
				Knockback(Vector2.right * dir * 1.0f + Vector2.up * 1.0f);
			}
		}

	}
}