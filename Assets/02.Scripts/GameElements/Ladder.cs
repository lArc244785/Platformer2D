using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer.GameElements
{

	public class Ladder : MonoBehaviour
	{
		public Vector2 top =>
			(Vector2)transform.position + _bound.offset +
			Vector2.up * _bound.size.y * 0.5f;
		
		public Vector2 bottom =>
			(Vector2)transform.position + _bound.offset +
			Vector2.down * _bound.size.y * 0.5f;

		public Vector2 upEnter => bottom + Vector2.down * _upEnterOffset;
		public Vector2 upExit => top + Vector2.down * _upExitOffset;
		public Vector2 downEnter => top + Vector2.down * _downEnterOffset;
		public Vector2 downExit => bottom + Vector2.down * _downExitOffset;

		public Vector2 groundEnter => bottom + Vector2.up * _groundOffset;

		public float centerX => transform.position.x + _bound.offset.x;

		[SerializeField] private float _upEnterOffset = 0.03f;
		[SerializeField] private float _upExitOffset = 0.03f;
		[SerializeField] private float _downEnterOffset = 0.05f;
		[SerializeField] private float _downExitOffset = 0.05f;
		[SerializeField] private float _groundOffset = 0f;
		private BoxCollider2D _bound;

		private void Awake()
		{
			_bound = GetComponent<BoxCollider2D>();
		}

		private void OnDrawGizmos()
		{
			_bound = GetComponent<BoxCollider2D>();
			Gizmos.color = Color.cyan;

			DrawLine(0.08f, upEnter, Color.cyan);
			DrawLine(0.08f, upExit, Color.cyan);
			DrawLine(0.08f, downEnter, Color.magenta);
			DrawLine(0.08f, downExit, Color.magenta);
			DrawLine(0.08f, groundEnter, Color.red);
		}

		private void DrawLine(float f, Vector2 pos, Color color)
		{
			Gizmos.color = color;
			Gizmos.DrawLine(Vector2.left * f + pos, Vector2.right * f + pos);
		}

	}
}
