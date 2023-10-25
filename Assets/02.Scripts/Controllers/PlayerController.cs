using Platformer.FSM;
using UnityEngine;

namespace Platformer.Controllers
{
	public class PlayerController : CharacterController
	{
		public override float horizontal => Input.GetAxis("Horizontal");

		public override float vertical => Input.GetAxis("Vertical");


		private void Start()
		{
			machine = new PlayerMachine(this);
			var machineData = StateMachineDataSheet.GetPlayerData(machine);
			machine.Init(machineData);
			OnHpMin += () => machine.ChangeState(CharacterStateID.Die);
			OnHpDepleted += (x) => machine.ChangeState(CharacterStateID.Hurt);

			ani = GetComponentInChildren<Animator>();
		}

		Animator ani;

		protected override void Update()
		{
			base.Update();

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

			if (Input.GetKeyDown(KeyCode.D))
				OnDamage(1);

			if (Input.GetKeyDown(KeyCode.LeftShift))
				machine.ChangeState(CharacterStateID.Dash);

			//if (Input.GetKeyDown(KeyCode.UpArrow) && isLadderUpDetected)
			//	machine.ChangeState(CharacterStateID.LadderUp);



		}

		private void OnDamage(float amount)
		{
			if (invincible)
				return;

			DepleteHp(null, 1);
		}

	}
}