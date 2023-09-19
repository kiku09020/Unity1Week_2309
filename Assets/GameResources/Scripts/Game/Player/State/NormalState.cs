using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.State
{
	public class NormalState : PlayerStateBase
	{
		[SerializeField] PlayerMover mover;

		[SerializeField] PlayerHitCheckerBase groundHitChecker;
		[SerializeField] PlayerEnterAbility ability;

		//--------------------------------------------------

		public override void OnFixedUpdate()
		{
			mover.Move();

			// 地面から離れたら落下状態に遷移
			if (!groundHitChecker.IsHit) {
				StateTransition("Falling");
			}

			// クリア状態に遷移
			if (GameManager.IsClear) {
				StateTransition("Clear");
			}
		}

		public override void OnUpdate()
		{
			ability.PlayerInput();
		}
	}
}
