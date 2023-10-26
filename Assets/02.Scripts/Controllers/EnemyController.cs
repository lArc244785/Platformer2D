using Platformer.FSM;
using Platformer.Stats;
using System.Collections.Generic;
using UnityEngine;
using CharacterController = Platformer.Controllers.CharacterController;
namespace Platfomer.Controllers
{

	public enum AI
	{
		None,
		Think,
		ExcuteRandomBehaviour,
		WaitUnitBehaviour,
		Follow,
	}

	public class EnemyController : CharacterController
	{
		public override float horizontal => _horizontal;

		public override float vertical => _vertical;

		private float _horizontal;
		private float _vertical;

		[SerializeField] private AI _ai;
		private Transform _target;
		[SerializeField] protected float _targetDetectRange;
		[SerializeField] private bool _autoFollow;
		[SerializeField] private bool _attackEnabled;
		[SerializeField] private float _attackRange;
		[SerializeField] private LayerMask _targetMask;
		[SerializeField] private List<CharacterStateID> _behaviours;
		[SerializeField] private float _behaviourTimeMin;
		[SerializeField] private float _behaviourTimeMax;
		private float _behaviourTimer;

		private CapsuleCollider2D _trigger;

		[SerializeField] private Vector2 _forwardGroundDetectOffset;
		[SerializeField] private float _forwardGroundDetectDistance;

		private bool isForwardGroundDetect =>
			Physics2D.Raycast((Vector2)transform.position +new Vector2(_forwardGroundDetectOffset.x * direction, _forwardGroundDetectOffset.y) ,
							   Vector2.down,
							   _forwardGroundDetectDistance, _groundMask);

		protected override void Awake()
		{
			base.Awake();
			_trigger = GetComponent<CapsuleCollider2D>();
		}


		protected override void Update()
		{
			base.Update();
			UpdateAI();
		}

		private void UpdateAI()
		{
			if (_autoFollow)
			{
				if (_target == null)
				{
					var col = Physics2D.OverlapCircle(rigidbody.position, _targetDetectRange, _targetMask);
					if (col)
						_target = col.transform;
				}
			}

			if (_target)
			{
				_ai = AI.Follow;
			}

			switch (_ai)
			{
				case AI.None:
					break;
				case AI.Think:
					break;
				case AI.ExcuteRandomBehaviour:
					break;
				case AI.WaitUnitBehaviour:
					break;
				case AI.Follow:
					if (_target == null)
					{
						_ai = AI.Think;
						break;
					}



					// 타겟이 오른쪽에 있으면 오른쪽으로
					if (transform.position.x < _target.position.x - _trigger.size.x)
						_horizontal = 1.0f;
					//타겟이 왼쪽에 있으면 왼쪽으로
					if (transform.position.x > _target.position.x + _trigger.size.x)
						_horizontal = -1.0f;

					

					if (!isForwardGroundDetect)
					{
						machine.ChangeState(CharacterStateID.Idle);
						return;
					}

					if (_attackEnabled && Vector2.Distance(transform.position, _target.position) <= _attackRange)
						machine.ChangeState(CharacterStateID.Attack);
					else
						machine.ChangeState(CharacterStateID.Move);

					break;
				default:
					break;
			}
		}

		protected override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, _targetDetectRange);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, _attackRange);

			Gizmos.color = isForwardGroundDetect ? Color.green : Color.red;
			Vector3 forwardDetectStartPos = transform.position + new Vector3(_forwardGroundDetectOffset.x * direction, _forwardGroundDetectOffset.y);
			Vector3 forwardDetectEndPos = forwardDetectStartPos + Vector3.down * _forwardGroundDetectDistance;

			Gizmos.DrawLine(forwardDetectStartPos, forwardDetectEndPos);
		}

		public override void DepleteHp(object subject, float amount)
		{
			base.DepleteHp(subject, amount);
			if (subject.GetType().Equals(typeof(Transform)))
			{
				Transform target = (Transform)subject;
				float dir = target.position.x - transform.position.x < 0 ? 1.0f : -1.0f;
				Knockback(Vector2.right * dir * 1.0f);
			}
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if ((1 << collision.gameObject.layer & _targetMask) > 0)
			{
				if (collision.TryGetComponent(out IHP target))
				{
					if (!target.invincible)
						target.DepleteHp(transform, Random.RandomRange(damageMin, damageMax));
				}
			}
		}

		protected override void Move()
		{
			if(isForwardGroundDetect)
				base.Move();
		}

	}

}

