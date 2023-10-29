using UnityEngine;
using UnityEngine.Pool;
namespace Platformer.ObjectPools
{
	public abstract class MyObjectPoolOfT<T> : MonoBehaviour, IGameObjectPool where T : Component
	{
		//public class PooledItem : MonoBehaviour
		//{
		//	private IObjectPool<T> _pool;
		//	private T _item;

		//	public void Init(IObjectPool<T> pool)
		//	{
		//		_pool = pool;
		//	}

		//	public void ReturnToPool()
		//	{
		//		_pool.Release(_item);
		//	}

		//	private void OnDisable()
		//	{
		//		ReturnToPool();
		//	}
		//}


		public enum PoolType
		{
			Stack,
			LinkedList,
		}

		[SerializeField] private PoolType _collectionType;
		[SerializeField] protected T prefab;
		[SerializeField] private int _capacity;
		[SerializeField] private int _sizeMax;
		[SerializeField] private bool _collectionTypeCheck;
		[SerializeField] private string _id;

		public int count {protected set; get; }
		public string id => _id;

		private IObjectPool<T> _pool;

		public IObjectPool<T> pool
		{
			get
			{
				if (_pool == null)
				{
					if (_collectionType == PoolType.Stack)
					{
						_pool = new ObjectPool<T>(CreatePooledItem,
															OnGetFromPool,
															OnRetrunToPool,
															OnDestroyPooledItem,
															_collectionTypeCheck,
															_capacity,
															_sizeMax);
					}
					else
					{
						_pool = new LinkedPool<T>(CreatePooledItem,
															OnGetFromPool,
															OnRetrunToPool,
															OnDestroyPooledItem,
															_collectionTypeCheck,
															_sizeMax);
					}
				}
				return _pool;
			}
		}


		public GameObject GetGameObject()
		{
			return pool.Get().gameObject;
		}

		protected abstract T CreatePooledItem();
		protected virtual void OnGetFromPool(T item)
		{
			item.gameObject.SetActive(true);
		}
		protected virtual void OnRetrunToPool(T item)
		{
			item.gameObject.SetActive(false);
		}
		protected virtual void OnDestroyPooledItem(T item)
		{
			Destroy(item.gameObject);
		}
	}
}
