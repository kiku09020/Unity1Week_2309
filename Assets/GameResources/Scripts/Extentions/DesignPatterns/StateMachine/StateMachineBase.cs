using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extention.State
{
	/// <summary> ��ԊǗ��N���X </summary>
	public class StateMachineBase<T> : MonoBehaviour where T : StateBase
	{
		[Header("State")]
		[SerializeField, Tooltip("�������")] T initState;
		[SerializeField, Tooltip("��ԃ��X�g")] protected List<T> states = new List<T>();

		/// <summary> ���݂̏�� </summary>
		public T CurrentState { get; protected set; }

		//--------------------------------------------------
		protected void Awake()
		{
			states.ForEach(state => {
				state.OnStateTransition += StateTransition;     // ��ԑJ�ڃC�x���g�ǉ�
				state.ID = states.IndexOf(state);				// ID��ݒ�
			});

			StateSetup();
		}

		//--------------------------------------------------
		/// <summary> ������ԃZ�b�g�A�b�v </summary>
		public void StateSetup()
		{
			if (CurrentState != null) {
				CurrentState.OnExit();
			}

			CurrentState = initState;
			CurrentState.OnEnter();
		}

		/// <summary> ���݂̏�Ԃ̍X�V���� </summary>
		public void OnUpdate()
		{
			CurrentState.OnUpdate();
		}


		public void OnFixedUpdate()
		{
			CurrentState.OnFixedUpdate();
		}

		//--------------------------------------------------
		/// <summary> ��ԑJ�� </summary>
		/// <param name="state">���̏��</param>
		void StateTransition(T state)
		{
			CurrentState.OnExit();
			CurrentState = state;
			CurrentState.OnEnter();
		}

		/// <summary> ��ԑJ�� </summary>
		public void StateTransition<State>() where State : T
		{
			StateTransition(GetState<State>());
		}

		/// <summary> ��ԑJ�� </summary>
		/// <param name="stateName">�J�ڐ�̏�Ԃ̖��O</param>
		public void StateTransition(string stateName)
		{
			StateTransition(GetState(stateName));
		}

		//--------------------------------------------------
		/// <summary> ���݂̏�Ԃ��w�肵����Ԃ� </summary>
		/// <param name="targetStateName"></param>
		public bool CheckCurrentState(string targetStateName)
		{
			return CurrentState == GetState(targetStateName);
		}

		public bool CheckCurrentState<State>() where State : T
		{
			return CurrentState is State;
		}

		// State����
		void CheckState<State>(Action<State> action) where State : T
		{
			foreach (var state in states) {
				if (state is State targetState) {
					action?.Invoke(targetState);
				}
			}
		}

		/// <summary> �w�肵��<typeparamref name="State"/>������΁A�����Ԃ� </summary>
		/// <typeparam name="State">�ړI�̏��</typeparam>
		/// <returns>�w�肳�ꂽ<typeparamref name="State"/></returns>
		public State GetState<State>() where State : T
		{
			State state = default(State);

			CheckState<State>((targetState) => {
				state = targetState;
			});

			if (state != null) {
				return state;
			}

			throw new Exception("�w�肳�ꂽState�͑��݂��܂���");
		}

		/// <summary> �w�肵�����O��State������΁A�����Ԃ� </summary>
		/// <param name="stateName">��Ԃ̖��O</param>
		/// <returns>�w�肳�ꂽ���O�̏��</returns>
		public T GetState(string stateName)
		{
			foreach (var state in states) {

				if (state.Name == stateName) {
					return state;
				}
			}

			throw new Exception("�w�肳�ꂽ���O��State�͑��݂��܂���");
		}
	}
}