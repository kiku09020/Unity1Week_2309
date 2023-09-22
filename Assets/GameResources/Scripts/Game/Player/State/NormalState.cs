using Game.Stage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.State
{
	public class NormalState : PlayerStateBase
	{
		[SerializeField] PlayerMover mover;
		[SerializeField] StageSelector selector;

		[SerializeField] PlayerHitCheckerBase groundHitChecker;
		[SerializeField] PlayerEnterAbility ability;

		//--------------------------------------------------

		public override void OnFixedUpdate()
		{
			selector.SetSelectionTile(player.transform.position);
			mover.Move();

			// ’n–Ê‚©‚ç—£‚ê‚½‚ç—‰ºó‘Ô‚É‘JˆÚ
			if (!groundHitChecker.IsHit) {
				StateTransition("Falling");
			}

			// ƒNƒŠƒAó‘Ô‚É‘JˆÚ
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
