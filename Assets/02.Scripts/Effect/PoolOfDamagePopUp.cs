using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.Effects
{
	public class PoolOfDamagePopUp : MonoBehaviour
	{
		public class PooledItem : MonoBehaviour
		{
			public IObjectPool<DamagePopUp> pool;
			private DamagePopUp _damagePopUp;

			private void Awake()
			{
				_damagePopUp = GetComponent<DamagePopUp>();
			}

			public void ReturnToPool()
			{
				pool.Release(_damagePopUp);
			}

			private void OnDisable()
			{
				ReturnToPool();
			}
		}

		public enum PoolType
		{
			Stack,
			LinkedList,
		}

		[SerializeField] private PoolType _collectionType;

		public IObjectPool<DamagePopUp> pool
		{
			get
			{
				if (_pool == null)
				{
					if (_collectionType == PoolType.Stack)
					{
						_pool = new ObjectPool<DamagePopUp>(CreatePooledItem,
															OnGetFromPool,
															OnRetrunToPool,
															OnDestroyPooledItem,
															_collectionTypeCheck,
															_count,
															_countMax);
					}
					else
					{
						_pool = new LinkedPool<DamagePopUp>(CreatePooledItem,
															OnGetFromPool,
															OnRetrunToPool,
															OnDestroyPooledItem,
															_collectionTypeCheck,
															_countMax);
					}
				}
					return _pool;
			}
		}

		private IObjectPool<DamagePopUp> _pool;
		[SerializeField] private DamagePopUp _prefab;
		[SerializeField] private int _count;
		[SerializeField] private int _countMax;
		[SerializeField] private bool _collectionTypeCheck;

		private DamagePopUp CreatePooledItem()
		{
			var item = Instantiate(_prefab);
			item.gameObject.AddComponent<PooledItem>().pool = pool;
			return item;
		}

		private void OnGetFromPool(DamagePopUp damagePopup)
		{
			damagePopup.gameObject.SetActive(true);
		}

		private void OnRetrunToPool(DamagePopUp damagePopup)
		{
			damagePopup.gameObject.SetActive(false);
		}

		private void OnDestroyPooledItem(DamagePopUp damagePopup)
		{
			Destroy(damagePopup.gameObject);
		}

	}

}
