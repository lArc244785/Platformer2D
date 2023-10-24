using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.Test
{
	public class TestGUI : MonoBehaviour
	{
#if UNITY_EDITOR
		private CharacterController _controller;

		private void Awake()
		{
			GameObject.Find("Player")?.TryGetComponent(out _controller);
		}

		private void OnGUI()
		{
			GUI.Box(new Rect(10f, 10f, 200f, 130f), "Test");
			if (GUI.Button(new Rect(20f, 40f, 160f, 80f), "Hurt"))
				_controller?.DepleteHp(null, 1.0f);
		}
#endif
	}
}

