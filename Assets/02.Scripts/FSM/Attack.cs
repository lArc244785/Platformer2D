
using Platformer.Animations;
using Platformer.Stats;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Platformer.Datum;

namespace Platformer.FSM.Character
{
	public class Attack : CharacterStateBase
	{
		public override CharacterStateID id => CharacterStateID.Attack;
		public override bool canExecute
		{
			get
			{
				if (!base.canExecute)
					return false;

				//공격 가능 시간을 넘어간 경우 False
				float elapsedTime = Time.time - _exitTimeMark;
				if (_comboStack > 0 &&
					elapsedTime >= _comboResetTime)
				{
					_comboStack = 0;
					return false;
				}

				//공격 최대 콤보인 경우 False
				if (_comboStack >= _comboStackMax)
					return false;

				//콤보가 0 이거나 0이상에서 피격을 시켰을 경우에는 True
				if (_comboStack == 0 || (_comboStack > 0 && _hasHit) &&
					(machine.currentStateID == CharacterStateID.Idle ||
					 machine.currentStateID == CharacterStateID.Move ||
					 machine.currentStateID == CharacterStateID.Crouch ||
					 machine.currentStateID == CharacterStateID.Jump ||
					 machine.currentStateID == CharacterStateID.DownJump ||
					 machine.currentStateID == CharacterStateID.DoubleJump ||
					 machine.currentStateID == CharacterStateID.Fall))
					return true;

				return false;
			}
		}


		private int _comboStackMax;
		private int _comboStack;
		private float _comboResetTime;
		private float _exitTimeMark;
		private bool _hasHit;

		private CharacterAnimationEvents _animationEvent;

		private SkillCastSetting[] _skillCastSettings;
		private List<IHP> _targets = new();

		public Attack(CharacterMachine machine, float comboResetTime, SkillCastSetting[] skillcastSetting) : base(machine)
		{
			_skillCastSettings = skillcastSetting;
			_comboStackMax = skillcastSetting.Length;
			_comboResetTime = comboResetTime;
			_animationEvent = animator.GetComponent<CharacterAnimationEvents>();
			//targets가 있다는 전제로함
			_animationEvent.onHit += () =>
			{
				foreach (var target in _targets)
				{
					if (target == null)
						continue;

					float damage = Random.Range(controller.damageMin, controller.damageMax) * _skillCastSettings[_comboStack - 1].damageGain;
					target.DepleteHp(transform, damage);
				}
				_hasHit = true;
			};
		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			controller.isDirectionChangeable = false;
			controller.isMovable = controller.isGrounded;

			SkillCastSetting setting = _skillCastSettings[_comboStack];
			var hits2D =
				Physics2D.BoxCastAll(rigidbody.position + new Vector2(setting.castCenter.x * controller.direction, setting.castCenter.y),
									 setting.castSize,
									 0.0f,
									 Vector2.right * controller.direction,
									 setting.castDistance,
									 setting.targetMask);

			Vector2 origin = rigidbody.position + new Vector2(setting.castCenter.x * controller.direction, setting.castCenter.y);
			Vector2 size = setting.castSize;
			float distance = setting.castDistance;


			// L-T -> R-T
			Debug.DrawLine(origin + new Vector2(-size.x / 2.0f * controller.direction, +size.y / 2.0f),
						   origin + new Vector2(+size.x / 2.0f * controller.direction, +size.y / 2.0f) + Vector2.right * controller.direction * distance,
						   Color.red, animator.GetCurrentAnimatorClipInfo(0).Length);
			// L-B -> R-B
			Debug.DrawLine(origin + new Vector2(-size.x / 2.0f * controller.direction, -size.y / 2.0f),
						   origin + new Vector2(+size.x / 2.0f * controller.direction, -size.y / 2.0f) + Vector2.right * controller.direction * distance,
						   Color.red, animator.GetCurrentAnimatorClipInfo(0).Length);
			// L-T -> L-B
			Debug.DrawLine(origin + new Vector2(-size.x / 2.0f * controller.direction, +size.y / 2.0f),
						   origin + new Vector2(-size.x / 2.0f * controller.direction, -size.y / 2.0f),
						   Color.red, animator.GetCurrentAnimatorClipInfo(0).Length);
			// R-T -> R-B
			Debug.DrawLine(origin + new Vector2(+size.x / 2.0f * controller.direction, +size.y / 2.0f) + Vector2.right * controller.direction * distance,
						   origin + new Vector2(+size.x / 2.0f * controller.direction, -size.y / 2.0f) + Vector2.right * controller.direction * distance,
						   Color.red, animator.GetCurrentAnimatorClipInfo(0).Length);

			//탐색된 타겟들에서 정해진 타겟수까지만 공격 대상으로 선정
			_targets.Clear();
			for (int i = 0; i < hits2D.Length; i++)
			{
				if (_targets.Count >= setting.targetMax)
					break;
				if (hits2D[i].collider.TryGetComponent<IHP>(out var target))
				{
					_targets.Add(target);
				}
			}

			animator.SetFloat("ComboStack", _comboStack++);
			Debug.Log(_comboStack);
			animator.Play("Attack");
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
			_exitTimeMark = Time.time;

		}
		public override CharacterStateID OnStateUpdate()
		{
			CharacterStateID nextId = base.OnStateUpdate();
			if (nextId == CharacterStateID.None)
				return id;

			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
				nextId = CharacterStateID.Idle;

			return nextId;
		}



	}
}