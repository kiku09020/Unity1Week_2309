using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	public class PlayerMover : PlayerComponentBase
	{
		[SerializeField] PlayerHitCheckerBase wallHitChecker;
		[SerializeField] PlayerHitCheckerBase groundHitChecker;

		float moveSideDir = 1;		// is•ûŒü

		//--------------------------------------------------

		protected override void Initialize()
		{
			// G‚ê‚½‚ç”½“]
			wallHitChecker.OnHitEvent += (Collider2D collider) => {
				Flip();
			};
		}

		// ˆÚ“®
		public void Move()
		{
			player.transform.position += player.transform.right * player.Data.Movement.Speed 
											* moveSideDir * Time.deltaTime;
		}

		// ”½“]
		void Flip()
		{
			moveSideDir *= -1;

			var x = player.transform.localScale.x * -1;
			var flipedScale = new Vector3(x, 1, 1);

			player.transform.localScale = flipedScale;
		}

		// —Ž‰º
		public void Fall()
		{
			player.transform.position += Vector3.down * player.Data.Movement.FallSpeed * Time.deltaTime;
		}
	}
}