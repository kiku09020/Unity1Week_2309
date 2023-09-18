using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	public class PlayerHitCheckerBase : PlayerComponentBase
	{
		[SerializeField] LayerMask hitLayerMask;

		public bool IsHit { get; private set; }

		public event Action<Collider2D> OnHitEvent;
		public event Action<Collider2D> OnHitExitEvent;

		//--------------------------------------------------

		protected override void Initialize()
		{
			
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (hitLayerMask.Contains(collision.gameObject.layer)) {
				IsHit = true;
				OnHitEvent?.Invoke(collision);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (hitLayerMask.Contains(collision.gameObject.layer)) {
				IsHit = false;
				OnHitExitEvent?.Invoke(collision);
			}
		}
	}
}