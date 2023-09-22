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

			// �n�ʂ��痣�ꂽ�痎����ԂɑJ��
			if (!groundHitChecker.IsHit) {
				StateTransition("Falling");
			}

			// �N���A��ԂɑJ��
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
