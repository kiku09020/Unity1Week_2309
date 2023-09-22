using Game.Player.State;
using GameController.Audio;
using GameController.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Player
{
	public class Player : MonoBehaviour
	{
		[Header("Data")]
		[SerializeField] PlayerData data;

		[Header("Components")]
		[SerializeField] PlayerStateMachine stateMachine;
		[SerializeField] SpriteRenderer rend;
		[SerializeField] WindowController windowController;
		[SerializeField] SEManager seManager;

		public PlayerData Data => data;
		public SpriteRenderer SpriteRenderer => rend;
		public WindowController WindowController => windowController;
		public SEManager SEManager => seManager;


		public event Action OnDeadEvent;

		public void RunDeadAction()=> OnDeadEvent?.Invoke();
		//--------------------------------------------------

		void Awake()
		{
			stateMachine.StateSetup();
		}

		private void FixedUpdate()
		{
			stateMachine.OnFixedUpdate();
		}

		private void Update()
		{
			stateMachine.OnUpdate();

			if (Input.GetKeyDown(KeyCode.R)) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}
}