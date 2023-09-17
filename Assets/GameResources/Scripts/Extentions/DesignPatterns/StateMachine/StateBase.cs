using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extention.State
{
    /// <summary> ��Ԓ��ۃN���X </summary>
    public abstract class StateBase : MonoBehaviour
    {
		[Header("State")]
		[SerializeField] string stateName;

		public int ID { get; set; }
		public string Name => stateName;

		/// <summary> ��ԑJ�ڃR�[���o�b�N </summary>
		public event Action<string> OnStateTransition;

		/// <summary> ��ԑJ�� </summary>
		/// <param name="nextStateName">�J�ڐ�̏�Ԗ�</param>
		protected void StateTransition(string nextStateName)
		{
			OnStateTransition?.Invoke(nextStateName);
		}

		public virtual void OnEnter() { }
		public virtual void OnUpdate() { }
		public virtual void OnFixedUpdate() { }
		public virtual void OnExit() { }
	}
}