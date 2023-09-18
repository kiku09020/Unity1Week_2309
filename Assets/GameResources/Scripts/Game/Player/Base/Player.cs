using Game.Player.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class Player : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] PlayerData data;

        [Header("Components")]
        [SerializeField] PlayerStateMachine stateMachine;

        public PlayerData Data => data;

        //--------------------------------------------------

        void Awake()
        {
            stateMachine.StateSetup();
        }

		private void FixedUpdate()
		{
			stateMachine.OnFixedUpdate();


		}
	}
}