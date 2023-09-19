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

			// ���n������ʏ��ԂɑJ��
			if(groundHitChecker.IsHit) {
				StateTransition("Normal");
			}

			// ���S����
			if (deadHitChecker.IsHit) {
				StateTransition("Dead");
			}
		}

	}
}