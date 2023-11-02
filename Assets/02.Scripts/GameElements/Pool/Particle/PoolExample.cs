using UnityEngine.Pool;
using UnityEngine;
using Platformer.GameElements.Pool.Generic;

public class PoolExample : MonoBehaviour
{
	public enum PoolType
	{
		Stack,
		LinkedList
	}

	public PoolType poolType;

	// Collection checks will throw errors if we try to release an item that is already in the pool.
	public bool collectionChecks = true;
	public int maxPoolSize = 10;
	public ParticleSystem prefab;

	IObjectPool<ParticleSystem> m_Pool;

	public static PoolExample Instance
	{
		get
		{
			return _instance;
		}
	}

	private static PoolExample _instance;

	public IObjectPool<ParticleSystem> Pool
	{
		get
		{
			if (m_Pool == null)
			{
				if (poolType == PoolType.Stack)
					m_Pool = new ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
				else
					m_Pool = new LinkedPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
			}
			return m_Pool;
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	ParticleSystem CreatePooledItem()
	{
		var ps = GameObject.Instantiate(prefab);
		ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

		// This is used to return ParticleSystems to the pool when they have stopped.
		var returnToPool = ps.gameObject.AddComponent<ReturnToPool>();
		returnToPool.pool = Pool;

		return ps;
	}

	// Called when an item is returned to the pool using Release
	void OnReturnedToPool(ParticleSystem system)
	{
		system.gameObject.SetActive(false);
	}

	// Called when an item is taken from the pool using Get
	void OnTakeFromPool(ParticleSystem system)
	{
		system.gameObject.SetActive(true);
		system.Play();
	}

	// If the pool capacity is reached then any items returned will be destroyed.
	// We can control what the destroy behavior does, here we destroy the GameObject.
	void OnDestroyPoolObject(ParticleSystem system)
	{
		Destroy(system.gameObject);
	}



}