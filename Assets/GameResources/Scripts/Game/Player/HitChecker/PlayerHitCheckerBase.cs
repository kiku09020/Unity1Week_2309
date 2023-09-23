using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	public class PlayerHitCheckerBase : PlayerComponentBase
	{
		[SerializeField] LayerMask hitLayerMask;

		public bool IsHit { get; protected set; }

		public event Action<Collider2D> OnHitEvent;
		public event Action<Collider2D> OnHitExitEvent;

		//--------------------------------------------------

		protected override void Initialize()
		{

		}

		protected virtual void OnHitEnter(Collider2D collision)
		{
			IsHit = true;
			OnHitEvent?.Invoke(collision);
		}
		protected virtual void OnHitExit(Collider2D collision)
		{
			IsHit = false;
			OnHitExitEvent?.Invoke(collision);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (hitLayerMask.Contains(collision.gameObject.layer)) {
				OnHitEnter(collision);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (hitLayerMask.Contains(collision.gameObject.layer)) {
				OnHitExit(collision);
			}
		}
	}
}