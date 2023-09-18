using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	public class PlayerMover : PlayerComponentBase
	{
		[SerializeField] PlayerHitCheckerBase wallHitChecker;
		[SerializeField] PlayerHitCheckerBase groundHitChecker;

		float moveSideDir = 1;		// �i�s����

		//--------------------------------------------------

		protected override void Initialize()
		{
			// �G�ꂽ�甽�]
			wallHitChecker.OnHitEvent += (Collider2D collider) => {
				Flip();
			};
		}

		// �ړ�
		public void Move()
		{
			player.transform.position += player.transform.right * player.Data.Movement.Speed 
											* moveSideDir * Time.deltaTime;
		}

		// ���]
		void Flip()
		{
			moveSideDir *= -1;

			var x = player.transform.localScale.x * -1;
			var flipedScale = new Vector3(x, 1, 1);

			player.transform.localScale = flipedScale;
		}

		// ����
		public void Fall()
		{
			player.transform.position += Vector3.down * player.Data.Movement.FallSpeed * Time.deltaTime;
		}
	}
}