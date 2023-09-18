using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
	/// <summary> �v���C���[�̃R���|�[�l���g�̊��N���X </summary>
	public abstract class PlayerComponentBase : MonoBehaviour
	{
		protected Player player;

		//--------------------------------------------------

		void Start()
		{
			player = GetComponentInParent<Player>();

			Initialize();
		}

		/// <summary> ���������� </summary>
		protected abstract void Initialize();
	}
}