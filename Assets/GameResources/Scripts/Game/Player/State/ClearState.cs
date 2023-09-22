using DG.Tweening;
using Game.Player.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	public class ClearState : PlayerStateBase
	{
		[SerializeField] Animator animator;

		[Header("Animation")]
		[SerializeField] float duration = 1;
		[SerializeField] Vector2 animScale = Vector2.one;
		[SerializeField] float jumpPower = 1;

		[SerializeField] Ease ease;

		//--------------------------------------------------

		public override void OnEnter()
		{
			animator.SetBool("isCleared", true);

			player.WindowController.ShowGameClearWindow();

			Animation();
		}

		void Animation()
		{
			var sequence = DOTween.Sequence();

			var jumpingTween = player.SpriteRenderer.transform.DOJump(player.transform.position, jumpPower, 1, duration);
			var jumpingScaleTween = player.SpriteRenderer.transform.DOScale(new Vector2(animScale.y, animScale.x), duration);

			var jumpStartTween = player.SpriteRenderer.transform.DOScale(animScale, duration / 2);
			var jumpEndTween = player.SpriteRenderer.transform.DOScale(Vector2.one, duration / 4);

			sequence.Append(jumpStartTween)
						.Append(jumpingTween)
						.Join(jumpingScaleTween)
						.Append(player.SpriteRenderer.transform.DOScale(animScale, duration / 8))
						.Append(jumpEndTween)

						.SetLoops(-1).SetEase(ease)
						.SetLink(player.gameObject);
		}
	}
}