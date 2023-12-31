using Extention.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.State
{
	public abstract class PlayerStateBase : StateBase
	{
		protected Player player;

		//--------------------------------------------------

		private void Awake()
		{
			player = GetComponentInParent<Player>();
		}
	}
}