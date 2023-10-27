using Platformer.FSM;
using Platformer.Stats;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platformer.Controllers
{
	public enum AI
	{
		None,
		Think,
		ExectueRandomBehaviour,
		WaitUntilBehaviour,
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
		[SerializeField] private float _targetDetectRange;
		[SerializeField] private bool _autoFollow;
		[SerializeField] private bool _attackEnabled;
		[SerializeField] private float _attackRange;
		[SerializeField] private LayerMask _targetMask;
		[SerializeField] private List<CharacterStateID> _behaviours;
		[SerializeField] private float _behaviourTimeMin;
		[SerializeField] private float _behaviourTimeMax;
		private float _behaviourTimer;
		[SerializeField] private float _slopeAngle = 45.0f;

		private CapsuleCollider2D _trigger;

		protected override void Awake()
		{
			base.Awake();
			_trigger = GetComponent<CapsuleCollider2D>();
			_ai = AI.Think;
		}

		protected override void Update()
		{
			UpdateAI();

			base.Update();
		}

		private void UpdateAI()
		{
			// �ڵ� ���󰡱� �ɼ��� �����ִµ�
			if (_autoFollow)
			{
				// Ÿ���� ������
				if (_target == null)
				{
					// Ÿ�� ����
					Collider2D col
						= Physics2D.OverlapCircle(rigidbody.position, _targetDetectRange, _targetMask);

					if (col)
						_target = col.transform;
				}
			}

			// Ÿ���� �����Ǿ����� ���󰡾���
			if (_target)
			{
				_ai = AI.Follow;
			}

			switch (_ai)
			{
				case AI.Think:
					{
						_ai = AI.ExectueRandomBehaviour;
					}
					break;
				case AI.ExectueRandomBehaviour:
					{
						var nextID = _behaviours[Random.RandomRange(0, _behaviours.Count)];
						if (machine.ChangeState(nextID))
						{
							_behaviourTimer = Random.RandomRange(_behaviourTimeMin, _behaviourTimeMax);
							_horizontal = Random.Range(-1.0f, 1.0f);
							_ai = AI.WaitUntilBehaviour;
						}
						else
							_ai = AI.Think;
					}
					break;
				case AI.WaitUntilBehaviour:
					{
						if (_behaviourTimer <= 0.0f)
							_ai = AI.Think;
						else
							_behaviourTimer -= Time.deltaTime;
					}
					break;
				case AI.Follow:
					{
						// Ÿ�� ������ �ٽû�����
						if (_target == null)
						{
							_ai = AI.Think;
							return;
						}

						if(Vector2.Distance(transform.position, _target.position) > _targetDetectRange)
						{
							_target = null;
							_ai = AI.Think;
							return;
						}

						// Ÿ���� �����ʿ� ������ ����������
						if (transform.position.x < _target.position.x - _trigger.size.x)
						{
							_horizontal = 1.0f;
						}
						// Ÿ���� ���ʿ� ������ ��������
						else if (transform.position.x > _target.position.x + _trigger.size.x)
						{
							_horizontal = -1.0f;
						}

						// ���� �����ϸ鼭 ���� ���� ���� Ÿ���� �ִٸ� ����, �ƴϸ� Ÿ������ �̵�
						if (_attackEnabled &&
							Vector2.Distance(transform.position, _target.position) <= _attackRange)
						{
							machine.ChangeState(CharacterStateID.Attack);
						}
						else
						{
							machine.ChangeState(CharacterStateID.Move);
						}
					}
					break;
				default:
					break;
			}
		}

		protected override void FixedUpdate()
		{
			if (machine.currentStateID != CharacterStateID.Move)
			{
				base.FixedUpdate();
			}
			else
			{
				machine.FixedUpdateState();
				// �� �����Ӵ� �̵� �Ÿ� = �ӷ� * �� �����Ӵ� �ɸ� �ð�
				Vector2 expected = rigidbody.position + move * Time.fixedDeltaTime;
				float distanceX = Mathf.Abs(expected.x - rigidbody.position.x);
				float height = distanceX * Mathf.Tan(_slopeAngle * Mathf.Deg2Rad);
				Vector2 origin = expected + Vector2.up * height;
				RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, height * 2.0f, groundMask);
				if (hit.collider)
				{
					rigidbody.position = hit.point;
				}
			}
		}

		public override void DepleteHp(object subject, float amount)
		{
			base.DepleteHp(subject, amount);

			//int a = 1;
			//Type type = a.GetType(); // GetType() �Լ��� ���� ����Լ��������� ȣ���ؼ� �ش� ���� Ÿ���� ���� ��ü�� ��ȯ
			//type = typeof(EnemyController); // typeof Ű����� � Ÿ���� ������ ������ ��ü�� ��ȯ

			if (subject.GetType().Equals(typeof(Transform)))
				Knockback(Vector2.right * (((Transform)subject).position.x - transform.position.x < 0 ? 1.0f : -1.0f) * 1.0f);
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if ((1 << collision.gameObject.layer & _targetMask) > 0)
			{
				if (collision.TryGetComponent(out IHP target))
				{
					if (target.invincible == false)
						target.DepleteHp(transform, Random.Range(damageMin, damageMax));
				}
			}
		}

		protected override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, _targetDetectRange);

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _attackRange);
		}
	}
}