using UnityEngine;
using UnityEngine.Pool;
using Platformer.Effects;

namespace Platformer.ObjectPools
{
	public class PoolOfDamagePopUp : MyObjectPoolOfT<DamagePopUp>
	{
		public class PooledItem : MonoBehaviour
		{
			private IObjectPool<DamagePopUp> _pool;
			private DamagePopUp _item;

			public void Init(IObjectPool<DamagePopUp> pool, DamagePopUp item)
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


		protected override DamagePopUp CreatePooledItem()
		{
			var item = Instantiate(prefab);
			var pooledItem = item.gameObject.AddComponent<PooledItem>();
			pooledItem.Init(pool, item);

			item.name = $"{id} {count++}";
			return item;
		}
	}

}
