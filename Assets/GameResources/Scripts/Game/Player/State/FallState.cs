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

			// 着地したら通常状態に遷移
			if(groundHitChecker.IsHit) {
				StateTransition("Normal");
			}
		}

	}
}