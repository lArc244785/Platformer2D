using System.Collections.Generic;
using UnityEngine;


namespace Platformer.ObjectPools
{ 

	public class ObjectPoolManager : MonoBehaviour
	{
		public static ObjectPoolManager instance
		{
			get
			{
				if (!_instance)
				{
					throw new System.Exception("ObjectPoolManger is null");
				}
				return _instance;
			}
		}

		private static ObjectPoolManager _instance;
		private Dictionary<string, IGameObjectPool> _pools;

		private void Awake()
		{
			_instance = this;
			_pools = new();

			var pools = GetComponentsInChildren<IGameObjectPool>();
			for (int i = 0; i < pools.Length; i++)
			{
				if (_pools.ContainsKey(pools[i].id))
					throw new System.Exception($"overlap key : {pools[i].id} {pools[i].GetGameObject().name}");

				_pools.Add(pools[i].id, pools[i]);
			}
		}

		public GameObject Get(string key)
		{
			if (!_pools.ContainsKey(key))
				throw new System.Exception($"Object Pool Not founed {key}");

			return _pools[key].GetGameObject();
		}
	}
}
