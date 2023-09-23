using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.State
{
    public class DeadState : PlayerStateBase
    {

		//--------------------------------------------------

		public override void OnEnter()
		{
			player.WindowController.ShowGameOverWindow();

			player.RunDeadAction();
		}
	}
}