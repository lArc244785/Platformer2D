using UnityEngine;
using UnityEngine.Pool;
using Platformer.Effects;
using Platformer.Controllers;

namespace Platformer.ObjectPools
{
	public class PoolOfEnemyCharacter : MyObjectPoolOfT<EnemyController>
	{
		public class PooledItem : MonoBehaviour
		{
			private IObjectPool<EnemyController> _pool;
			private EnemyController _item;

			public void Init(IObjectPool<EnemyController> pool, EnemyController item)
			{
				_item = item;
				_pool = pool;
			}

			public void ReturnToPool()
			{
				_pool.Release(_item);
			}

			private void OnDisable()
			{
				ReturnToPool();
			}
		}


		protected override EnemyController CreatePooledItem()
		{
			var item = Instantiate(prefab);
			var pooledItem = item.gameObject.AddComponent<PooledItem>();
			pooledItem.Init(pool, item);

			item.name = $"{id} {count++}";
			return item;
		}
	}

}
