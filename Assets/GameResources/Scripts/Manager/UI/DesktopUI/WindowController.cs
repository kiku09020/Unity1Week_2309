using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.UI
{

	public class WindowController : MonoBehaviour
	{
		[SerializeField] GameObject gameOverWindow;
		[SerializeField] GameObject gameClearWindow;

		//--------------------------------------------------

		void Awake()
		{
			gameOverWindow.SetActive(false);
			gameClearWindow.SetActive(false);
		}

		public void ShowGameOverWindow()
		{
			gameOverWindow.SetActive(true);
		}

		public void ShowGameClearWindow()
		{
			gameClearWindow.SetActive(true);
		}

		public void HideGameOverWindow()
		{
			gameOverWindow.SetActive(false);
		}

		public void HideGameClearWindow()
		{
			gameClearWindow.SetActive(false);
		}
	}
}