using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.State
{
    public class FallState : PlayerStateBase
    {
		[SerializeField] PlayerMover mover;
		[SerializeField] PlayerHitCheckerBase groundHitChecker;

		//--------------------------------------------------

		public override void OnFixedUpdate()
		{
			mover.Fall();

			// ’…’n‚µ‚½‚ç’Êíó‘Ô‚É‘JˆÚ
			if(groundHitChecker.IsHit) {
				StateTransition("Normal");
			}
		}

	}
}