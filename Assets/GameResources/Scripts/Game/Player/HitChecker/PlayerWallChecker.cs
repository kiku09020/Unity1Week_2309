using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	public class PlayerWallChecker : PlayerHitCheckerBase
	{
		//--------------------------------------------------

		protected override void OnHitEnter(Collider2D collision)
		{
			// 一度のみ判定
			if (!IsHit) {
				base.OnHitEnter(collision);
			}
		}

		protected override void OnHitExit(Collider2D collision)
		{
			if (IsHit) {
				base.OnHitExit(collision);
			}
		}
	}
}