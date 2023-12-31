using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.State
{
    public class FallState : PlayerStateBase
    {
		[SerializeField] PlayerMover mover;
		[SerializeField] PlayerHitCheckerBase groundHitChecker;

		[SerializeField] PlayerHitCheckerBase deadHitChecker;

		//--------------------------------------------------

		public override void OnFixedUpdate()
		{
			mover.Fall();

			// 着地したら通常状態に遷移
			if(groundHitChecker.IsHit) {
				StateTransition("Normal");
			}

			// 死亡判定
			if (deadHitChecker.IsHit) {
				StateTransition("Dead");
			}
		}

	}
}