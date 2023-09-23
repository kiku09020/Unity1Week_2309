using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	public class PlayerGroundChecker : PlayerHitCheckerBase
	{
		List<GameObject> gameObjects = new List<GameObject>();

		//--------------------------------------------------
		protected override void OnHitEnter(Collider2D collision)
		{
			base.OnHitEnter(collision);

			gameObjects.Add(collision.gameObject);
		}

		protected override void OnHitExit(Collider2D collision)
		{
			IsHit = false;

			gameObjects.Remove(collision.gameObject);

			if (gameObjects.Count != 0) {
				base.OnHitExit(collision);
				IsHit = true;
			}
		}
	}
}