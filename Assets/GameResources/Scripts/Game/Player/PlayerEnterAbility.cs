using Game.Stage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Player
{
    /// <summary> �v���C���[�̉��s�\�� </summary>
    public class PlayerEnterAbility : PlayerComponentBase
    {
		[SerializeField] StageManager stageManager;

		//--------------------------------------------------

		protected override void Initialize()
		{
			
		}

		public void PlayerInput()
		{
			if (Input.GetKeyDown(KeyCode.Return)) {
				stageManager.OnChangedStage(player.transform);
			}
		}

	}
}