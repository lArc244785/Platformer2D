using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Platformer.ObjectPools
{
	internal interface IGameObjectPool
	{
		public string id { get; }
		public GameObject GetGameObject();
	}
}
