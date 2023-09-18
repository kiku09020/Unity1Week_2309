using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.State
{
	public class NormalState : PlayerStateBase
	{
		[SerializeField] PlayerMover mover;

		[SerializeField] PlayerHitCheckerBase groundHitChecker;

		//--------------------------------------------------

		public override void OnFixedUpdate()
		{
			mover.Move();

			// ’n–Ê‚©‚ç—£‚ê‚½‚ç—‰ºó‘Ô‚É‘JˆÚ
			if (!groundHitChecker.IsHit) {
				StateTransition("Falling");
			}
		}
	}
}
