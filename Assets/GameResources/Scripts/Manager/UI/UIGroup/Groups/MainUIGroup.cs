using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.UI
{
    public class MainUIGroup : UIGroup
    {
		[SerializeField] GameObject mainGameObject;

		public override void Hide()
		{
			base.Hide();

			mainGameObject.SetActive(false);

			Time.timeScale = 0;
		}

		public override void Show()
		{
			base.Show();

			mainGameObject.SetActive(true);
			Time.timeScale = 1;
		}
	}
}