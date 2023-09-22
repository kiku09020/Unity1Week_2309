using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	public class PlayerGroundChecker : PlayerHitCheckerBase
	{
		List<GameObject> gameObjects = new List<GameObject>();

		//--------------------------------------------------

		protected override void Initialize()
		{
			base.Initialize();

			OnHitEvent += AddToList;
			OnHitExitEvent += RemoveFromList;
		}

		void AddToList(Collider2D collision)
		{
			gameObjects.Add(collision.gameObject);
		}

		void RemoveFromList(Collider2D collision)
		{
			gameObjects.Remove(collision.gameObject);

			if (gameObjects.Count != 0) {
				IsHit = true;
			}
		}
	}
}