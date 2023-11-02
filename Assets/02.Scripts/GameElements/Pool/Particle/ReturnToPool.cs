using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ReturnToPool : MonoBehaviour
{
	public ParticleSystem system;
	public IObjectPool<ParticleSystem> pool;

	void Start()
	{
		system = GetComponent<ParticleSystem>();
	}

	void OnParticleSystemStopped()
	{
		// Return to the pool
		pool.Release(system);
	}
}
