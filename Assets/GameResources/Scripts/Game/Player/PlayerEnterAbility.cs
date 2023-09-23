using DG.Tweening;
using Game.Stage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Player
{
	/// <summary> プレイヤーの改行能力 </summary>
	public class PlayerEnterAbility : PlayerComponentBase
	{
		[Header("Components")]
		[SerializeField] StageManager stageManager;

		[Header("Animator")]
		[SerializeField] float duration = 0.5f;
		[SerializeField] Vector2 endValue = Vector2.one;
		[SerializeField] Ease ease;

		Tween tween;

		//--------------------------------------------------

		protected override void Initialize()
		{

		}

		public void PlayerInput()
		{
			if (Input.GetKeyDown(KeyCode.Return)) {
				stageManager.OnChangedStage(player.transform);

				PlayAnimation();
				player.SEManager.PlayAudio("enterSound");
			}
		}

		void PlayAnimation()
		{
			tween?.Complete();

			tween = player.SpriteRenderer.transform.DOScale(endValue, duration)
				.SetLoops(2, LoopType.Yoyo)
				.SetEase(ease);
		}

	}
}