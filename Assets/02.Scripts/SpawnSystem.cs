using Platformer.ObjectPools;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
	//���� ����
	[SerializeField] private Vector2 _size;
	//���� ����
	private Vector2 _lefTopPos;
	private Vector2 _rightBottomPos;

	[SerializeField] private LayerMask _groundMask;

	//�ֱ�
	[SerializeField] private float _spawnCycle;
	private float _time;

	private void Awake()
	{
		_lefTopPos = (Vector2)transform.position + new Vector2(-0.5f, 0.5f) * _size;
		_rightBottomPos = (Vector2)transform.position + new Vector2(0.5f, -0.5f) * _size;
		_time = _spawnCycle;
	}


	public Vector2 RandomSpawn()
	{
		float x = Random.RandomRange(_lefTopPos.x, _rightBottomPos.x);
		float y = Random.RandomRange(_lefTopPos.y, _rightBottomPos.y);
		Vector2 randomPos = new Vector2(x, y);

		var hit = Physics2D.Raycast(randomPos, Vector2.down, Mathf.Infinity, _groundMask);

		Debug.DrawLine(randomPos, hit.point, Color.red, 1.0f);

		return hit.point;
	}

	private void Update()
	{
		_time -= Time.deltaTime;
		if (_time > 0.0f)
			return;

		_time = _spawnCycle;
		var slug = ObjectPoolManager.instance.Get("Slug");
		slug.transform.position = RandomSpawn();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, _size);
	}


}
