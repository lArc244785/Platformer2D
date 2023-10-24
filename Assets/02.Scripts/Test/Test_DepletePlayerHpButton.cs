using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.Test
{
	public class Test_DepletePlayerHpButton : MonoBehaviour
	{
		private Button _button;
		[SerializeField] private float _depleteAmount = 1.0f;
		[SerializeField] private CharacterController _characterController;

		private void Awake()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(() => _characterController.DepleteHp(null, _depleteAmount));
		}
	}
}

